using AutoMapper;
using EmployeeTaskManagement.Application.DTOs.Auth;
using EmployeeTaskManagement.Application.Interfaces.Repositories;
using EmployeeTaskManagement.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace EmployeeTaskManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ISystemLogRepository _logRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthService(
        IUserRepository userRepository,
        ISystemLogRepository logRepository,
        IConfiguration configuration,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _logRepository = logRepository;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);

        if (user == null || !user.IsActive)
        {
            await LogAsync("Login Failed", $"Failed login attempt for email: {loginDto.Email}", "Warning");
            return null;
        }

        // Şifre kontrolü (BCrypt ile hash kontrolü yapılacak - şimdilik basit kontrol)
        if (!VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            await LogAsync("Login Failed", $"Invalid password for email: {loginDto.Email}", "Warning");
            return null;
        }

        var token = GenerateJwtToken(user.Id, user.Email, user.Role);

        await LogAsync("Login Success", $"User logged in: {user.Email}", "Info");

        return new LoginResponseDto
        {
            Token = token,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role
        };
    }

    public string GenerateJwtToken(int userId, string email, string role)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyForJWTTokenGeneration12345";
        var issuer = jwtSettings["Issuer"] ?? "EmployeeTaskManagement";
        var audience = jwtSettings["Audience"] ?? "EmployeeTaskManagementUsers";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        // BCrypt ile hash doğrulama
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    private async Task LogAsync(string action, string description, string level)
    {
        await _logRepository.AddAsync(new Domain.Entities.SystemLog
        {
            Action = action,
            Description = description,
            Level = level,
            CreatedAt = DateTime.UtcNow
        });
        await _logRepository.SaveChangesAsync();
    }
}
