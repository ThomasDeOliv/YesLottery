using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Yes.Server.Api.Controllers;
using Yes.Server.Services.GameProvider.Service.Interfaces;

namespace Yes.Tests.Api.Controllers.GameControllerTests
{
    [ExcludeFromCodeCoverage]
    public class TicketsControllerTests
    {
        private ILogger<TicketsController> _logger;
        private ITicketsProviderService _ticketsProviderService;

        private TicketsController _ticketsController;

        [Fact]
        public async Task PostNewTicketAsync_TakeIntsArrayFromBody_ReturnsOkObjectResult()
        {
            // ARRANGE *****************************************************************

            // False ILogger
            Mock<ILogger<TicketsController>> mockLogger = new Mock<ILogger<TicketsController>>();
            _logger = mockLogger.Object;

            // Mock dependency with directed action 
            Mock<ITicketsProviderService> mockProvider = new Mock<ITicketsProviderService>();
            mockProvider.Setup(m => m.CreateNewTicketAsync(It.IsAny<int[]>())).ReturnsAsync(It.IsAny<string>()); // False service return what is expected
            _ticketsProviderService = mockProvider.Object;

            // Instanciation with mocks
            _ticketsController = new TicketsController(_logger, _ticketsProviderService);

            // ACT *********************************************************************

            var result = await _ticketsController.PostNewTicketAsync(new int[6] { 1, 2, 3, 4, 5, 6 });

            // ASSERT ******************************************************************

            Assert.Equal(typeof(OkObjectResult), result.GetType());
        }

        [Fact]
        public async Task PostNewTicketAsync_ServiceThrowsExceptionForbiddenGame_ReturnsUnauthorizedObjectResult()
        {
            // ARRANGE *****************************************************************

            Mock<ILogger<TicketsController>> mockLogger = new Mock<ILogger<TicketsController>>();
            _logger = mockLogger.Object;

            Mock<ITicketsProviderService> mockProvider = new Mock<ITicketsProviderService>();
            mockProvider.Setup(m => m.CreateNewTicketAsync(It.IsAny<int[]>())).ThrowsAsync(new Exception("ForbiddenGame")); // Service throws an exception
            _ticketsProviderService = mockProvider.Object;

            _ticketsController = new TicketsController(_logger, _ticketsProviderService);

            // ACT *********************************************************************

            var result = await _ticketsController.PostNewTicketAsync(new int[6] { 1, 2, 3, 4, 5, 6 });

            // ASSERT ******************************************************************

            Assert.Equal(typeof(UnauthorizedObjectResult), result.GetType());
        }

        [Fact]
        public async Task PostNewTicketAsync_ServiceThrowsExceptionOldDraw_ReturnsUnauthorizedObjectResult()
        {
            // ARRANGE *****************************************************************

            Mock<ILogger<TicketsController>> mockLogger = new Mock<ILogger<TicketsController>>();
            _logger = mockLogger.Object;

            Mock<ITicketsProviderService> mockProvider = new Mock<ITicketsProviderService>();
            mockProvider.Setup(m => m.CreateNewTicketAsync(It.IsAny<int[]>())).ThrowsAsync(new Exception("OldDraw")); // Service throws an exception
            _ticketsProviderService = mockProvider.Object;

            _ticketsController = new TicketsController(_logger, _ticketsProviderService);

            // ACT *********************************************************************

            var result = await _ticketsController.PostNewTicketAsync(new int[6] { 1, 2, 3, 4, 5, 6 });

            // ASSERT ******************************************************************

            Assert.Equal(typeof(UnauthorizedObjectResult), result.GetType());
        }

        [Fact]
        public async Task PostNewTicketAsync_ServiceThrowsExceptionInvalidArray_ReturnsUnauthorizedObjectResult()
        {
            // ARRANGE *****************************************************************

            Mock<ILogger<TicketsController>> mockLogger = new Mock<ILogger<TicketsController>>();
            _logger = mockLogger.Object;

            Mock<ITicketsProviderService> mockProvider = new Mock<ITicketsProviderService>();
            mockProvider.Setup(m => m.CreateNewTicketAsync(It.IsAny<int[]>())).ThrowsAsync(new Exception("InvalidArray")); // Service throws an exception
            _ticketsProviderService = mockProvider.Object;

            _ticketsController = new TicketsController(_logger, _ticketsProviderService);

            // ACT *********************************************************************

            var result = await _ticketsController.PostNewTicketAsync(new int[6] { 1, 2, 3, 4, 5, 6 });

            // ASSERT ******************************************************************

            Assert.Equal(typeof(UnauthorizedObjectResult), result.GetType());
        }

        [Fact]
        public async Task PostNewTicketAsync_ServiceThrowsExceptionFailedStatisticsRegistration_Returns500Error()
        {
            // ARRANGE *****************************************************************

            Mock<ILogger<TicketsController>> mockLogger = new Mock<ILogger<TicketsController>>();
            _logger = mockLogger.Object;

            Mock<ITicketsProviderService> mockProvider = new Mock<ITicketsProviderService>();
            mockProvider.Setup(m => m.CreateNewTicketAsync(It.IsAny<int[]>())).ThrowsAsync(new Exception("FailedStatisticsRegistration")); // Service throws an exception
            _ticketsProviderService = mockProvider.Object;

            _ticketsController = new TicketsController(_logger, _ticketsProviderService);

            // ACT *********************************************************************

            var result = await _ticketsController.PostNewTicketAsync(new int[6] { 1, 2, 3, 4, 5, 6 });

            // ASSERT ******************************************************************

            Assert.Equal(typeof(ForbidResult), result.GetType());
        }
    }
}