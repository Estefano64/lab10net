using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using lab10.Application.DTOs;
using lab10.Application.Interfaces;
using System.Security.Claims;

namespace lab10.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResponsesController : ControllerBase
{
    private readonly IResponseService _responseService;

    public ResponsesController(IResponseService responseService)
    {
        _responseService = responseService;
    }

    /// <summary>
    /// Obtiene todas las respuestas de un ticket
    /// </summary>
    [HttpGet("ticket/{ticketId}")]
    public async Task<ActionResult<IEnumerable<ResponseDto>>> GetByTicketId(Guid ticketId)
    {
        var responses = await _responseService.GetResponsesByTicketIdAsync(ticketId);
        return Ok(responses);
    }

    /// <summary>
    /// Obtiene una respuesta por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseDto>> GetById(Guid id)
    {
        var response = await _responseService.GetResponseByIdAsync(id);
        if (response == null)
            return NotFound(new { message = "Respuesta no encontrada" });

        return Ok(response);
    }

    /// <summary>
    /// Crea una nueva respuesta en un ticket
    /// </summary>
    [HttpPost("ticket/{ticketId}")]
    public async Task<ActionResult<ResponseDto>> Create(Guid ticketId, [FromBody] CreateResponseDto createResponseDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Usuario no autenticado" });

        try
        {
            var response = await _responseService.CreateResponseAsync(ticketId, userId, createResponseDto);
            return CreatedAtAction(nameof(GetById), new { id = response.ResponseId }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Elimina una respuesta
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _responseService.DeleteResponseAsync(id);
        if (!result)
            return NotFound(new { message = "Respuesta no encontrada" });

        return Ok(new { message = "Respuesta eliminada exitosamente" });
    }
}
