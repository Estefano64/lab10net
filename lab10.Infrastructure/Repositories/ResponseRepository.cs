using Microsoft.EntityFrameworkCore;
using lab10.Domain.Entities;
using lab10.Domain.Interfaces;
using lab10.Infrastructure.Data;

namespace lab10.Infrastructure.Repositories;

public class ResponseRepository : Repository<Response>, IResponseRepository
{
    public ResponseRepository(TicketeraDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Response>> GetByTicketIdAsync(Guid ticketId)
    {
        return await _dbSet
            .Where(r => r.TicketId == ticketId)
            .Include(r => r.Responder)
            .OrderBy(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Response>> GetByResponderIdAsync(Guid responderId)
    {
        return await _dbSet
            .Where(r => r.ResponderId == responderId)
            .Include(r => r.Ticket)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }
}
