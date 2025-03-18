using AssemblyService.DTOs.Requests;
using AssemblyService.DTOs.Responses;
using AssemblyService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssemblyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssemblesController : ControllerBase
    {
        private readonly IAssembleService assembleService;

        public AssemblesController(IAssembleService assembleService)
        {
            this.assembleService = assembleService;
        }

        [HttpPost("get-all")]
        public BaseResponse GetAssembles([FromQuery] int? vehicle_id, [FromQuery] int? worker_id, [FromQuery] int? assignee_id)
        {
            return assembleService.GetAssembles(vehicle_id, worker_id, assignee_id);
        }

        [HttpPost("create")]
        public async Task<BaseResponse> CreateAssemble([FromForm] PutAssembleRequest request)
        {
            return await assembleService.CreateAssemble(request);
        }

    }
}
