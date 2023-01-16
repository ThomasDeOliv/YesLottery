using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Scheduler.Extensions
{
    /// <summary>
    /// Static class providing methods for subscribe the services of this assembly to the dependencies injection
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SchedulerExtensions
    {
        /// <summary>
        /// Static Function for connecting this service to the main injection dependencies
        /// </summary>
        /// <param name="serviceCollection">IServiceCollection provided by injection dependencies</param>
        /// <param name="cronExpression">Cron expression in appsettings.json provided by IConfiguration from dependencies injection</param>
        public static void AddSchedulerExtensions(this IServiceCollection serviceDescriptors, string cronExpression)
        {
            serviceDescriptors.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                JobKey jobKey = new JobKey("DrawCreation");

                q.AddJob<SchedulerBackgroundService>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("DrawCreationTrigger")
                    .WithCronSchedule(cronExpression));
            });

            serviceDescriptors.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
