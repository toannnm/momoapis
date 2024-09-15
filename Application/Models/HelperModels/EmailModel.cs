namespace Application.Models.HelperModels
{
    public class EmailModel
    {
        public List<string> To { get; } = null!;
        public List<string> Bcc { get; } = null!;
        public List<string> Cc { get; } = null!;

        public string? From { get; }

        public string? DisplayName { get; }

        public string? ReplyTo { get; }

        public string? ReplyToName { get; }

        public string Subject { get; } = null!;

        public string? Body { get; }
    }
}
