using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoPublisher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoPublisher.Controllers
{
    [ApiController]
    [Route("runner")]
    public class InformController : ControllerBase
    {
        private readonly IScriptRunner _scriptRunner;
        private readonly ILogger<InformController> _logger;
        public InformController(IScriptRunner scriptRunner, ILogger<InformController> logger)
        {
            _scriptRunner = scriptRunner;
            _logger = logger;
        }

        [HttpGet("inform")]
        public async Task<IActionResult> InformTest(string serviceName)
        {
            _logger.LogInformation("----------inside inform action - get---------------");
            _scriptRunner.PublishService(serviceName);
            _logger.LogInformation("----------returning from inform action - get---------------");
            return Ok();
        }
        [HttpPost("inform/content")]
        public async Task<IActionResult> Inform([FromBody] Event @event)
        {
            _logger.LogInformation("----------inside inform action - post---------------");
            _scriptRunner.PublishService("content");
            _logger.LogInformation("----------returning from inform action - post---------------");

            return Ok();
        }
        [HttpGet("healthCheck")]
        public async Task<IActionResult> HealthCheck()
        {
            Console.WriteLine("Inside HealthCheck action");
            return Ok("I'm alive!'");
        }
    }
}
