using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace TF47_Backend.Services.Mail
{
    public class MailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _senderMail;
        private readonly string _senderName;

        

        public MailService(ILogger<MailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _smtpServer = configuration["Credentials:Mail:Server"];
            _port = Convert.ToInt32(configuration["Credentials:Mail:Port"]);
            _username = configuration["Credentials:Mail:Username"];
            _password = configuration["Credentials:Mail:Password"];
            _senderMail = configuration["Credentials:Mail:Mail"];
            _senderName = configuration["Credentials:Mail:SenderName"];
        }

        public async Task SendMailAsync(string receiverMail, string username, string subject, string body,
            CancellationToken cancellationToken)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_senderName, _senderMail));
            message.To.Add(new MailboxAddress(username, receiverMail));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_smtpServer, _port, SecureSocketOptions.Auto, cancellationToken);
                await client.AuthenticateAsync(_username, _password, cancellationToken);
                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to send mail to {receiverMail}, subject {subject}");
                _logger.LogError($"Failed to send mail: {ex.Message}");
            }
            finally
            {
                client.Dispose();
            }
        }


    }
}
