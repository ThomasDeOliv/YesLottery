using System.Threading.Tasks;

namespace Yes.Server.Services.GameProvider.Service.Interfaces
{
    /// <summary>
    /// Interface to provide methods to draws related service
    /// </summary>
    public interface IDrawsProviderService
    {
        /// <summary>
        /// Function to get informations related to the current draw
        /// </summary>
        /// <returns>Number of participants</returns>
        public Task<int> GetNumerOfParticipantsAsync();
    }
}
