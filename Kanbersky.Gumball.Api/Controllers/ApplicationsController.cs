using System.Threading.Tasks;
using Kanbersky.Gumball.Business.Abstract;
using Kanbersky.Gumball.Business.DTO.Request;
using Kanbersky.Gumball.Business.DTO.Response;
using Microsoft.AspNetCore.Mvc;

namespace Kanbersky.Gumball.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var applications = await _applicationService.GetAllApplications();
            return Ok(applications);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateApplicationRequestModel createApplicationRequestModel)
        {
            var application = await _applicationService.AddAsync(createApplicationRequestModel);
            return Ok(application);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            await _applicationService.Remove(id);
            return NoContent();
        }
    }
}