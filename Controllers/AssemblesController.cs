using AssemblyService.DTOs;
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

        [HttpPost("get-all/worker/{id}")]
        public IActionResult GetWorkerassemblesById(int id)
        {
            List<AssemblesForWorkerDTO> result = assembleService.GetWorkerAssemblesById(id);

            if (result == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<BaseResponse> CreateAssemble([FromForm] PutAssembleRequest request)
        {
            return await assembleService.CreateAssemble(request);
        }

    }
}
