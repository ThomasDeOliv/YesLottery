using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yes.Server.Datas.Business.Repositories.Interfaces
{   
    /// <summary>
    /// Interface to provide requests on the statistics table
    /// </summary>
    public interface IStatisticsRepository
    {
        /// <summary>
        /// Get all related stats of a specific draw
        /// </summary>
        /// <param name="drawId">Unique id of the concerned draw</param>
        /// <returns>Dictionary with requested stats</returns>
        public Task<Dictionary<string, int>> GetStatisticsForADrawAsync(int drawId);

        /// <summary>
        /// During game only - for counting people participating on a specific draw
        /// </summary>
        /// <param name="drawId">Unique id of the concerned draw</param>
        /// <returns>Number of records in database for current operation</returns>
        public Task<int> CountParticipantsAsync(int drawId);

        /// <summary>
        /// Function to initialise a set of stats when creating a new draw
        /// </summary>
        /// <param name="drawId">Unique id of the concerned draw</param>
        /// <returns>Number of records in database for current operation</returns>
        public Task<int> CreateStatisticsAsync(int drawId);

        /// <summary>
        /// Function to update the set of draw-related stats at the end of a game
        /// </summary>
        /// <param name="drawId">Unique id of the concerned draw</param>
        /// <returns>Number of records in database for current operation</returns>
        public Task<int> UpdateAllStatisticsAsync(int drawId);
    }
}