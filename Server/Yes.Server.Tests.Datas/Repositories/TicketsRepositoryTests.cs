using AutoMapper;
using System;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Datas.Business.Repositories;
using Yes.Server.Datas.DbAccess;
using Yes.Server.Datas.Business.DTO;
using System.Threading.Tasks;
using Yes.Server.Datas.DbAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Yes.Tests.Datas.Business.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TicketsRepositoryTests : IDisposable
    {
        private IMapper _mapper;
        private YesDbContext _ctx;
        private ITicketsRepository _ticketsRepository;

        public TicketsRepositoryTests()
        {
            _mapper = TestsHelpers.GetTestsMapper();
            _ctx = TestsHelpers.GetTestsContext();

            _ticketsRepository = new TicketsRepository(_mapper, _ctx);
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _ticketsRepository = null;
        }

        [Fact]
        public async Task GetAssociatedTicketsAsync_TakeDrawIdentityValue_ReturnExpectedList()
        {
            // ARRANGE *****************************************************************

            var expectedList = new List<Ticket>()
            {
                new Ticket()
                {
                    Id = 1,
                    PlayedNumbers = "02,05,15,17,25,45",
                    AccessCode = "M49Sd4pMNEyU8UGUmkSZFA",
                    GameDateTime = new DateTime(15,12,21,15,31,58),
                    FKRankId = 1,
                    FKDrawId = 1
                },
                new Ticket()
                {
                    Id = 2,
                    PlayedNumbers = "07,08,16,27,35,42",
                    AccessCode = "Yx4koyrOXU-1i8g-jGwzHw",
                    GameDateTime = new DateTime(15,12,21,15,32,05),
                    FKRankId = 4,
                    FKDrawId = 1
                },
                new Ticket()
                {
                    Id = 3,
                    PlayedNumbers = "02,05,15,19,27,45",
                    AccessCode = "KgHRdeg2hkmv9gGqzQzUzA",
                    GameDateTime = new DateTime(15,12,21,15,33,19),
                    FKRankId = 3,
                    FKDrawId = 1
                }
            };

            // ACT *********************************************************************

            var result = await _ticketsRepository.GetAssociatedTicketsAsync(1);

            // ASSERT ******************************************************************

            Assert.NotNull(result);

            Assert.True(result.Count == 3);
            Assert.Equal(expectedList.Count, result.Count);

            Assert.Equivalent(expectedList[0].Id, result[0].Id);
            Assert.Equivalent(expectedList[0].PlayedNumbers, result[0].PlayedNumbers);
            Assert.Equivalent(expectedList[0].AccessCode, result[0].AccessCode);
            Assert.Equivalent(expectedList[0].GameDateTime, result[0].GameDateTime);
            Assert.Equivalent(expectedList[0].FKRankId, result[0].FKRankId);
            Assert.Equivalent(expectedList[0].FKDrawId, result[0].FKDrawId);

            Assert.Equivalent(expectedList[1].Id, result[1].Id);
            Assert.Equivalent(expectedList[1].PlayedNumbers, result[1].PlayedNumbers);
            Assert.Equivalent(expectedList[1].AccessCode, result[1].AccessCode);
            Assert.Equivalent(expectedList[1].GameDateTime, result[1].GameDateTime);
            Assert.Equivalent(expectedList[1].FKRankId, result[1].FKRankId);
            Assert.Equivalent(expectedList[1].FKDrawId, result[1].FKDrawId);

            Assert.Equivalent(expectedList[2].Id, result[2].Id);
            Assert.Equivalent(expectedList[2].PlayedNumbers, result[2].PlayedNumbers);
            Assert.Equivalent(expectedList[2].AccessCode, result[2].AccessCode);
            Assert.Equivalent(expectedList[2].GameDateTime, result[2].GameDateTime);
            Assert.Equivalent(expectedList[2].FKRankId, result[2].FKRankId);
            Assert.Equivalent(expectedList[2].FKDrawId, result[2].FKDrawId);
        }

        [Fact]
        public async Task GetAssociatedTicketsAsync_TakeDrawIdentityValue_ReturnEmptyList()
        {
            // ARRANGE *****************************************************************

            var expectedList = new List<Ticket>();

            // ACT *********************************************************************

            var result = await _ticketsRepository.GetAssociatedTicketsAsync(2);

            // ASSERT ******************************************************************

            Assert.NotNull(result);

            Assert.True(result.Count == 0);
            Assert.Equal(expectedList.Count, result.Count);
        }

        [Fact]
        public async Task GetTicketByAccessCodeAsync_TakeValidAccessCodeValue_ReturnExpectedTicket()
        {
            // ARRANGE *****************************************************************

            var validAccessCode = "M49Sd4pMNEyU8UGUmkSZFA";

            // ACT *********************************************************************

            var result = await _ticketsRepository.GetTicketByAccessCodeAsync(validAccessCode);

            // ASSERT ******************************************************************

            Assert.NotNull(result);

            Assert.Equal(1, result.Id);
            Assert.Equal("02,05,15,17,25,45", result.PlayedNumbers);
            Assert.Equal("M49Sd4pMNEyU8UGUmkSZFA", result.AccessCode);
            Assert.Equal(new DateTime(15, 12, 21, 15, 31, 58), result.GameDateTime);
            Assert.Equal(1, result.FKDrawId);
            Assert.Equal(1, result.FKRankId);
        }

        [Fact]
        public async Task GetTicketByAccessCodeAsync_TakeInValidAccessCodeValue_ReturnNull()
        {
            // ARRANGE *****************************************************************

            var validAccessCode = "aaa";

            // ACT *********************************************************************

            var result = await _ticketsRepository.GetTicketByAccessCodeAsync(validAccessCode);

            // ASSERT ******************************************************************

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateNewTicketAsync_TakeTicket_ReturnExpectedInt()
        {
            // ARRANGE *****************************************************************

            _ctx.Draws.Add(new DrawEntity() { StartDateTime = DateTime.UtcNow.AddSeconds(10) });
            _ctx.SaveChanges();

            var registrationDate = DateTime.UtcNow;

            var expectedTicket = new Ticket()
            {
                Id = 5,
                PlayedNumbers = "01,02,03,04,05,06",
                AccessCode = "UI8hRjtcLUqcypDCisfLHA",
                FKDrawId = 4,
                FKRankId = 5,
                GameDateTime = registrationDate,
            };

            // ACT *********************************************************************

            var registrationResult = await _ticketsRepository.CreateNewTicketAsync(new Ticket()
            {
                PlayedNumbers = "01,02,03,04,05,06",
                AccessCode = "UI8hRjtcLUqcypDCisfLHA",
                FKDrawId = 4,
                FKRankId = 5,
                GameDateTime = registrationDate,
            });

            var result = _ctx.Tickets.FirstOrDefault(t => t.Id == expectedTicket.Id);

            // ASSERT ******************************************************************

            Assert.True(registrationResult != 0);

            Assert.Equal(expectedTicket.Id, result.Id);
            Assert.Equal(expectedTicket.PlayedNumbers, result.PlayedNumbers);
            Assert.Equal(expectedTicket.AccessCode, result.AccessCode);
            Assert.Equal(expectedTicket.FKDrawId, result.FKDrawId);
            Assert.Equal(expectedTicket.FKRankId, result.FKRankId);
            Assert.Equal(expectedTicket.GameDateTime, result.GameDateTime);
        }

        [Fact]
        public async Task UpdateAllTickets_TakeITicketsRepositoryDrawCombinationAndDrawId_UpdateAllTheRelatedTickets()
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
                    FKRankId = 5,
                },
                new TicketEntity()
                {
                    PlayedNumbers = "07,08,09,10,11,12",
                    GameDateTime = DateTime.UtcNow,
                    AccessCode = "uLyi-7A9DEOXceyAjqAyuQ",
                    FKDrawId = drawEntity.Id,
                    FKRankId = 5,
                }
            };
            await _ctx.Tickets.AddRangeAsync(ticketEntities);

            await _ctx.SaveChangesAsync();

            // ACT *********************************************************************

            await _ticketsRepository.UpdateAllTicketsAsync(drawEntity.DrawedNumbers, drawEntity.Id);

            var registredTickets = await _ctx.Tickets.Where(t => t.FKDrawId == 4).ToListAsync();

            // ASSERT ******************************************************************

            Assert.Equal(4, registredTickets[0].FKRankId);
            Assert.Equal(1, registredTickets[1].FKRankId);
        }
    }
}