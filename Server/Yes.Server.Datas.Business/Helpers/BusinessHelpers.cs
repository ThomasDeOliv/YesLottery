using System.Collections.Generic;
using System.Linq;

namespace Yes.Server.Datas.Business.Helpers
{
    /// <summary>
    /// Static class providing a library of functions for specific actions in this assembly
    /// </summary>
    public static class BusinessHelpers
    {
        /// <summary>
        /// Function to calculate the specific rank of a combination, in function of its related draw
        /// </summary>
        /// <param name="userCombination">Numbers played by the user</param>
        /// <param name="drawCombination">Numbers drawed by the server</param>
        /// <returns>A number corresponding to the related rank</returns>
        public static int CalculateRankAsync(string userCombination, string drawCombination)
        {
            // Transform the drawed numbers in IEnumerable<int>
            IEnumerable<int> requestedDrawNumbers = drawCombination.Split(',').Select(int.Parse);

            // Transform the played numbers in IEnumerable<int>
            IEnumerable<int> ticketNumbers = userCombination.Split(',').Select(int.Parse);

            // Searching for the common elements of the two previous IEnumerable<int>
            IEnumerable<int> intersection = requestedDrawNumbers.Intersect(ticketNumbers);

            // Return the corresponding rank, by the number of elements inside the intersection IEnumerable<int>
            return intersection.Count() switch
            {
                6 => 1,
                5 => 2,
                4 => 3,
                _ => 4,
            };
        }
    }
}
