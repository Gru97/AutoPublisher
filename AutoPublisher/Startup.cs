using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoPublisher.Models;
using AutoPublisher.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace AutoPublisher
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<RunnerConfig>(sp =>
            {
                var runnerConfig = new RunnerConfig();
                var list = Configuration
                    .GetSection("RunnerConfig")
                    .GetSection("ScriptPath")
                    .GetChildren().ToList();
                       
                list.ForEach(e =>
                {
                    runnerConfig.ScriptPath.Add(e.Key.ToLower(),e.Value);

                });
                return runnerConfig;

            });
            services.AddScoped<IScriptRunner,ScriptRunner>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoPublisher API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("myPolicy",
                    builder =>
                    {
                        builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("logs/log.txt", LogLevel.Debug, new Dictionary<string, LogLevel>()
            {
                {"Microsoft", LogLevel.Information},
                {"System", LogLevel.Error}
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoPublisher API"); });
            app.UseStaticFiles();  //no need for wwwroot- enables us to access scrip files with relative path
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
