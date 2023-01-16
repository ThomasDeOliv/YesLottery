using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;

namespace Yes.Server.Datas.Business.Repositories.Interfaces
{
    /// <summary>
    /// Interface to provide requests on the draw table
    /// </summary>
    public interface IDrawsRepository
    {
        /// <summary>
        /// Function to get the last created draw
        /// </summary>
        /// <returns>The last Draw DTO</returns>
        public Task<Draw> GetCurrentDrawAsync();

        /// <summary>
        /// Function to create a new draw
        /// </summary>
        /// <returns>Number of record provided in database</returns>
        public Task<int> CreateNewDrawAsync();

        /// <summary>
        /// Function to close a specific draw
        /// </summary>
        /// <param name="drawId">Unique Id of the draw</param>
        /// <param name="drawCombination">A random generated draw combination of 6 numbers separated by comas</param>
        /// <returns>Number of record provided in database</returns>
        public Task<int> CloseDrawAsync(int drawId, string drawCombination);
    }
}
