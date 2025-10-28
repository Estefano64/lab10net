using lab10.Domain.Entities;

namespace lab10.Domain.Interfaces;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Ticket>> GetByStatusAsync(string status);
    Task<Ticket?> GetWithResponsesAsync(Guid ticketId);
}
