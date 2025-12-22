using EmployeeTaskManagement.Application.DTOs.User;

namespace EmployeeTaskManagement.Application.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto?> GetUserByEmailAsync(string email);
    Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserResponseDto?> UpdateUserAsync(UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(int id);
    Task<IEnumerable<UserResponseDto>> GetActiveUsersAsync();
    Task<IEnumerable<UserResponseDto>> GetUsersByRoleAsync(string role);
}
