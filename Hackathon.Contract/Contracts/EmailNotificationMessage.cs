namespace Hackathon.Contract.Contracts
{
    public class EmailNotificationMessage
    {
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
