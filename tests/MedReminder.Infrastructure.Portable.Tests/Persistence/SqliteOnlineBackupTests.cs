using FluentAssertions;
using MedReminder.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Xunit;

namespace MedReminder.Infrastructure.Tests.Persistence;

// The online-backup copy is retried while another connection holds a lock
// on the source (SQLITE_BUSY), and gives up after the last retry.
public sealed class SqliteOnlineBackupTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "mr-online-backup-" + Guid.NewGuid().ToString("N"));

    public SqliteOnlineBackupTests() => Directory.CreateDirectory(_root);

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        try { Directory.Delete(_root, recursive: true); } catch (IOException) { }
    }

    private string Source => Path.Combine(_root, "source.db");

    private string Destination => Path.Combine(_root, "copy.db");

    private static SqliteConnection Open(string path)
    {
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = path, Pooling = false }.ToString());
        connection.Open();
        return connection;
    }

    private static void Execute(SqliteConnection connection, string sql)
    {
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    private static long Count(string path)
    {
        using var connection = Open(path);
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM item";
        return (long)command.ExecuteScalar()!;
    }

    // A rollback-journal database whose writer holds an exclusive lock: a
    // reader, the backup included, gets SQLITE_BUSY until it commits.
    private SqliteConnection LockedSource()
    {
        using (var setup = Open(Source))
        {
            Execute(setup, "CREATE TABLE item (id INTEGER PRIMARY KEY); INSERT INTO item DEFAULT VALUES;");
        }
        var writer = Open(Source);
        Execute(writer, "BEGIN EXCLUSIVE; INSERT INTO item DEFAULT VALUES;");
        return writer;
    }

    [Fact]
    public async Task A_copy_waits_for_a_lock_to_be_released()
    {
        using var writer = LockedSource();
        var delays = new List<TimeSpan>();

        await SqliteOnlineBackup.CopyAsync(Source, Destination, (wait, _) =>
        {
            delays.Add(wait);
            if (delays.Count == 2) Execute(writer, "COMMIT;");
            return Task.CompletedTask;
        }, CancellationToken.None);

        delays.Should().Equal(SqliteOnlineBackup.RetryDelays.Take(2));
        Count(Destination).Should().Be(2, "the copy holds the committed write");
    }

    [Fact]
    public async Task A_lock_that_outlasts_the_retries_is_thrown()
    {
        using var writer = LockedSource();
        var waits = 0;

        var act = () => SqliteOnlineBackup.CopyAsync(Source, Destination, (_, _) =>
        {
            waits++;
            return Task.CompletedTask;
        }, CancellationToken.None);

        (await act.Should().ThrowAsync<SqliteException>()).Which.SqliteErrorCode.Should().Be(5);
        waits.Should().Be(SqliteOnlineBackup.RetryDelays.Count);
    }

    [Fact]
    public async Task An_unlocked_database_is_copied_at_once()
    {
        using (var setup = Open(Source))
        {
            Execute(setup, "CREATE TABLE item (id INTEGER PRIMARY KEY); INSERT INTO item DEFAULT VALUES;");
        }

        await SqliteOnlineBackup.CopyAsync(Source, Destination,
            (_, _) => throw new InvalidOperationException("No retry expected."), CancellationToken.None);

        Count(Destination).Should().Be(1);
    }
}
