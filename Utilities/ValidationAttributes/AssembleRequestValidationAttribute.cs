using System.ComponentModel.DataAnnotations;
using AssemblyService.DTOs.Requests;

namespace AssemblyService.Attributes.ValidationAttributes
{
    public class AssembleRequestValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            PutAssembleRequest request = (PutAssembleRequest)validationContext.ObjectInstance;

            if (request.vehicle_id <= 0 || request.nic <= 0 || request.date == default)
            {
                return new ValidationResult("All fields are required.");
            }

            IAssembleRequestValidationService? validationService = validationContext.GetService(typeof(IAssembleRequestValidationService)) as IAssembleRequestValidationService;

            if (validationService == null)
            {
                return new ValidationResult("Validation service is unavailable.");
            }

            List<string> validationErrors = validationService.Validate(request).Result;

            if (validationErrors.Any())
            {
                return new ValidationResult(string.Join("; ", validationErrors));
            }

            return ValidationResult.Success;
        }
    }

}
