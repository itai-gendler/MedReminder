using System.IO.Enumeration;
using MedReminder.Application.Abstractions;

namespace MedReminder.Infrastructure.Sync;

// ISyncTransport over a folder (B.1 Phase 3c, docs/analysis/
// ANALYSIS-B1-MOBILE-SYNC.md §5.8): a folder that a third-party client
// (OneDrive, Google Drive, Dropbox desktop) or a NAS share keeps in sync
// between PCs. The folder is chosen by the user, like the cloud-backup
// folder of C.3+.
//
// A file is written under a temporary name starting with '.' and then
// renamed, so other devices and the sync client never see it half
// written; List skips those names. A create never replaces a file.
//
// Paths stay inside the root: lexically, and not through a symbolic link
// or junction below it, which would point reads, writes and deletes
// elsewhere. List does not follow such links; the root itself may be one.
public sealed class LocalFolderSyncTransport : ISyncTransport
{
    private const string TempPrefix = ".tmp-";

    private readonly string _root;

    public LocalFolderSyncTransport(string root)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(root);
        _root = Path.GetFullPath(root);
    }

    public Task<IReadOnlyList<string>> ListAsync(string prefix, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prefix);
        if (!Directory.Exists(_root)) return Task.FromResult<IReadOnlyList<string>>([]);
        var entries = new FileSystemEnumerable<string>(_root,
            (ref FileSystemEntry entry) => entry.ToFullPath(),
            new EnumerationOptions { RecurseSubdirectories = true, AttributesToSkip = 0, IgnoreInaccessible = false })
        {
            ShouldIncludePredicate = (ref FileSystemEntry entry) => !entry.IsDirectory && !IsLink(ref entry),
            ShouldRecursePredicate = (ref FileSystemEntry entry) => !IsLink(ref entry),
        };
        IReadOnlyList<string> files = [.. entries
            .Where(f => !Path.GetFileName(f).StartsWith('.'))
            .Select(f => Path.GetRelativePath(_root, f).Replace('\\', '/'))
            .Where(p => p.StartsWith(prefix, StringComparison.Ordinal))
            .Order(StringComparer.Ordinal)];
        return Task.FromResult(files);
    }

    public async Task<byte[]?> ReadAsync(string path, CancellationToken cancellationToken)
    {
        var full = Resolve(path);
        try
        {
            return await File.ReadAllBytesAsync(full, cancellationToken);
        }
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            return null;
        }
    }

    public async Task<bool> CreateAsync(string path, byte[] content, CancellationToken cancellationToken)
    {
        var full = Resolve(path);
        if (File.Exists(full)) return false;
        var temp = await WriteTempAsync(full, content, cancellationToken);
        try
        {
            File.Move(temp, full, overwrite: false);
            return true;
        }
        catch (IOException) when (File.Exists(full))
        {
            return false;
        }
        finally
        {
            TryDelete(temp);
        }
    }

    public async Task WriteAsync(string path, byte[] content, CancellationToken cancellationToken)
    {
        var full = Resolve(path);
        var temp = await WriteTempAsync(full, content, cancellationToken);
        try
        {
            File.Move(temp, full, overwrite: true);
        }
        finally
        {
            TryDelete(temp);
        }
    }

    public Task DeleteAsync(string path, CancellationToken cancellationToken)
    {
        var full = Resolve(path);
        if (File.Exists(full)) File.Delete(full);
        return Task.CompletedTask;
    }

    private static async Task<string> WriteTempAsync(string full, byte[] content, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(content);
        Directory.CreateDirectory(Path.GetDirectoryName(full)!);
        var temp = Path.Combine(Path.GetDirectoryName(full)!, TempPrefix + Guid.NewGuid().ToString("N"));
        try
        {
            await File.WriteAllBytesAsync(temp, content, cancellationToken);
            return temp;
        }
        catch
        {
            TryDelete(temp);
            throw;
        }
    }

    // A failed write or rename must not leave the temporary file behind:
    // List skips it, but leftovers would pile up in the synced folder.
    // After a successful rename the file is gone and Delete does nothing.
    private static void TryDelete(string temp)
    {
        try
        {
            File.Delete(temp);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // Best effort (the sync client may hold the file): the original
            // failure is the one that surfaces, and List skips the leftover.
        }
    }

    // Relative paths only, never outside the root.
    private string Resolve(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        var full = Path.GetFullPath(Path.Combine(_root, path.Replace('/', Path.DirectorySeparatorChar)));
        if (!full.StartsWith(_root + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            throw new ArgumentException("The path leaves the sync folder.", nameof(path));
        EnsureNoLinkBelowRoot(full, path);
        return full;
    }

    // Every existing component between the root and the file, the file
    // included. The message names the relative path only: the full path
    // holds the user name.
    private void EnsureNoLinkBelowRoot(string full, string path)
    {
        for (var current = full; current.Length > _root.Length; current = Path.GetDirectoryName(current)!)
        {
            FileAttributes attributes;
            try
            {
                attributes = File.GetAttributes(current);
            }
            catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
            {
                continue;
            }
            if ((attributes & FileAttributes.ReparsePoint) == 0) continue;
            FileSystemInfo info = (attributes & FileAttributes.Directory) != 0
                ? new DirectoryInfo(current)
                : new FileInfo(current);
            if (info.LinkTarget is not null)
                throw new IOException($"The sync folder path '{path}' goes through a symbolic link or junction.");
        }
    }

    // Symbolic links and junctions only. The cloud placeholders of OneDrive
    // Files On-Demand are reparse points too, but have no link target.
    private static bool IsLink(ref FileSystemEntry entry)
        => (entry.Attributes & FileAttributes.ReparsePoint) != 0 && entry.ToFileSystemInfo().LinkTarget is not null;
}
