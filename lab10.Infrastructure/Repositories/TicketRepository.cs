using Microsoft.EntityFrameworkCore;
using lab10.Domain.Entities;
using lab10.Domain.Interfaces;
using lab10.Infrastructure.Data;

namespace lab10.Infrastructure.Repositories;

public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    public TicketRepository(TicketeraDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(t => t.UserId == userId)
            .Include(t => t.Responses)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetByStatusAsync(string status)
    {
        return await _dbSet
            .Where(t => t.Status == status)
            .Include(t => t.User)
            .ToListAsync();
    }

    public async Task<Ticket?> GetWithResponsesAsync(Guid ticketId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.Responses)
                .ThenInclude(r => r.Responder)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);
    }
}
