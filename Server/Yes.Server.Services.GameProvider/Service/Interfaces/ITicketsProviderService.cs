using System.Threading.Tasks;

namespace Yes.Server.Services.GameProvider.Service.Interfaces
{
    /// <summary>
    /// Interface to provide methods to tickets related service
    /// </summary>
    public interface ITicketsProviderService
    {
        /// <summary>
        /// Function to create a new ticket
        /// </summary>
        /// <param name="ints">Array of ints provided by user</param>
        /// <returns>Access Code generated</returns>
        public Task<string> CreateNewTicketAsync(int[] ints);
    }
}
