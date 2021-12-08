﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace infinityTableWebsite.Controllers
{
    [ApiController]
    [Route("health")]
    public class MonitorController : ControllerBase
    {
        private readonly ILogger<MonitorController> _logger;

        public MonitorController(ILogger<MonitorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("ping")]
        public IActionResult Get()
        {
            return Ok("You have hit me");
        }
    }
}
