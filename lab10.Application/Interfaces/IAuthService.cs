using lab10.Application.DTOs;

namespace lab10.Application.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
}
