using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Yes.Server.Services.GameProvider.Service;
using Yes.Server.Services.GameProvider.Service.Interfaces;

namespace Yes.Server.Services.GameProvider.Extensions
{
    /// <summary>
    /// Static class providing methods for subscribe the services of this assembly to the dependencies injection
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class GameProviderExtensions
    {
        /// <summary>
        /// Static Function for connecting this service to the main injection dependencies
        /// </summary>
        /// <param name="serviceCollection">IServiceCollection provided by injection dependencies</param>
        public static void AddGameProviderConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITicketsProviderService, TicketsProviderService>();
            serviceCollection.AddScoped<IDrawsProviderService, DrawsProviderService>();
            serviceCollection.AddScoped<IResultProviderService, ResultProviderService>();
        }
    }
}
