using lab10.Application.DTOs;

namespace lab10.Application.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
    Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(Guid userId);
    Task<IEnumerable<TicketDto>> GetTicketsByStatusAsync(string status);
    Task<TicketDto?> GetTicketByIdAsync(Guid ticketId);
    Task<TicketDto> CreateTicketAsync(Guid userId, CreateTicketDto createTicketDto);
    Task<TicketDto?> UpdateTicketAsync(Guid ticketId, UpdateTicketDto updateTicketDto);
    Task<bool> DeleteTicketAsync(Guid ticketId);
    Task<bool> CloseTicketAsync(Guid ticketId);
}
