using System;
using System.Linq;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Services.GameProvider.Service.Interfaces;

namespace Yes.Server.Services.GameProvider.Service
{
    /// <summary>
    /// Implementation of IDrawsProviderService
    /// </summary>
    public class DrawsProviderService : IDrawsProviderService
    {
        private readonly IDrawsRepository _drawsRepository;

        public DrawsProviderService(IDrawsRepository drawsRepository)
        {
            _drawsRepository = drawsRepository;
        }

        public async Task<int> GetNumerOfParticipantsAsync()
        {
            // Request for the last draw
            Draw currentDraw = await _drawsRepository.GetCurrentDrawAsync();

            if(currentDraw != null)
            {
                // Send the number of ticket generated to this specific draw
                return currentDraw.Statistics.Sum(s => s.PeopleByRank);
            }

            // If no current draw, throws exception with custom message
            throw new Exception("NoCurrentGame");
        }
    }
}