using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;

namespace AutoPublisher
{
    public interface IScriptRunner
    {
        void PublishService();
    }
    public class ScriptRunner: IScriptRunner
    {
        private Configuration _configuration;
        public ScriptRunner(Configuration configuration)
        {
            _configuration = configuration;
        }
        public void PublishService()
        {
            var scriptPath = _configuration.ScriptPath;
            string script = File.ReadAllText(scriptPath);
            using (var powerShell = PowerShell.Create())
            {
                powerShell.AddScript(script);
                powerShell.Invoke();
            }

        }
    }

    public class Configuration
    {
        public string ScriptPath{ get; set; }
    }
}
