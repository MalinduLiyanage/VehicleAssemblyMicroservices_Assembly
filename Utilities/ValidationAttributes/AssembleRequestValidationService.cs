using AssemblyService.DTOs.Requests;

namespace AssemblyService.Attributes.ValidationAttributes
{
    public class AssembleRequestValidationService : IAssembleRequestValidationService
    {
        private readonly ApplicationDbContext context;

        public AssembleRequestValidationService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<string> Validate(PutAssembleRequest request)
        {
            List<string> errors = new List<string>();

            if (!context.vehicle.Any(v => v.vehicle_id == request.vehicle_id))
            {
                errors.Add("Invalid Vehicle ID. Vehicle does not exist.");
            }

            if (!context.worker.Any(w => w.NIC == request.nic))
            {
                errors.Add("Invalid NIC. Worker does not exist.");
            }

            if (!context.admins.Any(a => a.NIC == request.assignee_id))
            {
                errors.Add("Invalid Assignee ID. Admin does not exist.");
            }

            return errors;
        }
    }
}
