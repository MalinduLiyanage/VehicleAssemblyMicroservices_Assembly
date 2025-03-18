using AssemblyService.DTOs.Requests;

namespace AssemblyService.Attributes.ValidationAttributes
{
    public interface IAssembleRequestValidationService
    {
        List<string> Validate(PutAssembleRequest request);
    }
}
