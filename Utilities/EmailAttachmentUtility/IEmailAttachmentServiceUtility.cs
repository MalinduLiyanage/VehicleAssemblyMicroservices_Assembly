using AssemblyService.DTOs.Requests;

namespace AssemblyService.Utilities.EmailAttachmentUtility
{
    public interface IEmailAttachmentServiceUtility
    {
        public Task<string?> PostFileAsync(PutAssembleRequest request);
    }
}
