using lab10.Application.DTOs;
using lab10.Application.Interfaces;
using lab10.Domain.Entities;
using lab10.Domain.Interfaces;

namespace lab10.Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
    {
        var tickets = await _ticketRepository.GetAllAsync();
        return tickets.Select(MapToDto);
    }

    public async Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(Guid userId)
    {
        var tickets = await _ticketRepository.GetByUserIdAsync(userId);
        return tickets.Select(MapToDto);
    }

    public async Task<IEnumerable<TicketDto>> GetTicketsByStatusAsync(string status)
    {
        var tickets = await _ticketRepository.GetByStatusAsync(status);
        return tickets.Select(MapToDto);
    }

    public async Task<TicketDto?> GetTicketByIdAsync(Guid ticketId)
    {
        var ticket = await _ticketRepository.GetWithResponsesAsync(ticketId);
        return ticket != null ? MapToDto(ticket) : null;
    }

    public async Task<TicketDto> CreateTicketAsync(Guid userId, CreateTicketDto createTicketDto)
    {
        var ticket = new Ticket
        {
            TicketId = Guid.NewGuid(),
            UserId = userId,
            Title = createTicketDto.Title,
            Description = createTicketDto.Description,
            Status = "Abierto",
            CreatedAt = DateTime.UtcNow
        };

        var createdTicket = await _ticketRepository.AddAsync(ticket);
        return MapToDto(createdTicket);
    }

    public async Task<TicketDto?> UpdateTicketAsync(Guid ticketId, UpdateTicketDto updateTicketDto)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null)
            return null;

        if (!string.IsNullOrEmpty(updateTicketDto.Title))
            ticket.Title = updateTicketDto.Title;

        if (updateTicketDto.Description != null)
            ticket.Description = updateTicketDto.Description;

        if (!string.IsNullOrEmpty(updateTicketDto.Status))
        {
            ticket.Status = updateTicketDto.Status;
            if (updateTicketDto.Status == "Cerrado")
                ticket.ClosedAt = DateTime.UtcNow;
        }

        await _ticketRepository.UpdateAsync(ticket);
        return MapToDto(ticket);
    }

    public async Task<bool> DeleteTicketAsync(Guid ticketId)
    {
        var exists = await _ticketRepository.ExistsAsync(ticketId);
        if (!exists)
            return false;

        await _ticketRepository.DeleteAsync(ticketId);
        return true;
    }

    public async Task<bool> CloseTicketAsync(Guid ticketId)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null)
            return false;

        ticket.Status = "Cerrado";
        ticket.ClosedAt = DateTime.UtcNow;
        await _ticketRepository.UpdateAsync(ticket);
        return true;
    }

    private static TicketDto MapToDto(Ticket ticket)
    {
        return new TicketDto
        {
            TicketId = ticket.TicketId,
            UserId = ticket.UserId,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt,
            ClosedAt = ticket.ClosedAt,
            Username = ticket.User?.Username,
            ResponseCount = ticket.Responses?.Count ?? 0
        };
    }
}
