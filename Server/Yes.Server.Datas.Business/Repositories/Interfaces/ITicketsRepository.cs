using System.Collections.Generic;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;

namespace Yes.Server.Datas.Business.Repositories.Interfaces
{
    /// <summary>
    /// Interface to provide requests on the ticket table
    /// </summary>
    public interface ITicketsRepository
    {
        /// <summary>
        /// Function to get all related ticket of a specific draw
        /// </summary>
        /// <param name="drawId">Unique id of a draw</param>
        /// <returns>Requested list of tickets</Ticket></returns>
        public Task<List<Ticket>> GetAssociatedTicketsAsync(int drawId);

        /// <summary>
        /// Function to get a specific ticket by its unique access code
        /// </summary>
        /// <param name="accessCode">Access code</param>
        /// <returns>Requested ticket</returns>
        public Task<Ticket> GetTicketByAccessCodeAsync(string accessCode);

        /// <summary>
        /// Function to create a new ticket
        /// </summary>
        /// <param name="ticket">Ticket DTO</param>
        /// <returns>Records provided during this operation in database</returns>
        public Task<int> CreateNewTicketAsync(Ticket ticket);

        /// <summary>
        /// Function to update all ranks of tickets related to a draw
        /// </summary>
        /// <param name="drawCombination">Random combination of numbers generated for the related draw</param>
        /// <param name="drawId">Unique id of a draw</param>
        /// <returns>Records provided during this operation in database</returns>
        public Task<int> UpdateAllTicketsAsync(string drawCombination, int drawId);
    }
}
