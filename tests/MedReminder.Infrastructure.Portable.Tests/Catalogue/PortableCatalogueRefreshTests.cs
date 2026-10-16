using System.Security.Cryptography;
using FluentAssertions;
using MedReminder.Application;
using MedReminder.Application.Abstractions;
using MedReminder.Application.Catalogue;
using MedReminder.Infrastructure.Catalogue.Parsers;
using MedReminder.Infrastructure.Email;
using MedReminder.Infrastructure.Persistence;
using MedReminder.Infrastructure.Profiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace MedReminder.Infrastructure.Portable.Tests.Catalogue;

// Android backlog B4-02: a host that references only Application and
// Infrastructure.Portable refreshes a reference catalogue from its feed
// into the open profile's database, as the desktop does.
public sealed class PortableCatalogueRefreshTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "mr-catalogue-" + Guid.NewGuid().ToString("N"));

    public void Dispose()
    {
        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
        try { Directory.Delete(_root, recursive: true); } catch (IOException) { }
    }

    [Fact]
    public async Task Portable_host_imports_a_feed_and_finds_it_up_to_date_next_time()
    {
        byte[] payload;
        await using (var snapshot = CatalogueFixtures.BuildAifaSnapshotStream())
        using (var copy = new MemoryStream())
        {
            await snapshot.CopyToAsync(copy);
            payload = copy.ToArray();
        }
        var manifest = new CatalogueFeedManifest("202610", "aifa-202610.zip", null,
            Convert.ToHexStringLower(SHA256.HashData(payload)), payload.Length);
        var feed = new FixedFeed(manifest, payload);

        await using var provider = await BuildAsync(feed);

        await using (var scope = provider.CreateAsyncScope())
        {
            scope.ServiceProvider.GetServices<IReferenceSnapshotParser>().Should().HaveCount(5);
            var outcome = await scope.ServiceProvider.GetRequiredService<RemoteCatalogueRefresher>()
                .RunAsync(CatalogueFeedDescriptor.Italy, CancellationToken.None);
            outcome.Should().Be(RemoteCatalogueRefreshOutcome.Imported);
        }

        await using (var scope = provider.CreateAsyncScope())
        {
            var state = await scope.ServiceProvider.GetRequiredService<IReferenceCatalogueImporter>()
                .GetImportStateAsync(CatalogueFeedDescriptor.Italy.Country, CancellationToken.None);
            state.Version.Should().Be("202610");
            state.RowCount.Should().BePositive();

            var again = await scope.ServiceProvider.GetRequiredService<RemoteCatalogueRefresher>()
                .RunAsync(CatalogueFeedDescriptor.Italy, CancellationToken.None);
            again.Should().Be(RemoteCatalogueRefreshOutcome.UpToDate);
        }

        feed.Downloads.Should().Be(1);
        Directory.EnumerateFiles(RemoteCatalogueRefresher.GetStagingDirectory(_root)).Should().BeEmpty(
            "the staged archive is deleted after the import");
    }

    private async Task<ServiceProvider> BuildAsync(ICatalogueFeedClient feed)
    {
        var profilesRoot = Path.Combine(_root, "profiles");
        var registry = new ProfileRegistry(Path.Combine(_root, "profiles.json"), profilesRoot, TimeProvider.System);
        var current = new CurrentProfile(registry.Create("Anna", ProfileRole.Admin), profilesRoot);

        var services = new ServiceCollection();
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        services.AddSingleton<IAppDataLocation>(new Location(_root));
        services.AddSingleton<IProfileRegistry>(registry);
        services.AddSingleton<ICurrentProfile>(current);
        services.Configure<SmtpSettings>(_ => { });
        services.Configure<BackupSettings>(_ => { });
        services.Configure<UserSettings>(_ => { });
        services.AddSingleton(feed);
        services.AddMedReminderApplication();
        services.AddMedReminderPortableInfrastructure(current.DatabasePath);
        services.AddMedReminderPortableProfileSettings(profilesRoot);

        var provider = services.BuildServiceProvider();
        await using var scope = provider.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<DatabaseInitializer>().InitializeAsync(CancellationToken.None);
        return provider;
    }

    private sealed class Location(string directory) : IAppDataLocation
    {
        public string DataDirectory => directory;
    }

    // Serves one manifest and its archive, as raw.githubusercontent.com
    // would.
    private sealed class FixedFeed(CatalogueFeedManifest manifest, byte[] payload) : ICatalogueFeedClient
    {
        public int Downloads { get; private set; }

        public Task<CatalogueFeedManifest?> GetLatestAsync(CatalogueFeedDescriptor feed, CancellationToken cancellationToken)
            => Task.FromResult<CatalogueFeedManifest?>(feed == CatalogueFeedDescriptor.Italy ? manifest : null);

        public async Task<CatalogueFeedDownload> DownloadAsync(
            CatalogueFeedDescriptor feed, CatalogueFeedManifest m, string destinationPath, CancellationToken cancellationToken)
        {
            Downloads++;
            await File.WriteAllBytesAsync(destinationPath, payload, cancellationToken);
            return new CatalogueFeedDownload(payload.Length, Convert.ToHexStringLower(SHA256.HashData(payload)));
        }
    }
}
