using AssemblyService.DTOs.Requests;
using AssemblyService.DTOs.Responses;

namespace AssemblyService.Services
{
    public interface IAssembleService
    {
        BaseResponse GetAssembles(int? vehicle_id, int? worker_id, int? assignee_id);
        Task<BaseResponse> CreateAssemble(PutAssembleRequest request);
    }
}
