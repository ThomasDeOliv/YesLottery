using CSharpVitamins;
using System.Linq;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Datas.Business.Repositories.Interfaces;

namespace Yes.Server.Services.GameProvider.Helpers
{
    /// <summary>
    /// Static class providing a library of functions for specific actions in this assembly
    /// </summary>
    public static class GameProviderServiceHelpers
    {
        /// <summary>
        /// Function to generate a unique id string of 22 characters
        /// </summary>
        /// <param name="ticketsRepository">Implementation of ITicketsRepository provided by dependency injection</param>
        /// <returns>Unique id string of 22 characters</returns>
        public static async Task<string> GenerateUniqueIdAsync(ITicketsRepository ticketsRepository)
        {
            // Containers
            string newId;
            Ticket result;

            // Verify if the ShortGUID is not already recorded in Database
            do
            {
                newId = ShortGuid.NewGuid();
                result = await ticketsRepository.GetTicketByAccessCodeAsync(newId);
            }
            while (result != null);

            // Return ShortGUID
            return newId;
        }

        /// <summary>
        /// Function to convert an array of ints in string ints separated by comas
        /// </summary>
        /// <param name="ints">Array of ints</param>
        /// <returns>Expected string</returns>
        public static string ConvertArrayOfIntsToString(int[] ints)
        {
            return string.Join(',', ints.Select(n => n == 0 ? "00" : (0 < n && n < 10 ? '0' + n.ToString() : n.ToString())).ToList());
        }
    }
}
