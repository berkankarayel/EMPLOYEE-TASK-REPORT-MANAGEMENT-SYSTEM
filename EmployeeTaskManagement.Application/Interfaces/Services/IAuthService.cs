using EmployeeTaskManagement.Application.DTOs.Auth;

namespace EmployeeTaskManagement.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginDto);
    string GenerateJwtToken(int userId, string email, string role);
}
