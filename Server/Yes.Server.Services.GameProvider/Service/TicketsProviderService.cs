using System;
using System.Linq;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Services.GameProvider.Helpers;
using Yes.Server.Services.GameProvider.Service.Interfaces;

namespace Yes.Server.Services.GameProvider.Service
{
    /// <summary>
    /// Implementation of ITicketsProviderService
    /// </summary>
    public class TicketsProviderService : ITicketsProviderService
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IDrawsRepository _drawsRepository;
        private readonly IStatisticsRepository _statisticsRepository;

        public TicketsProviderService(ITicketsRepository ticketsRepository, IDrawsRepository drawsRepository, IStatisticsRepository statisticsRepository)
        {
            _ticketsRepository = ticketsRepository;
            _drawsRepository = drawsRepository;
            _statisticsRepository = statisticsRepository;
        }

        public async Task<string> CreateNewTicketAsync(int[] ints)
        {
            DateTime registrationDate = DateTime.UtcNow;

            if (!ints.SequenceEqual(ints.Distinct()) || ints.Length != 6 || ints.Min() < 1 || ints.Max() > 49)
            {
                // If the format of the ints array is invalid, throws exception with custom message
                throw new Exception("InvalidArray");
            }

            // Searching for the current draw
            Draw currentDraw = await _drawsRepository.GetCurrentDrawAsync();

            if (currentDraw != null && string.IsNullOrEmpty(currentDraw.DrawedNumbers))
            {
                if (currentDraw.StartDateTime <= DateTime.UtcNow.AddMinutes(-4))
                {
                    // if the request arrive 4 minutes after the starting of the game, throws exception with custom message
                    throw new Exception("OldDraw");
                }

                // Format the current ints array
                int[] orderedNumbers = ints.OrderBy(n => n)
                                    .Distinct()
                                    .ToArray();

                // Generate the related string with the provided array
                string combination = GameProviderServiceHelpers.ConvertArrayOfIntsToString(orderedNumbers);

                // Create a ticket
                Ticket ticket = new Ticket()
                {
                    Id = 0,
                    AccessCode = await Helpers.GameProviderServiceHelpers.GenerateUniqueIdAsync(_ticketsRepository),
                    PlayedNumbers = combination,
                    GameDateTime = registrationDate,
                    FKRankId = 5,
                    FKDrawId = currentDraw.Id
                };

                // Register ticket in database
                int successDrawCreation = await _ticketsRepository.CreateNewTicketAsync(ticket);

                if (successDrawCreation == 0)
                {
                    // if error with the registration of the ticket, throws exception with custom message
                    throw new Exception("FailedDrawRegistration");
                }

                // Register related statistics in database
                int successStatCreation = await _statisticsRepository.CountParticipantsAsync(currentDraw.Id);

                if (successStatCreation == 0)
                {
                    // if error with the registration of the related statistics, throws exception with custom message
                    throw new Exception("FailedStatisticsRegistration");
                }

                return ticket.AccessCode;
            }

            // If the provided last draw contains a generated numbers string, throws exception with custom message
            throw new Exception(currentDraw != null ? "ForbiddenGame" : "NoCurrentGame");
        }
    }
}