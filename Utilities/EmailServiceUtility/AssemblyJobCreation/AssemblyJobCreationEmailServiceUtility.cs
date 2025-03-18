using AdminService.DTOs.Requests.EmailRequests;
using MimeKit;

namespace AdminService.Utilities.EmailServiceUtility.AdminAccountCreation
{
    public class AssemblyJobCreationEmailServiceUtility : MimeMessage
    {
        public AssemblyJobCreationEmailServiceUtility(AssemblyJobCreationEmailRequestDTO request, Stream? attachmentStream = null, string? attachmentFileName = null)
        {
            BodyBuilder bodyBuilder = new BodyBuilder();

            string emailTemplate = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "SendEmailView", "AssemblyJobEmail.html");
            string emailBody = File.ReadAllText(emailTemplate);

            emailBody = emailBody.Replace("{{WorkerName}}", request.WorkerName)
                .Replace("{{model}}", request.model)
                .Replace("{{vehicle_id}}", request.vehicle_id.ToString())
                .Replace("{{date}}", request.date.ToString())
                .Replace("{{assignee_first_name}}", request.assignee_first_name)
                .Replace("{{assignee_last_name}}", request.assignee_last_name)
                .Replace("{{assignee_id}}", request.assignee_id.ToString());

            bodyBuilder.HtmlBody = emailBody;
            this.To.Add(new MailboxAddress(request.WorkerName, request.email));
            this.Subject = "Assemble Job Assignment - " + request.date;
            var multipart = new Multipart("mixed");

            var textPart = new TextPart(MimeKit.Text.TextFormat.Html) { Text = bodyBuilder.HtmlBody };
            multipart.Add(textPart);

            if (attachmentStream != null && !string.IsNullOrEmpty(attachmentFileName))
            {
                var attachment = new MimePart()
                {
                    Content = new MimeContent(attachmentStream),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = attachmentFileName
                };
                multipart.Add(attachment);
            }

            this.Body = multipart;
        }

    }
}
