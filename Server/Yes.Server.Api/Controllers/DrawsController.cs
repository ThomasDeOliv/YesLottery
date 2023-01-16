using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Yes.Server.Services.GameProvider.Service.Interfaces;

namespace Yes.Server.Api.Controllers
{
    /// <summary>
    /// Controller for all operations related to the draws
    /// </summary>
    [ApiController]
    public class DrawsController : ControllerBase
    {
        private readonly ILogger<DrawsController> _logger;
        private readonly IDrawsProviderService _drawsProviderService;

        public DrawsController(ILogger<DrawsController> logger, IDrawsProviderService drawsProviderService)
        {
            _logger = logger;
            _drawsProviderService = drawsProviderService;
        }

        /// <summary>
        /// Action to get all participants of a draw
        /// </summary>
        /// <returns>ActionResult 200 or 404</returns>
        [HttpGet("/draws")]
        public async Task<IActionResult> GetCurrentDrawAsync()
        {
            // Secure operation
            try
            {
                // Ask the service to provide participants number
                int participants = await _drawsProviderService.GetNumerOfParticipantsAsync();

                // Return 200 response with requested information
                return Ok(participants);
            }
            catch (Exception ex)
            {
                // If no current draw, send 404 response with error message
                return NotFound(ex.Message);
            }
        }
    }
}
