using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Yes.Server.Datas.Business.Repositories;
using Yes.Server.Datas.Business.Repositories.Interfaces;

namespace Yes.Server.Datas.Business.Extensions
{
    /// <summary>
    /// Static class providing methods for subscribe the services of this assembly to the dependencies injection
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DatasRepositoriesExtensions
    {
        /// <summary>
        /// Static Function for connecting the repositories service to the core injection dependencies
        /// </summary>
        /// <param name="serviceCollection">IServiceCollection provided by dependencies injection</param>
        public static void AddRepositoriesConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDrawsRepository, DrawsRepository>(); // Add Draw repository
            serviceCollection.AddScoped<IStatisticsRepository, StatisticsRepository>(); // Add Statistics repository
            serviceCollection.AddScoped<ITicketsRepository, TicketsRepository>(); // Add Tickets repository
        }
    }
}