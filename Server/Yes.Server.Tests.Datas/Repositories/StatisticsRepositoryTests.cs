using AutoMapper;
using System;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Datas.Business.Repositories;
using Yes.Server.Datas.DbAccess;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Datas.DbAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Yes.Tests.Datas.Business.Repositories
{
    [ExcludeFromCodeCoverage]
    public class StatisticsRepositoryTests : IDisposable
    {
        private IMapper _mapper;
        private YesDbContext _ctx;
        private IStatisticsRepository _statisticsRepository;

        public StatisticsRepositoryTests()
        {
            _mapper = TestsHelpers.GetTestsMapper();
            _ctx = TestsHelpers.GetTestsContext();

            _statisticsRepository = new StatisticsRepository(_mapper, _ctx);
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _statisticsRepository = null;
        }

        [Fact]
        public async Task GetStatisticsForADrawAsync_TakeDrawId_ReturnExpectedStatistics()
        {
            // ARRANGE *****************************************************************

            var expected = new Dictionary<string, int>
            {
                { "rank1", 1 },
                { "rank2", 0 },
                { "rank3", 1 },
                { "rank4", 1 },
                { "rank5", 0 }
            };

            // ACT *********************************************************************

            var result = await _statisticsRepository.GetStatisticsForADrawAsync(1);

            // ASSERT ******************************************************************

            Assert.NotNull(result);
            Assert.True(result.Count == 5);
            Assert.Equivalent(expected, result);
        }

        [Fact]
        public async Task IncrementStatisticBeforeEndForADrawByRankAsync_TakeDrawId_ReturnNumberOfAdd()
        {
            // ARRANGE *****************************************************************

            await _ctx.Draws.AddAsync(new DrawEntity() { StartDateTime = DateTime.UtcNow.AddSeconds(10) });
            await _ctx.Statistics.AddRangeAsync(
                new StatisticEntity[]
                {
                    new StatisticEntity() { FKDrawId = 4, FKRankId = 1, PeopleByRank = 0 },
                    new StatisticEntity() { FKDrawId = 4, FKRankId = 2, PeopleByRank = 0 },
                    new StatisticEntity() { FKDrawId = 4, FKRankId = 3, PeopleByRank = 0 },
                    new StatisticEntity() { FKDrawId = 4, FKRankId = 4, PeopleByRank = 0 },
                    new StatisticEntity() { FKDrawId = 4, FKRankId = 5, PeopleByRank = 0 },
                });

            await _ctx.SaveChangesAsync();

            // ACT *********************************************************************

            var result = await _statisticsRepository.CountParticipantsAsync(4);

            // ASSERT ******************************************************************

            Assert.Equal(1, result);

            Assert.Equal(0, (await _ctx.Statistics.FirstOrDefaultAsync(s => s.FKDrawId == 4 && s.FKRankId == 1)).PeopleByRank);
            Assert.Equal(0, (await _ctx.Statistics.FirstOrDefaultAsync(s => s.FKDrawId == 4 && s.FKRankId == 2)).PeopleByRank);
            Assert.Equal(0, (await _ctx.Statistics.FirstOrDefaultAsync(s => s.FKDrawId == 4 && s.FKRankId == 3)).PeopleByRank);
            Assert.Equal(0, (await _ctx.Statistics.FirstOrDefaultAsync(s => s.FKDrawId == 4 && s.FKRankId == 4)).PeopleByRank);
            Assert.Equal(1, (await _ctx.Statistics.FirstOrDefaultAsync(s => s.FKDrawId == 4 && s.FKRankId == 5)).PeopleByRank);
        }
        [Fact]
        public async Task CreateStatisticsAsync_TakeIDrawRepositoryAndIStatisticsRepository_CreateFiveNewStats()
        {
            // ARRANGE *****************************************************************

            DrawEntity drawEntity = new DrawEntity() { StartDateTime = DateTime.UtcNow };
            await _ctx.Draws.AddAsync(drawEntity);

            await _ctx.SaveChangesAsync();

            // ACT *********************************************************************

            var result = await _statisticsRepository.CreateStatisticsAsync(drawEntity.Id);
            var registredStats = await _ctx.Statistics.Where(s => s.FKDrawId == drawEntity.Id).ToListAsync();

            // ASSERT ******************************************************************

            Assert.True(registredStats.Count() == 5);
            Assert.True(registredStats.Count(s => s.FKDrawId != 4) == 0);
            Assert.True(registredStats.Count(s => s.PeopleByRank != 0) == 0);

            Assert.Equal(5, registredStats[0].FKRankId);
            Assert.Equal(4, registredStats[1].FKRankId);
            Assert.Equal(3, registredStats[2].FKRankId);
            Assert.Equal(2, registredStats[3].FKRankId);
            Assert.Equal(1, registredStats[4].FKRankId);
        }

        [Fact]
        public async Task UpdateAllStatistics_TakeIStatisticsRepositoryITicketsRepositoryAndDrawId_UpdateAllTheRelatedStats()
        {
            // ARRANGE *****************************************************************

            DrawEntity drawEntity = new DrawEntity() { StartDateTime = DateTime.UtcNow, DrawedNumbers = "01,02,03,04,05,06" };
            await _ctx.Draws.AddAsync(drawEntity);

            List<StatisticEntity> statisticEntities = new List<StatisticEntity>()
            {
                new StatisticEntity()
                {
                    FKDrawId = drawEntity.Id,
                    FKRankId = 1,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    FKDrawId = drawEntity.Id,
                    FKRankId = 2,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    FKDrawId = drawEntity.Id,
                    FKRankId = 3,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    FKDrawId = drawEntity.Id,
                    FKRankId = 4,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    FKDrawId = drawEntity.Id,
                    FKRankId = 5,
                    PeopleByRank = 0
                }
            };
            await _ctx.Statistics.AddRangeAsync(statisticEntities);

            List<TicketEntity> ticketEntities = new List<TicketEntity>()
            {
                new TicketEntity()
                {
                    PlayedNumbers = "01,02,03,04,05,06",
                    GameDateTime = DateTime.UtcNow.AddMinutes(-1),
                    AccessCode = "dy4jZYsQaUe1xOBvjalgTw",
                    FKDrawId = drawEntity.Id,
                    FKRankId = 1,
                },
                new TicketEntity()
                {
                    PlayedNumbers = "07,08,09,10,11,12",
                    GameDateTime = DateTime.UtcNow,
                    AccessCode = "uLyi-7A9DEOXceyAjqAyuQ",
                    FKDrawId = drawEntity.Id,
                    FKRankId = 4,
                }
            };
            await _ctx.Tickets.AddRangeAsync(ticketEntities);

            await _ctx.SaveChangesAsync();

            // ACT *********************************************************************

            await _statisticsRepository.UpdateAllStatisticsAsync(drawEntity.Id);
            var statsResult = await _ctx.Statistics.Where(t => t.FKDrawId == drawEntity.Id).ToListAsync();

            // ASSERT ******************************************************************

            Assert.Equal(5, statsResult[0].FKRankId);
            Assert.Equal(4, statsResult[0].FKDrawId);
            Assert.Equal(0, statsResult[0].PeopleByRank);

            Assert.Equal(4, statsResult[1].FKRankId);
            Assert.Equal(4, statsResult[1].FKDrawId);
            Assert.Equal(1, statsResult[1].PeopleByRank);

            Assert.Equal(3, statsResult[2].FKRankId);
            Assert.Equal(4, statsResult[2].FKDrawId);
            Assert.Equal(0, statsResult[2].PeopleByRank);

            Assert.Equal(2, statsResult[3].FKRankId);
            Assert.Equal(4, statsResult[3].FKDrawId);
            Assert.Equal(0, statsResult[3].PeopleByRank);

            Assert.Equal(1, statsResult[4].FKRankId);
            Assert.Equal(4, statsResult[4].FKDrawId);
            Assert.Equal(1, statsResult[4].PeopleByRank);
        }
    }
}