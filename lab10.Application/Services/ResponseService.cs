using lab10.Application.DTOs;
using lab10.Application.Interfaces;
using lab10.Domain.Entities;
using lab10.Domain.Interfaces;

namespace lab10.Application.Services;

public class ResponseService : IResponseService
{
    private readonly IResponseRepository _responseRepository;
    private readonly ITicketRepository _ticketRepository;

    public ResponseService(IResponseRepository responseRepository, ITicketRepository ticketRepository)
    {
        _responseRepository = responseRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<IEnumerable<ResponseDto>> GetResponsesByTicketIdAsync(Guid ticketId)
    {
        var responses = await _responseRepository.GetByTicketIdAsync(ticketId);
        return responses.Select(MapToDto);
    }

    public async Task<ResponseDto?> GetResponseByIdAsync(Guid responseId)
    {
        var response = await _responseRepository.GetByIdAsync(responseId);
        return response != null ? MapToDto(response) : null;
    }

    public async Task<ResponseDto> CreateResponseAsync(Guid ticketId, Guid responderId, CreateResponseDto createResponseDto)
    {
        // Verify ticket exists
        var ticketExists = await _ticketRepository.ExistsAsync(ticketId);
        if (!ticketExists)
            throw new InvalidOperationException("El ticket no existe");

        var response = new Response
        {
            ResponseId = Guid.NewGuid(),
            TicketId = ticketId,
            ResponderId = responderId,
            Message = createResponseDto.Message,
            CreatedAt = DateTime.UtcNow
        };

        var createdResponse = await _responseRepository.AddAsync(response);
        return MapToDto(createdResponse);
    }

    public async Task<bool> DeleteResponseAsync(Guid responseId)
    {
        var exists = await _responseRepository.ExistsAsync(responseId);
        if (!exists)
            return false;

        await _responseRepository.DeleteAsync(responseId);
        return true;
    }

    private static ResponseDto MapToDto(Response response)
    {
        return new ResponseDto
        {
            ResponseId = response.ResponseId,
            TicketId = response.TicketId,
            ResponderId = response.ResponderId,
            Message = response.Message,
            CreatedAt = response.CreatedAt,
            ResponderUsername = response.Responder?.Username
        };
    }
}
