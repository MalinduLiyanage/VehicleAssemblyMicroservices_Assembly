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

            try
            {
                var vehicleData = await communicationClientUtility.GetVehicleData(request.vehicle_id);
            }
            catch (HttpRequestException ex)
            {
                errors.Add("Invalid Vehicle ID. Vehicle does not exist.");
            }

            try
            {
                var workerData = await communicationClientUtility.GetWorkerData(request.nic);
            }
            catch (HttpRequestException ex)
            {
                errors.Add("Invalid NIC. Worker does not exist.");
            }

            try
            {
                var assigneeData = await communicationClientUtility.GetAssigneeData(request.assignee_id);
            }
            catch (HttpRequestException ex)
            {
                errors.Add("Invalid Assignee ID. Admin does not exist.");
            }

            return errors;
        }


    }
}
