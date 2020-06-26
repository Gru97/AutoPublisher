using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoPublisher.Controllers
{
    [ApiController]
    [Route("inform")]
    public class InformController : ControllerBase
    {
        private readonly IScriptRunner _scriptRunner;

        public InformController(IScriptRunner scriptRunner)
        {
            _scriptRunner = scriptRunner;
        }

        [HttpGet]
        public async Task<IActionResult> Inform()
        {
            Console.WriteLine("Inside Inform action");
            _scriptRunner.PublishService();
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
