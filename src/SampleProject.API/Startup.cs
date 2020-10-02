using Microsoft.Extensions.Configuration.UserSecrets;

[assembly: UserSecretsId("54e8eb06-aaa1-4fff-9f05-3ced1cb623c2")]

namespace SampleProject.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Configuration;
    using Application.Configuration.Emails;
    using Application.Configuration.Validation;
    using Configuration;
    using Domain.SeedWork;
    using Hellang.Middleware.ProblemDetails;
    using Infrastructure;
    using Infrastructure.Caching;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using SeedWork;
    using Serilog;
    using Serilog.Formatting.Compact;

    public class Startup
    {
        private const string OrdersConnectionString = "OrdersConnectionString";

        private static ILogger _logger;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment env)
        {
            _logger = ConfigureLogger();
            _logger.Information("Logger configured");

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddJsonFile($"hosting.{env.EnvironmentName}.json")
                .AddUserSecrets<Startup>()
                .Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMemoryCache();

            services.AddSwaggerDocumentation();

            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });


            services.AddHttpContextAccessor();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            IExecutionContextAccessor executionContextAccessor =
                new ExecutionContextAccessor(serviceProvider.GetService<IHttpContextAccessor>());

            IEnumerable<IConfigurationSection> children = _configuration.GetSection("Caching").GetChildren();
            Dictionary<string, TimeSpan> cachingConfiguration =
                children.ToDictionary(child => child.Key, child => TimeSpan.Parse(child.Value));
            EmailsSettings emailsSettings = _configuration.GetSection("EmailsSettings").Get<EmailsSettings>();
            IMemoryCache memoryCache = serviceProvider.GetService<IMemoryCache>();
            return ApplicationStartup.Initialize(
                services,
                _configuration[OrdersConnectionString],
                new MemoryCacheStore(memoryCache, cachingConfiguration),
                null,
                emailsSettings,
                _logger,
                executionContextAccessor);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<CorrelationMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseProblemDetails();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwaggerDocumentation();
        }

        private static ILogger ConfigureLogger()
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.RollingFile(new CompactJsonFormatter(), "logs/logs")
                .CreateLogger();
        }
    }
}