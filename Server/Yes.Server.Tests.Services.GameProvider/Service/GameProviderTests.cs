using AutoMapper;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Datas.Business.DTO;
using System.Collections.Generic;
using Yes.Server.Services.GameProvider.Service.Interfaces;
using Yes.Server.Services.GameProvider.Service;
using Yes.Server.Services.ResultProvider.Models;
using Yes.Server.Datas.Business.Repositories;
using Yes.Server.Datas.DbAccess;

namespace Yes.Tests.Services.GameProvider.Service
{
    [ExcludeFromCodeCoverage]
    public class GameProviderTests : IDisposable
    {
        private IMapper _mapper;
        private YesDbContext _ctx;

        public GameProviderTests()
        {

        }

        public void Dispose()
        {
            if(_ctx != null)
            {
                _ctx.Dispose();
            }

            _mapper = null;
        }

        [Fact]
        public async Task GetCurrentDrawAsync_PlayAuthorized_ReturnExpectedCurrentDraw()
        {
            // ARRANGE *****************************************************************

            DateTime registrationDate = DateTime.UtcNow;

            Mock<IDrawsRepository> mockDrawsRepo = new Mock<IDrawsRepository>();
            mockDrawsRepo.Setup(m => m.GetCurrentDrawAsync()).ReturnsAsync(new Draw() { Id = 1, StartDateTime = registrationDate, DrawedNumbers = string.Empty, Statistics = new List<Statistic>() { new Statistic() { FKRankId = 1, PeopleByRank = 0 }, new Statistic() { FKRankId = 2, PeopleByRank = 0 }, new Statistic() { FKRankId = 3, PeopleByRank = 0 }, new Statistic() { FKRankId = 4, PeopleByRank = 0 }, new Statistic() { FKRankId = 5, PeopleByRank = 3 } }, Tickets = new List<Ticket>() { new Ticket(), new Ticket(), new Ticket() } });

            IDrawsProviderService drawsProviderService = new DrawsProviderService(mockDrawsRepo.Object);

            // ACT *********************************************************************

            var result = await drawsProviderService.GetNumerOfParticipantsAsync();

            // ASSERT ******************************************************************

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetCurrentDrawAsync_NoCurrentGame_ThrowsExpectedExceptionWithCustomMessage()
        {
            // ARRANGE *****************************************************************

            Mock<IDrawsRepository> mockDrawsRepo = new Mock<IDrawsRepository>();
            mockDrawsRepo.Setup(m => m.GetCurrentDrawAsync()).ReturnsAsync((Draw)null);

            IDrawsProviderService drawsProviderService = new DrawsProviderService(mockDrawsRepo.Object);

            // ACT *********************************************************************

            var result = await Assert.ThrowsAsync<Exception>(async () => await drawsProviderService.GetNumerOfParticipantsAsync());

            // ASSERT ******************************************************************

            Assert.Equal("NoCurrentGame", result.Message);
        }

        [Fact]
        public async Task CreateNewTicketAsync_TakeValidArray_ReturnExpectedString()
        {
            // ARRANGE *****************************************************************

            int[] ints = new int[] { 1, 2, 3, 4, 5, 6 };

            DateTime registrationDate = DateTime.UtcNow;

            Mock<IDrawsRepository> mockDrawsRepo = new Mock<IDrawsRepository>();
            mockDrawsRepo.Setup(m => m.GetCurrentDrawAsync()).ReturnsAsync(new Draw() { Id = 1, StartDateTime = registrationDate, DrawedNumbers = string.Empty, Statistics = new List<Statistic>(), Tickets = new List<Ticket>() });

            Mock<ITicketsRepository> mockTicketsRepo = new Mock<ITicketsRepository>();
            mockTicketsRepo.Setup(m => m.CreateNewTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(1);

            Mock<IStatisticsRepository> mockStatisticsRepo = new Mock<IStatisticsRepository>();
            mockStatisticsRepo.Setup(m => m.CountParticipantsAsync(It.IsAny<int>())).ReturnsAsync(1);

            ITicketsProviderService resultProviderService = new TicketsProviderService(mockTicketsRepo.Object, mockDrawsRepo.Object, mockStatisticsRepo.Object);

            // ACT *********************************************************************

            string result = await resultProviderService.CreateNewTicketAsync(ints);

            // ASSERT ******************************************************************

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(22, result.Length);
        }

        [Theory]
        [InlineData(new int[] { 0, 2, 3, 4, 5, 6 })]
        [InlineData(new int[] { 2, 2, 3, 4, 5, 6 })]
        [InlineData(new int[] { 1, 2, -3, 4, 5, 6 })]
        [InlineData(new int[] { 1, 2, 3, 50, 5, 6 })]
        [InlineData(new int[] { 1, 2, 3, 4, 51, 6 })]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 })]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [InlineData(new int[] { 1, 2, 3, 4, 5 })]
        [InlineData(new int[] { 2, 3, 4, 5, 6 })]
        [InlineData(new int[] { 1, 2, 3 })]
        public async Task CreateNewTicketAsync_TakeInvalidArray_ThrowsExpectedExceptionWithCustomMessage(int[] ints)
        {
            // ARRANGE *****************************************************************

            Mock<IDrawsRepository> mockDrawsRepo = new Mock<IDrawsRepository>();
            Mock<ITicketsRepository> mockTicketsRepo = new Mock<ITicketsRepository>();
            Mock<IStatisticsRepository> mockStatisticsRepo = new Mock<IStatisticsRepository>();

            ITicketsProviderService resultProviderService = new TicketsProviderService(mockTicketsRepo.Object, mockDrawsRepo.Object, mockStatisticsRepo.Object);

            // ACT *********************************************************************

            Exception result = await Assert.ThrowsAsync<Exception>(async () => await resultProviderService.CreateNewTicketAsync(ints));

            // ASSERT ******************************************************************

            Assert.Equal("InvalidArray", result.Message);
        }

