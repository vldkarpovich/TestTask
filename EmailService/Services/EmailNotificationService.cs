using EmailService.Interfaces;
using EmailService.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailService.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly ISmtpClient _smtpClient;
        private readonly EmailSettings _emailSettings;

        public EmailNotificationService( ISmtpClient smtpClient, IOptions<EmailSettings> emailSettings) 
        {
            _emailSettings = emailSettings.Value;
            _smtpClient = smtpClient;
        }
        public async Task SendAsync(Receipt receipt)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Order Service", _emailSettings.SmtpUser));
            message.To.Add(new MailboxAddress("Customer", receipt.Email));
            message.Subject = "Thank you for your order";
            message.Body = new TextPart("plain")
            {
                Text = $"Thank you for your order. Please find your receipt attached.\n\nOrder ID: {receipt.OrderId}"
            };

            var attachment = new MimePart("text/plain")
            {
                Content = new MimeContent(File.OpenRead(receipt.ReceiptPath)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(receipt.ReceiptPath)
            };

            var multipart = new Multipart("mixed");
            multipart.Add(message.Body);
            multipart.Add(attachment);

            message.Body = multipart;

            await _smtpClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, false);
            await _smtpClient.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
            await _smtpClient.SendAsync(message);
            await _smtpClient.DisconnectAsync(true);
        }
    }
}
