using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using lab10.Application.DTOs;
using lab10.Application.Interfaces;
using System.Security.Claims;

namespace lab10.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    /// <summary>
    /// Obtiene todos los tickets
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetAll()
    {
        var tickets = await _ticketService.GetAllTicketsAsync();
        return Ok(tickets);
    }

    /// <summary>
    /// Obtiene un ticket por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDto>> GetById(Guid id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        return Ok(ticket);
    }

    /// <summary>
    /// Obtiene tickets por estado
    /// </summary>
    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetByStatus(string status)
    {
        var tickets = await _ticketService.GetTicketsByStatusAsync(status);
        return Ok(tickets);
    }

    /// <summary>
    /// Obtiene los tickets del usuario autenticado
    /// </summary>
    [HttpGet("my-tickets")]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetMyTickets()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Usuario no autenticado" });

        var tickets = await _ticketService.GetTicketsByUserIdAsync(userId);
        return Ok(tickets);
    }

    /// <summary>
    /// Crea un nuevo ticket
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TicketDto>> Create([FromBody] CreateTicketDto createTicketDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Usuario no autenticado" });

        var ticket = await _ticketService.CreateTicketAsync(userId, createTicketDto);
        return CreatedAtAction(nameof(GetById), new { id = ticket.TicketId }, ticket);
    }

    /// <summary>
    /// Actualiza un ticket existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TicketDto>> Update(Guid id, [FromBody] UpdateTicketDto updateTicketDto)
    {
        var ticket = await _ticketService.UpdateTicketAsync(id, updateTicketDto);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        return Ok(ticket);
    }

    /// <summary>
    /// Cierra un ticket
    /// </summary>
    [HttpPatch("{id}/close")]
    public async Task<IActionResult> Close(Guid id)
    {
        var result = await _ticketService.CloseTicketAsync(id);
        if (!result)
            return NotFound(new { message = "Ticket no encontrado" });

        return Ok(new { message = "Ticket cerrado exitosamente" });
    }

    /// <summary>
    /// Elimina un ticket
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _ticketService.DeleteTicketAsync(id);
        if (!result)
            return NotFound(new { message = "Ticket no encontrado" });

        return Ok(new { message = "Ticket eliminado exitosamente" });
    }
}
