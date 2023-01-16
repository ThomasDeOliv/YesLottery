using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using Yes.Server.Datas.DbAccess.Entities;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Services.ResultProvider.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Yes.Server.Api
{
    /// <summary>
    /// Static class providing methods for subscribe the services of this assembly to the dependencies injection
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MappingConfiguration
    {
        /// <summary>
        /// Static Function for connecting this service to the main injection dependencies
        /// </summary>
        /// <param name="serviceCollection">IServiceCollection provided by injection dependencies</param>
        public static void AddMapperConfiguration(this IServiceCollection serviceDescriptors)
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

            IMapper mapper = configuration.CreateMapper();

            serviceDescriptors.AddSingleton(mapper);
            serviceDescriptors.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}