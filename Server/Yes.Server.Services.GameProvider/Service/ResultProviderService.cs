using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Services.GameProvider.Service.Interfaces;
using Yes.Server.Services.ResultProvider.Models;

namespace Yes.Server.Services.GameProvider.Service
{
    /// <summary>
    /// Implementation of IResultProviderService
    /// </summary>
    public class ResultProviderService : IResultProviderService
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IMapper _mapper;

        public ResultProviderService(ITicketsRepository ticketsRepository, IStatisticsRepository statisticsRepository, IMapper mapper)
        {
            _ticketsRepository = ticketsRepository;
            _statisticsRepository = statisticsRepository;
            _mapper = mapper;
        }

        public async Task<ResultModel> GetResultsByAccessCodeAsync(string accessCode)
        {
            // Searching for a ticket related to the provided access code
            Ticket result = await _ticketsRepository.GetTicketByAccessCodeAsync(accessCode);

            if (result != null)
            {
                if (result.Draw.StartDateTime.AddMinutes(5) <= DateTime.UtcNow)
                {
                    // Convert played and drawed numbers to list
                    IEnumerable<int> played = result.PlayedNumbers.Split(',').Select(n => int.Parse(n));
                    IEnumerable<int> drawed = result.Draw.DrawedNumbers.Split(',').Select(n => int.Parse(n));

                    // Search the number of common numbers between the two lists
                    IEnumerable<int> cross = drawed.Intersect(played);
                    int rank = cross.Count();

                    // Transforming the ticket in result representation for the client
                    ResultModel model = _mapper.Map<Ticket, ResultModel>(result);

                    // Generate all related stats
                    model.Draw.Statistics = await _statisticsRepository.GetStatisticsForADrawAsync(result.FKDrawId);

                    // Return the model to the client
                    return model;
                }

                // If consultation request is too early, throws exception with custom message
                throw new Exception("ConsultingResultForbidden");
            }

            // If no ticket, throws exception with custom message
            throw new Exception("InexistingItem");
        }
    }
}
