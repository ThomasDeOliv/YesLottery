using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yes.Server.Datas.Business.DTO;
using Yes.Server.Datas.Business.Helpers;
using Yes.Server.Datas.Business.Repositories.Interfaces;
using Yes.Server.Datas.DbAccess;
using Yes.Server.Datas.DbAccess.Entities;

namespace Yes.Server.Datas.Business.Repositories
{
    /// <summary>
    /// Instance which implement ITicketsRepository
    /// </summary>
    public class TicketsRepository : ITicketsRepository
    {
        private readonly IMapper _mapper;
        private readonly YesDbContext _ctx;

        public TicketsRepository(IMapper mapper, YesDbContext ctx)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<Ticket>> GetAssociatedTicketsAsync(int drawId)
        {
            var requestedTickets = await _ctx.Tickets
                .Where(t => t.FKDrawId.Equals(drawId)).ToListAsync();

            return _mapper.Map<List<TicketEntity>, List<Ticket>>(requestedTickets);
        }

        public async Task<Ticket> GetTicketByAccessCodeAsync(string accessCode)
        {
            var requestedTicket = await _ctx.Tickets
                .Include(t => t.Draw)
                .ThenInclude(d => d.Statistics)
                .FirstOrDefaultAsync(t => t.AccessCode.Equals(accessCode));

            return _mapper.Map<TicketEntity, Ticket>(requestedTicket);
        }

        public async Task<int> CreateNewTicketAsync(Ticket ticket)
        {
            var ticketEntity = _mapper.Map<Ticket, TicketEntity>(ticket);

            await _ctx.Tickets.AddAsync(ticketEntity);
            int result = await _ctx.SaveChangesAsync();

            return result;
        }

        public async Task<int> UpdateAllTicketsAsync(string drawCombination, int drawId)
        {
            int resultTickets = 0;

            var associatedTickets = await _ctx.Tickets.Where(t => t.FKDrawId == drawId).ToListAsync();

            if (associatedTickets.Count != 0)
            {
                var updatesTickets = associatedTickets
                    .Select(t =>
                    {
                        t.FKRankId = BusinessHelpers.CalculateRankAsync(t.PlayedNumbers, drawCombination);
                        return t;

                    })
                    .ToList();

                _ctx.Tickets.UpdateRange(updatesTickets);
                resultTickets = await _ctx.SaveChangesAsync();
            }

            return resultTickets;
        }
    }
}
