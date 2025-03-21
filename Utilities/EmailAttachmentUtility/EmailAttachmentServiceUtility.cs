using AssemblyService.DTOs.Requests;

namespace AssemblyService.Utilities.EmailAttachmentUtility
{
    public class EmailAttachmentServiceUtility : IEmailAttachmentServiceUtility
    {
        public async Task<string?> PostFileAsync(PutAssembleRequest request)
        {
            string? saved_filepath = null;

            try
            {
                if (request.assembly_attachment == null || request.assembly_attachment.Length == 0)
                {
                    return null;
                }

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileExtension = Path.GetExtension(request.assembly_attachment.FileName);
                string filename = request.assignee_id + "_" + request.vehicle_id + "_" + request.nic + fileExtension;

                string filePath = Path.Combine(uploadsFolder, filename);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.assembly_attachment.CopyToAsync(stream);
                }

                saved_filepath = filePath;
            }
            catch (Exception ex)
            {
                saved_filepath = "Error saving file: " + ex.Message;
            }

            return saved_filepath;
        }

    }
}
