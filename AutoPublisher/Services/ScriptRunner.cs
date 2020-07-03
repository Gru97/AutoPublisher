using System;
using System.IO;
using System.Management.Automation;
using AutoPublisher.Models;
using Microsoft.Extensions.Logging;

namespace AutoPublisher.Services
{
    public interface IScriptRunner
    {
        void PublishService(string serviceName);
    }
    public class ScriptRunner : IScriptRunner
    {
        private RunnerConfig _runnerConfig;
        private static ILogger<ScriptRunner> _logger;
        public ScriptRunner(RunnerConfig runnerConfig, ILogger<ScriptRunner> logger)
        {
            _runnerConfig = runnerConfig;
            _logger = logger;
        }
        public void PublishService(string serviceName)
        {
            var script = ReadScriptFile(GetScripPathFor(serviceName));

            using (var powerShell = PowerShell.Create())
            {
                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                
                powerShell.Streams.Progress.DataAdded += myProgressEventHandler;
                outputCollection.DataAdded += dataAddedHandler;
                powerShell.Streams.Error.DataAdded += errorAddedHandler;
            
                _logger.LogInformation("------------beginning to run script-----------------");

               
                powerShell.AddScript(script);
            
                var results = powerShell.Invoke();

                foreach (var psObject in results)
                {
                    _logger.LogInformation(psObject.BaseObject.ToString());
                }
                foreach (var psObject in powerShell.Streams.Error)
                {
                    _logger.LogInformation(psObject.ToString());
                }
                _logger.LogInformation("--------------end of script----------------");
            }

        }

        private void errorAddedHandler(object sender, DataAddedEventArgs e)
        {
            _logger.LogInformation("error");
        }

        private void dataAddedHandler(object sender, DataAddedEventArgs e)
        {
            _logger.LogInformation("data");
        }

        private string GetScripPathFor(string serviceName)
        {
            serviceName = serviceName.ToLower().Trim();

            try
            {
               return _runnerConfig.ScriptPath[serviceName];
            }
            catch (Exception e)
            { 
                _logger.LogError($"Unable to get path of service {serviceName}");
                throw new Exception("Unable to get path of service. The service name is not valid." );
            }
        }

        private string ReadScriptFile(string scriptPath)
        {
            if (scriptPath == default)
                throw new Exception("No script file path has not been configured.'");
            _logger.LogInformation(scriptPath);

            string script = string.Empty;
            try
            {
                script = File.ReadAllText(scriptPath);

            }
            catch (Exception e)
            {
                _logger.LogInformation(script);
                throw new Exception("Error occured while reading file. "+e.Message);
            }
            
            _logger.LogInformation(script);
            return script;

        }

        static void myProgressEventHandler(object sender, DataAddedEventArgs e)
        {
            ProgressRecord newRecord = ((PSDataCollection<ProgressRecord>)sender)[e.Index];
            if (newRecord.PercentComplete != -1)
            {
                _logger.LogInformation("Progress updated: {0}", newRecord.PercentComplete);
            }
        }

    }

    
}
