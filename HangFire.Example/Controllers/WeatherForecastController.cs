using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DotNetHelper_CommandLine;

namespace HangFire.Example.Controllers
{

    public class CommandResult
    {
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Output { get; set; }= new List<string>();
        public int? ExitCode { get; set; } = 0;
    }
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private CommandResult Response { get; } = new CommandResult();
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Process exit code from command</returns>
        [HttpGet]
        public CommandResult RunCommand([FromQuery] string command = "ping google.com")
        {
            var cmd = new CommandPrompt();
            Response.ExitCode = cmd.RunCommand(command,Exited,OutputDataReceived, ErrorDataReceived);
          
            return Response;
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Response.Errors.Add(e.Data);
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Response.Output.Add(e.Data);
        }

        private void Exited(object? sender, EventArgs e)
        {
            
        }
    }
}