        [Fact]
        public async Task CreateNewTicketAsync_TakeValidArray_ProblemWithTicketRegistration_ThrowsExpectedExceptionWithCustomMessage()
        {
            // ARRANGE *****************************************************************

            int[] ints = new int[] { 1, 2, 3, 4, 5, 6 };

            DateTime registrationDate = DateTime.UtcNow;

            Mock<IDrawsRepository> mockDrawsRepo = new Mock<IDrawsRepository>();
            mockDrawsRepo.Setup(m => m.GetCurrentDrawAsync()).ReturnsAsync(new Draw() { Id = 1, StartDateTime = registrationDate, DrawedNumbers = string.Empty, Statistics = new List<Statistic>(), Tickets = new List<Ticket>() });

            Mock<ITicketsRepository> mockTicketsRepo = new Mock<ITicketsRepository>();
            mockTicketsRepo.Setup(m => m.CreateNewTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(0);

            Mock<IStatisticsRepository> mockStatisticsRepo = new Mock<IStatisticsRepository>();
            mockStatisticsRepo.Setup(m => m.CountParticipantsAsync(It.IsAny<int>())).ReturnsAsync(1);

            ITicketsProviderService resultProviderService = new TicketsProviderService(mockTicketsRepo.Object, mockDrawsRepo.Object, mockStatisticsRepo.Object);

            // ACT *********************************************************************

            Exception result = await Assert.ThrowsAsync<Exception>(async () => await resultProviderService.CreateNewTicketAsync(ints));

            // ASSERT ******************************************************************

            Assert.Equal("FailedDrawRegistration", result.Message);
        }

        [Fact]
        public async Task CreateNewTicketAsync_TakeValidArray_ProblemWithIncrementation_ThrowsExpectedExceptionWithCustomMessage()
        {
            // ARRANGE *****************************************************************

            int[] ints = new int[] { 1, 2, 3, 4, 5, 6 };

            DateTime registrationDate = DateTime.UtcNow;

            Mock<IDrawsRepository> mockDrawsRepo = new Mock<IDrawsRepository>();
            mockDrawsRepo.Setup(m => m.GetCurrentDrawAsync()).ReturnsAsync(new Draw() { Id = 1, StartDateTime = registrationDate, DrawedNumbers = string.Empty, Statistics = new List<Statistic>(), Tickets = new List<Ticket>() });

            Mock<ITicketsRepository> mockTicketsRepo = new Mock<ITicketsRepository>();
            mockTicketsRepo.Setup(m => m.CreateNewTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(1);

            Mock<IStatisticsRepository> mockStatisticsRepo = new Mock<IStatisticsRepository>();
            mockStatisticsRepo.Setup(m => m.CountParticipantsAsync(It.IsAny<int>())).ReturnsAsync(0);

            ITicketsProviderService resultProviderService = new TicketsProviderService(mockTicketsRepo.Object, mockDrawsRepo.Object, mockStatisticsRepo.Object);

            // ACT *********************************************************************

            Exception result = await Assert.ThrowsAsync<Exception>(async () => await resultProviderService.CreateNewTicketAsync(ints));

            // ASSERT ******************************************************************

            Assert.Equal("FailedStatisticsRegistration", result.Message);
        }

        [Fact]
        public async Task CreateNewTicketAsync_TakeValidArray_NoNewDraw_ThrowsExpectedExceptionWithCustomMessage()
        {
            // ARRANGE *****************************************************************

            int[] ints = new int[] { 1, 2, 3, 4, 5, 6 };

            DateTime registrationDate = DateTime.UtcNow.AddMinutes(-4);

            Mock<IDrawsRepository> mockDrawsRepo = new Mock<IDrawsRepository>();
            mockDrawsRepo.Setup(m => m.GetCurrentDrawAsync()).ReturnsAsync(new Draw() { Id = 1, StartDateTime = registrationDate, Statistics = new List<Statistic>(), Tickets = new List<Ticket>() });

            Mock<ITicketsRepository> mockTicketsRepo = new Mock<ITicketsRepository>();

            Mock<IStatisticsRepository> mockStatisticsRepo = new Mock<IStatisticsRepository>();

            ITicketsProviderService resultProviderService = new TicketsProviderService(mockTicketsRepo.Object, mockDrawsRepo.Object, mockStatisticsRepo.Object);

            // ACT *********************************************************************

            Exception result = await Assert.ThrowsAsync<Exception>(async () => await resultProviderService.CreateNewTicketAsync(ints));

            // ASSERT ******************************************************************

            Assert.Equal("OldDraw", result.Message);
        }

        [Fact]
        public async Task CreateNewTicketAsync_TakeValidArray_TooLateToPlay_ThrowsExpectedExceptionWithCustomMessage()
        {
            // ARRANGE *****************************************************************

            int[] ints = new int[] { 1, 2, 3, 4, 5, 6 };

            DateTime registrationDate = DateTime.UtcNow.AddMinutes(-4);

            Mock<IDrawsRepository> mockDrawsRepo = new Mock<IDrawsRepository>();
            mockDrawsRepo.Setup(m => m.GetCurrentDrawAsync()).ReturnsAsync(new Draw() { Id = 1, StartDateTime = registrationDate, DrawedNumbers = "01,02,03,04,05,06", Statistics = new List<Statistic>(), Tickets = new List<Ticket>() });

            Mock<ITicketsRepository> mockTicketsRepo = new Mock<ITicketsRepository>();

            Mock<IStatisticsRepository> mockStatisticsRepo = new Mock<IStatisticsRepository>();

            ITicketsProviderService resultProviderService = new TicketsProviderService(mockTicketsRepo.Object, mockDrawsRepo.Object, mockStatisticsRepo.Object);

            // ACT *********************************************************************

            Exception result = await Assert.ThrowsAsync<Exception>(async () => await resultProviderService.CreateNewTicketAsync(ints));

            // ASSERT ******************************************************************

            Assert.Equal("ForbiddenGame", result.Message);
        }

        [Fact]
        public async Task CreateNewTicketAsync_TakeValidArray_NoCurrentGame_ThrowsExpectedExceptionWithCustomMessage()
        {
            // ARRANGE *****************************************************************

            int[] ints = new int[] { 1, 2, 3, 4, 5, 6 };

            Mock<IDrawsRepository> mockDrawsRepo = new Mock<IDrawsRepository>();
            mockDrawsRepo.Setup(m => m.GetCurrentDrawAsync()).ReturnsAsync((Draw)null);

            Mock<ITicketsRepository> mockTicketsRepo = new Mock<ITicketsRepository>();

            Mock<IStatisticsRepository> mockStatisticsRepo = new Mock<IStatisticsRepository>();

            ITicketsProviderService resultProviderService = new TicketsProviderService(mockTicketsRepo.Object, mockDrawsRepo.Object, mockStatisticsRepo.Object);

            // ACT *********************************************************************

            Exception result = await Assert.ThrowsAsync<Exception>(async () => await resultProviderService.CreateNewTicketAsync(ints));

            // ASSERT ******************************************************************

            Assert.Equal("NoCurrentGame", result.Message);
        }

        [Fact]
        public async Task GetResultsWithAccessCodeAsync_TakeValidAccessCode_ReturnExpectedModel()
        {
            // ARRANGE *****************************************************************

            string accessCode = "M49Sd4pMNEyU8UGUmkSZFA";

            Dictionary<string, int> stats = new Dictionary<string, int>()
            {
                {"rank1",1},
                {"rank2",0},
                {"rank3",1},
                {"rank4",1},
                {"rank5",0}
            };

            _mapper = GameProviderTestsHelpers.GetTestsMapper();
            _ctx = GameProviderTestsHelpers.GetTestsContext();

            TicketsRepository ticketsRepository = new TicketsRepository(_mapper, _ctx);
            StatisticsRepository statisticsRepository = new StatisticsRepository(_mapper, _ctx);

            IResultProviderService _service = new ResultProviderService(ticketsRepository, statisticsRepository, _mapper);

            // ACT *********************************************************************

            ResultModel result = await _service.GetResultsByAccessCodeAsync(accessCode);

            // ASSERT ******************************************************************

            Assert.Equal("M49Sd4pMNEyU8UGUmkSZFA", result.AccessCode);
            Assert.Equal("02,05,15,17,25,45", result.PlayedNumbers);
            Assert.Equal(new DateTime(15, 12, 21, 15, 31, 58), result.GameDateTime);
            Assert.Equal("02,05,15,17,25,45", result.Draw.DrawedNumbers);
            Assert.Equal(new DateTime(15, 12, 21, 15, 30, 00).AddMinutes(5), result.Draw.EndDateTime);
            Assert.Equivalent(stats, result.Draw.Statistics);
        }

        [Fact]
        public async Task GetResultsWithAccessCodeAsync_TakeInvalidAccessCode_ThrowsExpectedExceptionWithCustomMessage()
        {
            // ARRANGE *****************************************************************

            string accessCode = "aaaaaa";

            _mapper = GameProviderTestsHelpers.GetTestsMapper();
            _ctx = GameProviderTestsHelpers.GetTestsContext();

            TicketsRepository ticketsRepository = new TicketsRepository(_mapper, _ctx);
            StatisticsRepository statisticsRepository = new StatisticsRepository(_mapper, _ctx);

            IResultProviderService _service = new ResultProviderService(ticketsRepository, statisticsRepository, _mapper);

            // ACT *********************************************************************

            Exception result = await Assert.ThrowsAsync<Exception>(async () => await _service.GetResultsByAccessCodeAsync(accessCode));

            // ASSERT ******************************************************************

            Assert.Equal("InexistingItem", result.Message);
        }

        [Fact]
        public async Task GetResultsWithAccessCodeAsync_TakeValidAccessCode_TooEarlyConsulting_ThrowsExpectedExceptionWithCustomMessage()
        {
            // ARRANGE *****************************************************************

            string accessCode = "fUTtdOOYH0qZYAYfY-B7Dg";

            _mapper = GameProviderTestsHelpers.GetTestsMapper();
            _ctx = GameProviderTestsHelpers.GetTestsContext();

            TicketsRepository ticketsRepository = new TicketsRepository(_mapper, _ctx);
            StatisticsRepository statisticsRepository = new StatisticsRepository(_mapper, _ctx);

            IResultProviderService _service = new ResultProviderService(ticketsRepository, statisticsRepository, _mapper);

            // ACT *********************************************************************

            Exception result = await Assert.ThrowsAsync<Exception>(async () => await _service.GetResultsByAccessCodeAsync(accessCode));

            // ASSERT ******************************************************************

            Assert.Equal("ConsultingResultForbidden", result.Message);
        }
    }
}