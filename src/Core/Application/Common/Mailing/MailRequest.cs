namespace Backend.Application.Common.Mailing;

public class MailRequest(
    List<string> to,
    string subject,
    string? body = null,
    string? from = null,
    string? displayName = null,
    string? replyTo = null,
    string? replyToName = null,
    List<string>? bcc = null,
    List<string>? cc = null,
    IDictionary<string, byte[]>? attachmentData = null,
    IDictionary<string, string>? headers = null)
{
    public List<string> To { get; } = to;

    public string Subject { get; } = subject;

    public string? Body { get; } = body;

    public string? From { get; } = from;

    public string? DisplayName { get; } = displayName;

    public string? ReplyTo { get; } = replyTo;

    public string? ReplyToName { get; } = replyToName;

    public List<string> Bcc { get; } = bcc ?? new List<string>();

    public List<string> Cc { get; } = cc ?? new List<string>();

    public IDictionary<string, byte[]> AttachmentData { get; } = attachmentData ?? new Dictionary<string, byte[]>();

    public IDictionary<string, string> Headers { get; } = headers ?? new Dictionary<string, string>();
}
