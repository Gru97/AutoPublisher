using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoPublisher.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AutoPublisher.Services
{
    public class TaskRunner : BackgroundService
    {
        private int executionCount = 0;
        private Timer timer;
        private RunnerConfig _runnerConfig;
        private ILogger<TaskRunner> _logger;
        private IServiceProvider _serviceProvider;


        public TaskRunner(RunnerConfig runnerConfig, 
            ILogger<TaskRunner> logger,
            IServiceProvider serviceProvider)
        {
            _runnerConfig = runnerConfig;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var serviceNames = Dequeue();
                var services = serviceNames.Split("\r\n");

                if (!string.IsNullOrEmpty(serviceNames) && serviceNames != "\r\n")
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var runner =
                        ActivatorUtilities.GetServiceOrCreateInstance(scope.ServiceProvider, typeof(IScriptRunner));
                    if (runner == null)
                        return;

                   
                        foreach (var service in services)
                        {
                            if(string.IsNullOrEmpty(service))
                                continue;

                            MethodInfo methodInfo = runner.GetType().GetMethod("PublishService");
                            methodInfo.Invoke(runner, new object[] { service });
                            //_scriptRunner.PublishService(service);
                            UpdateQueue(service);
                        }
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        private void UpdateQueue(string serviceName)
        {
            var serviceNames = File.ReadAllText(_runnerConfig.QueuePath);
            serviceNames=serviceNames.Replace(serviceName,string.Empty);
            using (var writer = File.CreateText(_runnerConfig.QueuePath))
            {
                writer.Write(serviceNames);
            }
        }

        private string Dequeue()
        {
            if (_runnerConfig.QueuePath == default)
                throw new Exception("No queue file path has not been configured.'");
            string serviceNames = String.Empty;
            try
            {
                serviceNames = File.ReadAllText(_runnerConfig.QueuePath);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception("Error occured while reading file. " + e.Message);
            }

            _logger.LogInformation(serviceNames);
            return serviceNames;

        }
    }


}
