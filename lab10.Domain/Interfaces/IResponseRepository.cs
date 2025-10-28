using lab10.Domain.Entities;

namespace lab10.Domain.Interfaces;

public interface IResponseRepository : IRepository<Response>
{
    Task<IEnumerable<Response>> GetByTicketIdAsync(Guid ticketId);
    Task<IEnumerable<Response>> GetByResponderIdAsync(Guid responderId);
}
