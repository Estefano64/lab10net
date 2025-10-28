using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using lab10.Application.DTOs;
using lab10.Application.Interfaces;
using lab10.Domain.Entities;
using lab10.Domain.Interfaces;

namespace lab10.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);

        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        return GenerateJwtToken(user);
    }

    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(registerDto.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists");
        }

        var existingEmail = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingEmail != null)
        {
            throw new InvalidOperationException("Email already exists");
        }

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = HashPassword(registerDto.Password),
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.AddAsync(user);

        return new UserDto
        {
            Id = (int)createdUser.UserId.GetHashCode(), // Temporal compatibility
            Username = createdUser.Username,
            Email = createdUser.Email ?? string.Empty,
            Role = "User",
            CreatedAt = createdUser.CreatedAt ?? DateTime.UtcNow
        };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? "DefaultSecretKeyForDevelopmentPurposesOnly123456";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, "User") // Default role
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"] ?? "lab10API",
            audience: jwtSettings["Audience"] ?? "lab10Client",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        // Simple hash - en producción usar BCrypt o similar
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(password + "SALT"));
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
}
