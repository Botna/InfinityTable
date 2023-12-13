using infinityTableWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace infinityTableWebsite.Controllers
{
    [ApiController]
    [Route("table")]
    public class TableController : ControllerBase
    {
        private readonly ILogger<TableController> _logger;
        private readonly IPiLedService _piLedService;

        public TableController(ILogger<TableController> logger, IPiLedService piLedService)
        {
            _logger = logger;
            _piLedService = piLedService;
        }

        [HttpGet]
        [Route("solidColor")]
        public IActionResult Get(string color)
        {
            _logger.LogInformation($"Just got a table color request for {color}");
            _piLedService.SetColor(color);
            return Ok();
        }

        [HttpPost]
        [Route("solidColor")]
        public IActionResult UpdateColorByHSV(int hue, int saturation, int value)
        {
            _logger.LogInformation($"Just got a table color request for HSV of {hue}:{saturation}:{value}");
            _piLedService.SetColor(hue, saturation, value);
            return Ok();
        }
    }
}
