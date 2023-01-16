using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Datas.DbAccess;
using Yes.Server.Datas.DbAccess.Entities;
using Yes.Server.Services.ResultProvider.Models;

namespace Yes.Tests.Services.GameProvider
{
    [ExcludeFromCodeCoverage]
    public static class GameProviderTestsHelpers
    {
        public static IMapper GetTestsMapper()
        {
            MapperConfiguration configuration = new MapperConfiguration(config =>
            {
                config.CreateMap<DrawEntity, Draw>()
                .ForMember(d => d.Id, opt => opt.MapFrom(t => t.Id))
                .ForMember(d => d.DrawedNumbers, opt => opt.MapFrom(t => t.DrawedNumbers))
                .ForMember(d => d.StartDateTime, opt => opt.MapFrom(t => t.StartDateTime))
                .ForMember(d => d.Tickets, opt => opt.MapFrom(t => t.Tickets))
                .ReverseMap();

                config.CreateMap<RankEntity, Rank>()
                .ForMember(r => r.Id, opt => opt.MapFrom(r => r.Id))
                .ForMember(r => r.Descriptor, opt => opt.MapFrom(r => r.Descriptor))
                .ForMember(r => r.Tickets, opt => opt.MapFrom(r => r.Tickets))
                .ReverseMap();

                config.CreateMap<TicketEntity, Ticket>()
                .ForMember(t => t.Id, opt => opt.MapFrom(t => t.Id))
                .ForMember(t => t.AccessCode, opt => opt.MapFrom(t => t.AccessCode))
                .ForMember(t => t.PlayedNumbers, opt => opt.MapFrom(t => t.PlayedNumbers))
                .ForMember(t => t.GameDateTime, opt => opt.MapFrom(t => t.GameDateTime))
                .ForMember(t => t.FKDrawId, opt => opt.MapFrom(t => t.FKDrawId))
                .ForMember(t => t.FKRankId, opt => opt.MapFrom(t => t.FKRankId))
                .ForMember(t => t.Draw, opt => opt.MapFrom(t => t.Draw))
                .ForMember(t => t.Rank, opt => opt.MapFrom(t => t.Rank))
                .ReverseMap();

                config.CreateMap<StatisticEntity, Statistic>()
                .ForMember(s => s.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(s => s.FKDrawId, opt => opt.MapFrom(s => s.FKDrawId))
                .ForMember(s => s.FKRankId, opt => opt.MapFrom(s => s.FKRankId))
                .ForMember(s => s.PeopleByRank, opt => opt.MapFrom(s => s.PeopleByRank))
                .ReverseMap();

                config.CreateMap<Ticket, ResultModel>()
                .ForMember(t => t.AccessCode, opt => opt.MapFrom(t => t.AccessCode))
                .ForMember(t => t.PlayedNumbers, opt => opt.MapFrom(t => t.PlayedNumbers))
                .ForMember(t => t.GameRank, opt => opt.MapFrom(t => t.FKRankId))
                .ForMember(t => t.GameDateTime, opt => opt.MapFrom(t => t.GameDateTime))
                .ForPath(t => t.Draw.DrawedNumbers, opt => opt.MapFrom(t => t.Draw.DrawedNumbers))
                .ForPath(t => t.Draw.EndDateTime, opt => opt.MapFrom(t => t.Draw.StartDateTime.AddMinutes(5)))
                .ForPath(t => t.Draw.Statistics, opt => opt.Ignore());
            });

            configuration.AssertConfigurationIsValid();

            return configuration.CreateMapper();
        }

        public static YesDbContext GetTestsContext() 
        {
            DbContextOptions<YesDbContext> options = new DbContextOptionsBuilder<YesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
            YesDbContext dbContext = new YesDbContext(options);

            dbContext.Draws.AddRange(new List<DrawEntity>()
            {
                new DrawEntity()
                {
                    Id = 1,
                    StartDateTime = new DateTime(15,12,21,15,30,00),
                    DrawedNumbers = "02,05,15,17,25,45"
                },
                new DrawEntity()
                {
                    Id = 2,
                    StartDateTime = new DateTime(15,12,21,15,35,00),
                    DrawedNumbers = "03,15,19,27,35,37",
                },
                new DrawEntity()
                {
                    Id = 3,
                    StartDateTime = DateTime.UtcNow.AddMinutes(-4).AddSeconds(-50),
                    DrawedNumbers = "03,15,19,27,35,37",
                }
            });

            dbContext.Tickets.AddRange(new List<TicketEntity>()
            {
                new TicketEntity()
                {
                    Id = 1,
                    PlayedNumbers = "02,05,15,17,25,45",
                    AccessCode = "M49Sd4pMNEyU8UGUmkSZFA",
                    GameDateTime = new DateTime(15,12,21,15,31,58),
                    FKRankId = 1,
                    FKDrawId = 1
                },
                new TicketEntity()
                {
                    Id = 2,
                    PlayedNumbers = "07,08,16,27,35,42",
                    AccessCode = "Yx4koyrOXU-1i8g-jGwzHw",
                    GameDateTime = new DateTime(15,12,21,15,32,05),
                    FKRankId = 4,
                    FKDrawId = 1
                },
                new TicketEntity()
                {
                    Id = 3,
                    PlayedNumbers = "02,05,15,19,27,45",
                    AccessCode = "KgHRdeg2hkmv9gGqzQzUzA",
                    GameDateTime = new DateTime(15,12,21,15,33,19),
                    FKRankId = 3,
                    FKDrawId = 1
                },
                new TicketEntity()
                {
                    Id = 4,
                    PlayedNumbers = "02,05,15,17,25,45",
                    AccessCode = "fUTtdOOYH0qZYAYfY-B7Dg",
                    GameDateTime = DateTime.UtcNow.AddMinutes(-1),
                    FKRankId = 5,
                    FKDrawId = 3
                }
            });

            dbContext.Statistics.AddRange(new List<StatisticEntity>()
            {
                new StatisticEntity()
                {
                    Id = 1,
                    FKDrawId = 1,
                    FKRankId = 1,
                    PeopleByRank = 1
                },
                new StatisticEntity()
                {
                    Id = 2,
                    FKDrawId = 1,
                    FKRankId = 2,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    Id = 3,
                    FKDrawId = 1,
                    FKRankId = 3,
                    PeopleByRank = 1
                },
                new StatisticEntity()
                {
                    Id = 4,
                    FKDrawId = 1,
                    FKRankId = 4,
                    PeopleByRank = 1
                },
                new StatisticEntity()
                {
                    Id = 5,
                    FKDrawId = 1,
                    FKRankId = 5,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    Id = 6,
                    FKDrawId = 2,
                    FKRankId = 1,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    Id = 7,
                    FKDrawId = 2,
                    FKRankId = 2,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    Id = 8,
                    FKDrawId = 2,
                    FKRankId = 3,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    Id = 9,
                    FKDrawId = 2,
                    FKRankId = 4,
                    PeopleByRank = 1
                },
                new StatisticEntity()
                {
                    Id = 10,
                    FKDrawId = 2,
                    FKRankId = 5,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    Id = 11,
                    FKDrawId = 3,
                    FKRankId = 1,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    Id = 12,
                    FKDrawId = 3,
                    FKRankId = 2,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    Id = 13,
                    FKDrawId = 3,
                    FKRankId = 3,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    Id = 14,
                    FKDrawId = 3,
                    FKRankId = 4,
                    PeopleByRank = 0
                },
                new StatisticEntity()
                {
                    Id = 15,
                    FKDrawId = 3,
                    FKRankId = 5,
                    PeopleByRank = 0
                }
            });

            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
