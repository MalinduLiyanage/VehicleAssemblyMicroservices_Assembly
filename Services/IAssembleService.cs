using AssemblyService.DTOs;
using AssemblyService.DTOs.Requests;
using AssemblyService.DTOs.Responses;

namespace AssemblyService.Services
{
    public interface IAssembleService
    {
        public List<AssemblesForWorkerDTO> GetWorkerAssemblesById(int id);
        public Task<BaseResponse> CreateAssemble(PutAssembleRequest request);
    }
}
