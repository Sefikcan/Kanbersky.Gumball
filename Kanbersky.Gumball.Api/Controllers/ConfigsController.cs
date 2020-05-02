using System.Threading.Tasks;
using Kanbersky.Gumball.Business.Abstract;
using Kanbersky.Gumball.Business.DTO.Request;
using Microsoft.AspNetCore.Mvc;

namespace Kanbersky.Gumball.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigsController : ControllerBase
    {
        private readonly IConfigService _configService;

        public ConfigsController(IConfigService configService)
        {
            _configService = configService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var configs = _configService.GetAllConfigsByApplicationId(id);
            return Ok(configs);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateConfigRequestModel createConfigRequestModel)
        {
            var configResponse = await _configService.AddAsync(createConfigRequestModel);
            return Ok(configResponse);
        }

        [HttpPut("{configId}")]
        public async Task<IActionResult> Update([FromRoute]int configId, [FromBody]UpdateConfigRequestModel configUpdateRequestModel)
        {
            var config = await _configService.UpdateAsync(configId, configUpdateRequestModel);
            return Ok(config);
        }
    }
}