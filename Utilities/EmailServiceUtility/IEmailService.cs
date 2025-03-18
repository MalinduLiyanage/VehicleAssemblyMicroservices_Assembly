using AssemblyService.DTOs.Responses;
using MimeKit;

namespace AssemblyService.Utilities.EmailServiceUtility
{
    public interface IEmailService
    {
        public Task<BaseResponse> SendEmail(MimeMessage msg);
    }
}
