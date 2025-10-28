using lab10.Application.DTOs;

namespace lab10.Application.Interfaces;

public interface IResponseService
{
    Task<IEnumerable<ResponseDto>> GetResponsesByTicketIdAsync(Guid ticketId);
    Task<ResponseDto?> GetResponseByIdAsync(Guid responseId);
    Task<ResponseDto> CreateResponseAsync(Guid ticketId, Guid responderId, CreateResponseDto createResponseDto);
    Task<bool> DeleteResponseAsync(Guid responseId);
}
