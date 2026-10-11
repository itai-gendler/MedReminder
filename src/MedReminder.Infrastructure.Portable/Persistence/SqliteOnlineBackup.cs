using Microsoft.Data.Sqlite;

namespace MedReminder.Infrastructure.Persistence;

// A consistent copy of a profile database through the SQLite online-backup
// API, shared by the desktop's BackupService and the portable
// ProfileArchive (export, cloud backup).
//
// The backup step fails at once with SQLITE_BUSY or SQLITE_LOCKED when
// another connection holds a lock it needs: a write being committed, or
// the checkpoint SQLite runs when the last connection of a WAL database
// closes (for instance when SqliteConnection.ClearAllPools closes an idle
// pooled connection). Such a lock lasts milliseconds, so the copy is tried
// again after short delays before the error is thrown. Each attempt starts
// from a new destination file. The connections are not pooled: the
// destination file is removed between attempts and must not stay open.
public static class SqliteOnlineBackup
{
    private const int Busy = 5;
    private const int Locked = 6;

    // The waits between attempts: about 4 seconds in all.
    public static IReadOnlyList<TimeSpan> RetryDelays { get; } =
    [
        TimeSpan.FromMilliseconds(100),
        TimeSpan.FromMilliseconds(250),
        TimeSpan.FromMilliseconds(500),
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(2),
    ];

    public static Task CopyAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken)
        => CopyAsync(sourcePath, destinationPath, Task.Delay, cancellationToken);

    // delay: the wait between attempts, replaced in tests.
    internal static async Task CopyAsync(string sourcePath, string destinationPath,
        Func<TimeSpan, CancellationToken, Task> delay, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourcePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationPath);

        var source = new SqliteConnectionStringBuilder(SqliteConnectionStrings.ForFile(sourcePath))
        {
            Cache = SqliteCacheMode.Private,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Pooling = false,
        }.ToString();
        var destination = new SqliteConnectionStringBuilder
        {
            DataSource = destinationPath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Pooling = false,
        }.ToString();

        for (var attempt = 0; ; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await using var from = new SqliteConnection(source);
                await using var to = new SqliteConnection(destination);
                await from.OpenAsync(cancellationToken);
                await to.OpenAsync(cancellationToken);
                // Synchronous: off the caller's thread.
                await Task.Run(() => from.BackupDatabase(to), cancellationToken);
                return;
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode is Busy or Locked && attempt < RetryDelays.Count)
            {
                DeleteDestination(destinationPath);
                await delay(RetryDelays[attempt], cancellationToken);
            }
        }
    }

    private static void DeleteDestination(string path)
    {
        foreach (var file in new[] { path, path + "-journal", path + "-wal", path + "-shm" })
        {
            try
            {
                if (File.Exists(file)) File.Delete(file);
            }
            catch (IOException)
            {
                // The next attempt overwrites the content anyway.
            }
        }
    }
}
