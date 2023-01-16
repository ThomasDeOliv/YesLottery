using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Datas.DbAccess;
using Yes.Server.Datas.DbAccess.Entities;

namespace Yes.Server.Datas.Business.Repositories
{
    /// <summary>
    /// Instance which implement IDrawsRepository
    /// </summary>
    public class DrawsRepository : IDrawsRepository
    {
        private readonly IMapper _mapper;
        private readonly YesDbContext _ctx;

        public DrawsRepository(IMapper mapper, YesDbContext ctx)
        {
            _mapper = mapper;
            _ctx = ctx;
        }

        public async Task<Draw> GetCurrentDrawAsync()
        {
            // Searching for the draw with highest id number in the table
            var requestedDraw = await _ctx.Draws
                .Include(d => d.Statistics)
                .OrderBy(d => d.Id)
                .LastOrDefaultAsync();

            return _mapper.Map<DrawEntity, Draw>(requestedDraw);
        }

        public async Task<int> CreateNewDrawAsync()
        {
            // Adding a new draw to the table
            var draw = new DrawEntity()
            {
                StartDateTime = DateTime.UtcNow,
            };

            await _ctx.Draws.AddAsync(draw);
            return await _ctx.SaveChangesAsync() != 0 ? draw.Id : 0;
        }

        public async Task<int> CloseDrawAsync(int drawId, string drawCombination)
        {
            // Searching of the concerned draw
            var currentDraw = await _ctx.Draws.FirstOrDefaultAsync(d => d.Id == drawId);

            // Adding combination to the draw
            currentDraw.DrawedNumbers = drawCombination;

            _ctx.Draws.Update(currentDraw);

            return await _ctx.SaveChangesAsync();
        }
    }
}
