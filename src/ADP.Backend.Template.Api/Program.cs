using System.Diagnostics.CodeAnalysis;
using ADP.Backend.Template.Api.Config;
using ADP.Backend.Template.Api.Extensions;
using ADP.Backend.Template.Api.HealthChecks;
using Asp.Versioning;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ADP.Backend.Template.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.UseHealthChecks("/healthy", new HealthCheckOptions()
            {
                Predicate = check => check.Name == "ready"
            });
            app.UseHealthChecks("/healthz", new HealthCheckOptions()
            {
                Predicate = check => check.Name == "live"
            });
            app.UseHealthChecks("/", new HealthCheckOptions()
            {
                Predicate = check => check.Name == "live"
            });

            app.Run();
        }

        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices();

            builder.Services.AddLogging();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddHealthChecks()
                   .AddCheck<LivenessCheck>("live")
                   .AddCheck<ReadinessCheck>("ready");

            var appInsightsConfig = new AppInsightsConfig
            {
                ConnectionString = builder.Configuration.GetValue<string>("AppInsights:ConnectionString") ?? string.Empty,
                CloudRole = builder.Configuration.GetValue<string>("AppInsights:CloudRole") ?? string.Empty
            };

            ConfigureOpenTelemetry(builder, appInsightsConfig);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "ADP.Backend.Template", Version = "v1" });
            });
            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }

        private static void ConfigureOpenTelemetry(WebApplicationBuilder builder, AppInsightsConfig appInsightsConfig)
        {
            if (!string.IsNullOrEmpty(appInsightsConfig.ConnectionString))
            {
                builder.Services.AddOpenTelemetry().UseAzureMonitor(options =>
                {
                    options.ConnectionString = appInsightsConfig.ConnectionString;

                    //var cred = new ChainedTokenCredential(
                    //        new AzureCliCredential(),
                    //        new DefaultAzureCredential());
                    //options.Credential = cred;
                    options.Credential = new DefaultAzureCredential();
                });
                if (!string.IsNullOrEmpty(appInsightsConfig.CloudRole))
                {
                    var resourceAttributes = new Dictionary<string, object> { { "service.name", appInsightsConfig.CloudRole } };
                    builder.Services.ConfigureOpenTelemetryTracerProvider((sp, b) =>
                    {
                        b.ConfigureResource(resourceBuilder => resourceBuilder.AddAttributes(resourceAttributes));
                    });
                }
                Console.WriteLine("App Insights Running!");
            }
            else
            {
                Console.WriteLine("App Insights Not Running!");
            }
        }
    }
}
