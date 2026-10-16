using MedReminder.Application.Abstractions;

namespace MedReminder.Infrastructure.Credentials;

// File-backed store for the SMTP password: the file holds the host
// protector's ciphertext (DPAPI CurrentUser on Windows, the Android
// Keystore on the phone), never the password itself. It lives in the
// installation's data directory (smtp.protected under
// %LOCALAPPDATA%\MedReminder\ on Windows), never in the repository and
// never in appsettings.json (spec §22). Portable since Android backlog
// B5-01: the host passes the path.
public sealed class SmtpCredentialStore : ISmtpCredentialStore
{
    public const string FileName = "smtp.protected";

    private readonly ICredentialProtector _protector;
    private readonly string _filePath;

    public SmtpCredentialStore(ICredentialProtector protector, string filePath)
    {
        ArgumentNullException.ThrowIfNull(protector);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        _protector = protector;
        _filePath = filePath;
    }

    // The store in a data directory, under its usual file name.
    public static SmtpCredentialStore In(ICredentialProtector protector, string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        return new SmtpCredentialStore(protector, Path.Combine(directory, FileName));
    }

    public bool HasPassword => File.Exists(_filePath) && new FileInfo(_filePath).Length > 0;

    public string? GetPassword()
    {
        if (!HasPassword) return null;
        var ciphertext = File.ReadAllText(_filePath).Trim();
        if (string.IsNullOrEmpty(ciphertext)) return null;
        return _protector.Unprotect(ciphertext);
    }

    public void SetPassword(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);
        var ciphertext = _protector.Protect(password);
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        // Written next to the target and moved over it, so an interrupted
        // write never leaves a truncated secret behind.
        var temporary = _filePath + ".tmp";
        File.WriteAllText(temporary, ciphertext);
        File.Move(temporary, _filePath, overwrite: true);
    }

    public void Clear()
    {
        if (File.Exists(_filePath)) File.Delete(_filePath);
    }
}
