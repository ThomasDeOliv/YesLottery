using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Server.Datas.DbAccess.Extensions
{
    /// <summary>
    /// Static class providing methods for subscribe the services of this assembly to the dependencies injection
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DatasExtensions
    {
        /// <summary>
        /// Static Function for connecting this service to the main injection dependencies
        /// </summary>
        /// <param name="serviceCollection">IServiceCollection provided by injection dependencies</param>
        /// <param name="connectionString">Connection String in appsettings.json provided by IConfiguration from dependencies injection</param>
        public static void AddDbConfiguration(this IServiceCollection serviceCollection, string connectionString)
        {
            // Subscribe DbContext instance
            serviceCollection.AddDbContext<YesDbContext>(options => 
            { 
                // Using SQL Server for this project
                options.UseSqlServer(connectionString)
#if DEBUG
                            .EnableSensitiveDataLogging()
#endif
                            ;
            });
        }
    }
}