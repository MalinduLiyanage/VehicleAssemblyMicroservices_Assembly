using AssemblyService.DTOs.Requests;
using AssemblyService.DTOs;
using AssemblyService.Models;
using Microsoft.EntityFrameworkCore;
using AssemblyService.Utilities.EmailServiceUtility;
using AssemblyService.DTOs.Responses;
using AssemblyService.Utilities.EmailAttachmentUtility;
using AdminService.DTOs.Requests.EmailRequests;
using AdminService.Utilities.EmailServiceUtility.AdminAccountCreation;
using AssemblyService.Utilities.CommunicationClientUtility;

namespace AssemblyService.Services
{
    public class AssembleService : IAssembleService
    {
        private readonly ApplicationDbContext context;
        private readonly IEmailService emailService;
        private readonly CommunicationClientUtility communicationClientUtility;

        public AssembleService(ApplicationDbContext context, IEmailService emailService, CommunicationClientUtility communicationClientUtility)
        {
            this.context = context;
            this.emailService = emailService;
            this.communicationClientUtility = communicationClientUtility;
        }

        public BaseResponse GetAssembles(int? vehicle_id, int? worker_id, int? assignee_id)
        {
            BaseResponse response;
            try
            {
                
                List<AssembleDTO> assembles = new List<AssembleDTO>();

                using (context)
                {/*
                    var query = context.assembles
                        .Include(a => a.Vehicle)
                        .Include(a => a.Worker)
                        .Include(a => a.Admin)
                        .AsQueryable();

                    if (vehicle_id.HasValue)
                        query = query.Where(a => a.vehicle_id == vehicle_id.Value);

                    if (worker_id.HasValue)
                        query = query.Where(a => a.NIC == worker_id.Value);

                    if (assignee_id.HasValue)
                        query = query.Where(a => a.assignee_id == assignee_id.Value);

                    query.ToList().ForEach(a => assembles.Add(new AssembleDTO
                    {
                        assignee_id = a.assignee_id,
                        assignee_first_name = a.Admin.firstname,
                        assignee_last_name = a.Admin.lastname,
                        vehicle_id = a.vehicle_id,
                        model = a.Vehicle.model,
                        color = a.Vehicle.color,
                        engine = a.Vehicle.engine,
                        NIC = a.NIC,
                        WorkerName = $"{a.Worker.firstname} {a.Worker.lastname}",
                        job_role = a.Worker.job_role,
                        date = a.date,
                        isCompleted = a.isCompleted,
                        attachment = a.attachment_path
                    }));*/
                }

                response = new BaseResponse
                {
                    status_code = StatusCodes.Status200OK,
                    data = new { assembles }
                };
                return response;

            }
            catch (Exception e)
            {
                response = new BaseResponse
                {
                    status_code = StatusCodes.Status500InternalServerError,
                    data = new { message = "An error occurred while fetching assemble records." + e.Message }
                };
            }
            return response;

        }

        public async Task<BaseResponse> CreateAssemble(PutAssembleRequest request)
        {
            BaseResponse response;
            try
            {
                EmailAttachmentServiceUtility attachFile = new EmailAttachmentServiceUtility();
                string filepath = attachFile.PostFileAsync(request).Result;

                AssembleModel newAssemble = new AssembleModel
                {
                    assignee_id = request.assignee_id,
                    vehicle_id = request.vehicle_id,
                    NIC = request.nic,
                    date = request.date,
                    isCompleted = request.isCompleted,
                    attachment_path = filepath
                };

                context.assembles.Add(newAssemble);
                context.SaveChanges();

                AdminDTO admin = await communicationClientUtility.GetAssigneeData(request.assignee_id);
                VehicleDTO vehicle = await communicationClientUtility.GetVehicleData(request.vehicle_id);
                WorkerDTO worker = await communicationClientUtility.GetWorkerData(request.nic);

                AssemblyJobCreationEmailRequestDTO emailInfo = new AssemblyJobCreationEmailRequestDTO
                {
                    assignee_id = request.assignee_id,
                    assignee_first_name = admin.firstname,
                    assignee_last_name = admin.lastname,
                    vehicle_id = vehicle.vehicle_id,
                    model = vehicle.model,
                    color = vehicle.color,
                    engine = vehicle.engine,
                    NIC = worker.NIC,
                    WorkerName = $"{worker.firstname} {worker.lastname}",
                    email = worker.email,
                    job_role = worker.job_role,
                    date = request.date
                };

                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                string fileName = Path.GetFileName(filepath);
                AssemblyJobCreationEmailServiceUtility message = new AssemblyJobCreationEmailServiceUtility(emailInfo, fs, fileName);

                response = new BaseResponse
                {
                    status_code = StatusCodes.Status200OK,
                    data = new
                    {
                        message = "Assemble record created successfully!",
                        email_status = emailService.SendEmail(message)
                    }
                };
            }
            catch (Exception ex)
            {
                response = new BaseResponse
                {
                    status_code = StatusCodes.Status500InternalServerError,
                    data = new { message = "An error occurred while creating the assemble record.", error = ex.Message }
                };
            }
            return response;

        }

    }
}
