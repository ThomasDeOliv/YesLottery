using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Diagnostics.CodeAnalysis;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Scheduler.Helpers;
using Yes.Server.Datas.Business.DTO;

namespace Yes.Server.Scheduler
{
    /// <summary>
    /// Implementation of IJob corresponing to the background work of the server
    /// </summary>
    [ExcludeFromCodeCoverage, DisallowConcurrentExecution]
    public class SchedulerBackgroundService : IJob
    {
        private readonly ILogger<SchedulerBackgroundService> _logger;
        private readonly IDrawsRepository _drawsRepository;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IStatisticsRepository _statisticsRepository;

        private readonly Regex _regex;
        private int _currentDrawId;
        private string _currentCombination;

        public SchedulerBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            IServiceScope scope = serviceScopeFactory.CreateScope();

            _logger = scope.ServiceProvider.GetRequiredService<ILogger<SchedulerBackgroundService>>();
            _drawsRepository = scope.ServiceProvider.GetRequiredService<IDrawsRepository>();
            _ticketsRepository = scope.ServiceProvider.GetRequiredService<ITicketsRepository>();
            _statisticsRepository = scope.ServiceProvider.GetRequiredService<IStatisticsRepository>();

            _regex = new Regex("^(0[1-9]|[1-4][0-9])+(,((0[1-9]|[1-4][0-9]))){5}$");
            _currentDrawId = 0;
            _currentCombination = string.Empty;
        }

        /// <summary>
        /// Game loop
        /// </summary>
        /// <param name="context">Job Context</param>
        /// <returns>Executed Task</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // Create new draw
                _logger.LogInformation("Create a new draw");
                _currentDrawId = await _drawsRepository.CreateNewDrawAsync();

                // Create a related set of statistics
                _logger.LogInformation("Create all stats");
                await _statisticsRepository.CreateStatisticsAsync(_currentDrawId);

                // Waiting for 4 minutes
                _logger.LogInformation("Waiting for players...");
                await Task.Delay(TimeSpan.FromMinutes(4), context.CancellationToken);

                // Create a new random numbers combination
                _logger.LogInformation("Generate a new combination for the draw");
                _currentCombination = SchedulerHelpers.RandomOrderedCombinationGenerator(_regex);

                // Close the current draw
                _logger.LogInformation("Close the current draw");
                await _drawsRepository.CloseDrawAsync(_currentDrawId, _currentCombination);

                // Update all related tickets
                _logger.LogInformation("Update all the tickets");
                await _ticketsRepository.UpdateAllTicketsAsync(_currentCombination, _currentDrawId);

                // Update all statistics
                _logger.LogInformation("Update all statistics");
                await _statisticsRepository.UpdateAllStatisticsAsync(_currentDrawId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot add draw to database : {exception}", ex.InnerException.Message);
            }
        }
    }
}