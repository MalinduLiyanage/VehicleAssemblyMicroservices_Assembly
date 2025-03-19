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
using Microsoft.AspNetCore.Mvc;

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

        public List<AssemblesForWorkerDTO> GetWorkerAssemblesById(int id)
        {
            try
            {
                var assemblies = context.assembles
                    .Where(a => a.NIC == id)
                    .Select(a => new AssemblesForWorkerDTO
                    {
                        assignee_id = a.assignee_id,
                        vehicle_id = a.vehicle_id,
                        date = a.date,
                        isCompleted = a.isCompleted,
                        attachment_path = a.attachment_path
                    })
                    .ToList();

                return assemblies.Count > 0 ? assemblies : null; 
            }
            catch (Exception)
            {
                return null; 
            }
        }

        public async Task<BaseResponse> CreateAssemble(PutAssembleRequest request)
        {
            BaseResponse response;
            try
            {
                AdminDTO admin = await communicationClientUtility.GetAssigneeData(request.assignee_id);
                VehicleDTO vehicle = await communicationClientUtility.GetVehicleData(request.vehicle_id);
                WorkerDTO worker = await communicationClientUtility.GetWorkerData(request.nic);

                if (admin != null && vehicle != null && worker != null)
                {
                    response = new BaseResponse
                    {
                        status_code = StatusCodes.Status409Conflict,
                        data = new { message = "The record already exists!" }
                    };
                }
                else 
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
