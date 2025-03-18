using AssemblyService.DTOs.Requests;

namespace AssemblyService.Attributes.ValidationAttributes
{
    public interface IAssembleRequestValidationService
    {
        Task<List<string>> Validate(PutAssembleRequest request);
    }

}
