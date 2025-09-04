using MimeKit;

namespace Aplication.Common;

public class Message
{
    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public bool IsHtml { get; set; }


    public Message(IEnumerable<string> to, string subject, string content)
    {
        this.To = new List<MailboxAddress>();
        To.AddRange(to.Select(x => new MailboxAddress("email", x)));
        this.Subject = subject;
        this.Content = content;
    }

}