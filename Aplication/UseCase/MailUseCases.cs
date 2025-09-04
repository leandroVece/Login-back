

using Aplication.Common;
using Application.ingoing;
using Domain.Entities;
using MailKit.Net.Smtp;
using MimeKit;

namespace Application.UseCases;

public class EmailUseCases : IEmailUseCases
{

    private readonly EmailConfigurations _emailConfig;

    public EmailUseCases(EmailConfigurations emailConfig)
    {
        _emailConfig = emailConfig;
    }

    public async void SendMail(Message message)
    {
        var EmailMessage = CreateEmailMessage(message);
        Send(EmailMessage);
    }


    private MimeMessage CreateEmailMessage(Message message)
    {
        var EmailMessage = new MimeMessage();
        EmailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
        EmailMessage.To.AddRange(message.To);
        EmailMessage.Subject = message.Subject;
        // Cambiar la siguiente línea para que el formato sea HTML
        EmailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

        return EmailMessage;
    }

    void Send(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_emailConfig.Username, _emailConfig.Password);

            client.Send(mailMessage);
        }
        catch (System.Exception)
        {

            throw;
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }
}