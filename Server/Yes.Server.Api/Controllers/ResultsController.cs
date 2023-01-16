using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Yes.Server.Services.GameProvider.Service.Interfaces;
using Yes.Server.Services.ResultProvider.Models;

namespace Yes.Server.Api.Controllers
{
    /// <summary>
    /// Controller for all operations related to the results
    /// </summary>
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly ILogger<ResultsController> _logger;
        private readonly IResultProviderService _resultProviderService;

        public ResultsController(ILogger<ResultsController> logger, IResultProviderService resultProviderService)
        {
            _logger = logger;
            _resultProviderService = resultProviderService;
        }

        /// <summary>
        /// Controller for all operations related to the draws
        /// </summary>
        /// <param name="accessCode">Code provided by the client</param>
        /// <returns>Action result 200, 401 or 404</returns>
        [HttpGet("results/{accessCode}")]
        public async Task<IActionResult> GetResultByAccessCodeAsync(string accessCode)
        {
            // Secure operation
            try
            {
                // Searching a result with the provided access code
                ResultModel ticket = await _resultProviderService.GetResultsByAccessCodeAsync(accessCode);

                // If found, send the model in the body of a 200 Reponse
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                // If cennot send 200 response
                return ex.Message switch 
                {
                    // If it is too early to consult the result of the requested draw
                    "ConsultingResultForbidden" => Unauthorized(ex.Message),

                    // If bad code
                    _ => NotFound(ex.Message)
                };
            }
        }
    }
}