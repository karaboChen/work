using DemoApi.Dtos;
using DemoApi.Services;
using DemoApi.Tools;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpPost]
        public async Task <ApiResult<List<Items>>> CreateItem(Items name)
        {
            var item = await _weatherService.CreateItem(name);
            return item; 
        }
    }
}
