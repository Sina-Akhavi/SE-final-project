namespace Paradaim.Core.Common.Models.Domains
{
    public class EmailDomain
    {
        public string To { get; set; }
        public string? CC { get; set; }
        public string? BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? FilePath { get; set; }
    }
}
