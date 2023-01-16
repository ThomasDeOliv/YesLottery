using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Yes.Server.Services.GameProvider.Service.Interfaces;

namespace Yes.Server.Api.Controllers
{
    /// <summary>
    /// Controller for all operations related to the tickets
    /// </summary>
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ILogger<TicketsController> _logger;
        private readonly ITicketsProviderService _ticketsProviderService;

        public TicketsController(ILogger<TicketsController> logger, ITicketsProviderService ticketsProviderService)
        {
            _logger = logger;
            _ticketsProviderService = ticketsProviderService;
        }

        /// <summary>
        /// Action to register a new ticket
        /// </summary>
        /// <param name="ints">Ints arrat provided by the client</param>
        /// <returns>ActionResult 200, 401 or 404</returns>
        [HttpPost("/tickets")]
        public async Task<IActionResult> PostNewTicketAsync([FromBody] int[] ints)
        {
            try
            {
                var registredTicket = await _ticketsProviderService.CreateNewTicketAsync(ints);
                return Ok(registredTicket);
            }
            catch (Exception ex)
            {
                return ex.Message switch
                {
                    "ForbiddenGame" => Unauthorized(string.Format("Cannot access to the current draw : {0}", ex.Message)),

                    "InvalidArray" => Unauthorized(string.Format("Invalid Array passed through controller : {0}", ex.Message)),

                    "OldDraw" => Unauthorized(string.Format("Impossible to play to an old Draw among 4 minutes after its starting : {0}", ex.Message)),

                    _ => Forbid(ex.Message)
                };
            }
        }
    }
}
