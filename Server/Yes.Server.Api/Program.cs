using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using Yes.Server.Datas.Business.Extensions;
using Yes.Server.Datas.DbAccess;
using Yes.Server.Datas.DbAccess.Extensions;
using Yes.Server.Scheduler.Extensions;
using Yes.Server.Services.GameProvider.Extensions;

namespace Yes.Server.Api
{
    /// <summary>
    /// Main class of the app
    /// </summary>
    [ExcludeFromCodeCoverage]
    class ServerProgram
    {
        /// <summary>
        /// Main method of the app
        /// </summary>
        /// <param name="args">Some strings args passed through console</param>
        internal static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("YesConnectionString");
            string cronExpression = builder.Configuration["CronExpression"];
            string policyName = builder.Configuration["PolicyName"];

            // Origin policy
            builder.Services.AddCors(options =>
            {
#if DEBUG
                options.AddPolicy(
                    name: policyName,
                    policy =>
                    {
                        policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
#else
                options.AddPolicy(
                    name: policyName,
                    policy =>
                    {
                        policy
                        .WithOrigins(builder.Configuration["AllowedDomain"])
                        .WithMethods("GET", "POST")
                        .WithHeaders("Content-type", "Access-Control-Allow-Origin");
                    });
#endif
            });

            builder.Services.AddResponseCaching();

            // Add logger from serilog to IServiceCollection
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            // Add services to the container.
            builder.Services.AddMapperConfiguration();
            builder.Services.AddDbConfiguration(connectionString);
            builder.Services.AddRepositoriesConfiguration();
            builder.Services.AddSchedulerExtensions(cronExpression);
            builder.Services.AddGameProviderConfiguration();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Apply migrations if no databases found during the first execution
            using (IServiceScope serviceScope = app.Services.CreateScope())
            {
                YesDbContext _ctx = serviceScope.ServiceProvider.GetRequiredService<YesDbContext>();
                _ctx.Database.Migrate();
            };

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(policyName);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}