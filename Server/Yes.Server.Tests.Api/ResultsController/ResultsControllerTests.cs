using System;
using System.Threading.Tasks;
using Yes.Server.Services.ResultProvider.Models;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Yes.Server.Api.Controllers;
using Yes.Server.Services.GameProvider.Service.Interfaces;

namespace Yes.Tests.Api.Controllers.ResultControllerTests
{
    [ExcludeFromCodeCoverage]
    public class ResultsControllerTests
    {
        private ILogger<ResultsController> _logger;
        private IResultProviderService _resultProvider;
        private ResultsController _resultController;

        [Fact]
        public async Task GetResultByAccessCodeAsync_ValidAccessCode_ReturnsOkObjectResult()
        {
            // ARRANGE *****************************************************************

            Mock<ILogger<ResultsController>> mockLogger = new Mock<ILogger<ResultsController>>();
            _logger = mockLogger.Object;

            Mock<IResultProviderService> mockService = new Mock<IResultProviderService>();
            mockService.Setup(m => m.GetResultsByAccessCodeAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<ResultModel>()); // Service return what is excepted
            _resultProvider = mockService.Object;

            _resultController = new ResultsController(_logger, _resultProvider);

            // ACT *********************************************************************

            string validAccessCode = "DP8hIgR_502VIxhKFhpPGA";
            var result = await _resultController.GetResultByAccessCodeAsync(validAccessCode);

            // ASSERT ******************************************************************

            Assert.Equal(typeof(OkObjectResult), result.GetType());
        }

        [Fact]
        public async Task GetResultByAccessCodeAsync_ServiceThrowsInexistingItem_ReturnsNotFoundObjectResult()
        {
            // ARRANGE *****************************************************************

            Mock<ILogger<ResultsController>> mockLogger = new Mock<ILogger<ResultsController>>();
            _logger = mockLogger.Object;

            Mock<IResultProviderService> mockService = new Mock<IResultProviderService>();
            mockService.Setup(m => m.GetResultsByAccessCodeAsync(It.IsAny<string>())).ThrowsAsync(new Exception("InexistingItem")); // Service throws an exception
            _resultProvider = mockService.Object;

            _resultController = new ResultsController(_logger, _resultProvider);

            // ACT *********************************************************************

            string invalidAccessCode = "DP8hIgR_503VIxhKFhpPGA";
            var result = await _resultController.GetResultByAccessCodeAsync(invalidAccessCode);

            // ASSERT ******************************************************************

            Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
        }

        [Fact]
        public async Task GetResultByAccessCodeAsync_ServiceThrowsExceptionConsultingResultForbidden_ReturnsUnauthorizedObjectResult()
        {
            // ARRANGE *****************************************************************

            Mock<ILogger<ResultsController>> mockLogger = new Mock<ILogger<ResultsController>>();
            _logger = mockLogger.Object;

            Mock<IResultProviderService> mockService = new Mock<IResultProviderService>();
            mockService.Setup(m => m.GetResultsByAccessCodeAsync(It.IsAny<string>())).ThrowsAsync(new Exception("ConsultingResultForbidden")); // Service throws an exception
            _resultProvider = mockService.Object;

            _resultController = new ResultsController(_logger, _resultProvider);

            // ACT *********************************************************************

            string invalidAccessCode = "DP8hIgR_503VIxhKFhpPGA";
            var result = await _resultController.GetResultByAccessCodeAsync(invalidAccessCode);

            // ASSERT ******************************************************************

            Assert.Equal(typeof(UnauthorizedObjectResult), result.GetType());
        }
    }
}