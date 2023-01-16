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
    public class DrawsControllerTests
    {
        private ILogger<DrawsController> _logger;
        private IDrawsProviderService _drawsProviderService;

        private DrawsController _drawsController;

        [Fact]
        public async Task GetCurrentDrawAsync_ReturnsOkObjectResult()
        {
            // ARRANGE *****************************************************************

            Mock<ILogger<DrawsController>> mockLogger = new Mock<ILogger<DrawsController>>();
            _logger = mockLogger.Object;

            Mock<IDrawsProviderService> mockProvider = new Mock<IDrawsProviderService>();
            mockProvider.Setup(m => m.GetNumerOfParticipantsAsync()).ReturnsAsync(It.IsAny<int>()); // Service returns what is excepted
            _drawsProviderService = mockProvider.Object;

            _drawsController = new DrawsController(_logger, _drawsProviderService);

            // ACT *********************************************************************

            var result = await _drawsController.GetCurrentDrawAsync();

            // ASSERT ******************************************************************

            Assert.Equal(typeof(OkObjectResult), result.GetType());
        }

        [Fact]
        public async Task GetCurrentDrawAsync_ServiceThrowsExceptionNoCurrentGame_ReturnsNotFoundObjectResult()
        {
            // ARRANGE *****************************************************************

            Mock<ILogger<DrawsController>> mockLogger = new Mock<ILogger<DrawsController>>();
            _logger = mockLogger.Object;

            Mock<IDrawsProviderService> mockProvider = new Mock<IDrawsProviderService>();
            mockProvider.Setup(m => m.GetNumerOfParticipantsAsync()).ThrowsAsync(new Exception("NoCurrentGame"));
            _drawsProviderService = mockProvider.Object;

            _drawsController = new DrawsController(_logger, _drawsProviderService);

            // ACT *********************************************************************

            var result = await _drawsController.GetCurrentDrawAsync();

            // ASSERT ******************************************************************

            Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
        }
    }
}