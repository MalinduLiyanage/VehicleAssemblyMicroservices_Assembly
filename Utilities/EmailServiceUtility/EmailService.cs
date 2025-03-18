using AssemblyService.DTOs.Responses;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace AssemblyService.Utilities.EmailServiceUtility
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<BaseResponse> SendEmail(MimeMessage message)
        {
            BaseResponse response = new BaseResponse();

            var emailSettings = configuration.GetSection("EmailSettings");
            string senderEmail = emailSettings["SenderEmail"];
            string senderName = emailSettings["SenderName"];
            string senderPassword = emailSettings["SenderPassword"];
            string smtpServer = emailSettings["SmtpServer"];
            int smtpPort = int.Parse(emailSettings["SmtpPort"]);

            message.From.Add(new MailboxAddress(senderName, senderEmail));

            using (SmtpClient client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(senderEmail, senderPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                    response = new BaseResponse
                    {
                        status_code = StatusCodes.Status200OK,
                        data = new { message = "Email sent successfully!" }
                    };
                }
                catch (Exception ex)
                {
                    response = new BaseResponse
                    {
                        status_code = StatusCodes.Status500InternalServerError,
                        data = new { message = "Failed to send the Email.", error = ex.Message }
                    };
                }
            }

            return response;
        }

    }
}
