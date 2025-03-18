using AssemblyService.DTOs.Requests;
using AssemblyService.Utilities.CommunicationClientUtility;
using System.Threading.Tasks;

namespace AssemblyService.Attributes.ValidationAttributes
{
    public class AssembleRequestValidationService : IAssembleRequestValidationService
    {
        private readonly ApplicationDbContext context;
        private readonly CommunicationClientUtility communicationClientUtility;

        public AssembleRequestValidationService(ApplicationDbContext context, CommunicationClientUtility communicationClientUtility)
        {
            this.context = context;
            this.communicationClientUtility = communicationClientUtility;
        }

        public async Task<List<string>> Validate(PutAssembleRequest request)
        {
            List<string> errors = new List<string>();

            // Validate Vehicle
            var vehicleData = await communicationClientUtility.GetVehicleData(request.vehicle_id);
            if (vehicleData == null || vehicleData.vehicle_id == 0) 
            {
                errors.Add("Invalid Vehicle ID. Vehicle does not exist.");
            }

            // Validate Worker
            var workerData = await communicationClientUtility.GetWorkerData(request.nic);
            if (workerData == null || string.IsNullOrEmpty(workerData.firstname) || string.IsNullOrEmpty(workerData.lastname))
            {
                errors.Add("Invalid NIC. Worker does not exist.");
            }

            // Validate Assignee
            var assigneeData = await communicationClientUtility.GetAssigneeData(request.assignee_id);
            if (assigneeData == null || assigneeData.nic == 0)
            {
                errors.Add("Invalid Assignee ID. Admin does not exist.");
            }

            return errors;
        }

    }
}
