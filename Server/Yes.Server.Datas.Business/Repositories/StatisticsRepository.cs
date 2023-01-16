using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Datas.DbAccess;
using Yes.Server.Datas.DbAccess.Entities;

namespace Yes.Server.Datas.Business.Repositories
{
    /// <summary>
    /// Instance which implement IStatisticsRepository
    /// </summary>
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly IMapper _mapper;
        private readonly YesDbContext _ctx;

        public StatisticsRepository(IMapper mapper, YesDbContext ctx)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<Dictionary<string, int>> GetStatisticsForADrawAsync(int drawId)
        {
            return await _ctx.Statistics
                .Where(s => s.FKDrawId == drawId)
                .ToDictionaryAsync(g => $"rank{g.FKRankId}", g => g.PeopleByRank);
        }

        public async Task<int> CountParticipantsAsync(int drawId)
        {
            var requested = await _ctx.Statistics.FirstOrDefaultAsync(s => s.FKDrawId == drawId && s.FKRankId == 5);

            requested.PeopleByRank++;

            _ctx.Statistics.Update(requested);

            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> CreateStatisticsAsync(int drawId)
        {
            var stats = new List<StatisticEntity>();

            for (int i = 1; i <= 5; i++)
            {
                stats.Add(new StatisticEntity()
                {
                    FKDrawId = drawId,
                    FKRankId = i,
                    PeopleByRank = 0
                });
            }

            await _ctx.Statistics.AddRangeAsync(stats);
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> UpdateAllStatisticsAsync(int drawId)
        {
            var statistics = await _ctx.Statistics.Where(s => s.FKDrawId == drawId).ToListAsync();
            var tickets = await _ctx.Tickets.Where(s => s.FKDrawId == drawId).ToListAsync();

            var updatedStats = statistics.Select(s =>
            {
                s.PeopleByRank = tickets.Count(t => t.FKRankId == s.FKRankId);
                return s;

            }).ToList();

            _ctx.Statistics.UpdateRange(updatedStats);
            return await _ctx.SaveChangesAsync();
        }
    }
}
