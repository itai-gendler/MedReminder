using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using MedReminder.Application.Notifications;

namespace MedReminder.Infrastructure.Email;

// The email delivery journal as a JSON file (Android backlog B5-04):
// email-deliveries.json in the host's data directory, one file for the
// installation on this device, so the emails of every profile the device
// sends for are listed together. It keeps the newest MaxEntries entries
// not older than MaxAge. No subject, body or address is stored
// (EmailDeliveryEntry).
//
// Several containers of one process write the same file (the open
// profile's and those of the master's profile checks): a lock per path
// serializes them, and each write goes through a temporary file. An
// unreadable file reads as empty and is replaced on the next write; an
// I/O error is swallowed, since a send must never fail because of the
// journal.
public sealed class JsonFileEmailDeliveryJournal : IEmailDeliveryJournal
{
    public const string FileName = "email-deliveries.json";
    public const int MaxEntries = 50;
    public static readonly TimeSpan MaxAge = TimeSpan.FromDays(30);

    private static readonly ConcurrentDictionary<string, object> Locks = new(StringComparer.Ordinal);

    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly string _path;
    private readonly TimeProvider _clock;
    private readonly object _lock;

    public JsonFileEmailDeliveryJournal(string dataDirectory, TimeProvider? clock = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dataDirectory);
        _path = Path.GetFullPath(Path.Combine(dataDirectory, FileName));
        _clock = clock ?? TimeProvider.System;
        _lock = Locks.GetOrAdd(_path, _ => new object());
    }

    public void Record(EmailDeliveryEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        lock (_lock)
        {
            try
            {
                var entries = Load().Where(e => e.Id != entry.Id).Append(entry);
                var directory = Path.GetDirectoryName(_path)!;
                Directory.CreateDirectory(directory);
                var tmp = _path + ".tmp";
                File.WriteAllText(tmp, JsonSerializer.Serialize(Prune(entries), Options));
                File.Move(tmp, _path, overwrite: true);
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }
    }

    public IReadOnlyList<EmailDeliveryEntry> Read()
    {
        lock (_lock)
        {
            return Prune(Load());
        }
    }

    private List<EmailDeliveryEntry> Load()
    {
        try
        {
            if (!File.Exists(_path)) return [];
            return JsonSerializer.Deserialize<List<EmailDeliveryEntry>>(File.ReadAllText(_path), Options)
                ?.Where(e => e is not null && e.Id != Guid.Empty).ToList() ?? [];
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException
                                       or NotSupportedException)
        {
            return [];
        }
    }

    private List<EmailDeliveryEntry> Prune(IEnumerable<EmailDeliveryEntry> entries)
    {
        var oldest = _clock.GetUtcNow() - MaxAge;
        return [.. entries
            .Where(e => e.At >= oldest)
            .OrderByDescending(e => e.StartedAt)
            .ThenByDescending(e => e.At)
            .Take(MaxEntries)];
    }
}
