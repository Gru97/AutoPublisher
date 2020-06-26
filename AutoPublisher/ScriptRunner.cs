using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AutoPublisher
{
    public interface IScriptRunner
    {
        void PublishService();
    }
    public class ScriptRunner: IScriptRunner
    {
        private Configuration _configuration;
        private ILogger<ScriptRunner> _logger;
        public ScriptRunner(Configuration configuration, ILogger<ScriptRunner> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public void PublishService()
        {
            var scriptPath = _configuration.ScriptPath;
            _logger.LogInformation(scriptPath);
            string script = File.ReadAllText(scriptPath);
            _logger.LogInformation(script);
            using (var powerShell = PowerShell.Create())
            {
                _logger.LogInformation("------------beginning to run script-----------------");
                powerShell.AddScript(script);
                var results=powerShell.Invoke();
                foreach (var psObject in results)
                {
                    //System.Diagnostics.Debug.Write(psObject.BaseObject.ToString());
                    _logger.LogInformation(psObject.BaseObject.ToString());
                }

            }
            _logger.LogInformation("--------------end of command----------------");

        }
    }

    public class Configuration
    {
        public string ScriptPath{ get; set; }
    }
}
