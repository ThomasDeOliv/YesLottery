using System.Threading.Tasks;
using Yes.Server.Services.ResultProvider.Models;

namespace Yes.Server.Services.GameProvider.Service.Interfaces
{
    /// <summary>
    /// Interface to provide methods to result related service
    /// </summary>
    public interface IResultProviderService
    {
        /// <summary>
        /// Function to get the result of a draw related to a specific ticket access code
        /// </summary>
        /// <param name="accessCode">Access code of a ticket</param>
        /// <returns>Representation of requested result</returns>
        public Task<ResultModel> GetResultsByAccessCodeAsync(string accessCode);
    }
}
