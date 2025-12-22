using AutoMapper;
using EmployeeTaskManagement.Application.DTOs.User;
using EmployeeTaskManagement.Application.Interfaces.Repositories;
using EmployeeTaskManagement.Application.Interfaces.Services;
using EmployeeTaskManagement.Domain.Entities;
using BCrypt.Net;

namespace EmployeeTaskManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ISystemLogRepository _logRepository;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        ISystemLogRepository logRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _logRepository = logRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null ? null : _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Email kontrolü
        if (await _userRepository.EmailExistsAsync(createUserDto.Email))
        {
            throw new InvalidOperationException("Bu email adresi zaten kullanılıyor.");
        }

        var user = _mapper.Map<User>(createUserDto);
        user.CreatedAt = DateTime.UtcNow;
        user.IsActive = true;

        // BCrypt ile şifre hashleme
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        await LogAsync("User Created", $"New user created: {user.Email}, Role: {user.Role}", "Info");

        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto?> UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(updateUserDto.Id);
        if (user == null)
        {
            return null;
        }

        // Email değişmişse ve başka kullanıcıda varsa hata
        if (user.Email != updateUserDto.Email && await _userRepository.EmailExistsAsync(updateUserDto.Email))
        {
            throw new InvalidOperationException("Bu email adresi zaten kullanılıyor.");
        }

        _mapper.Map(updateUserDto, user);

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        await LogAsync("User Updated", $"User updated: {user.Email}", "Info");

        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        await _userRepository.DeleteAsync(id);
        await _userRepository.SaveChangesAsync();

        await LogAsync("User Deleted", $"User deleted: {user.Email}", "Warning");

        return true;
    }

    public async Task<IEnumerable<UserResponseDto>> GetActiveUsersAsync()
    {
        var users = await _userRepository.GetActiveUsersAsync();
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    public async Task<IEnumerable<UserResponseDto>> GetUsersByRoleAsync(string role)
    {
        var users = await _userRepository.GetUsersByRoleAsync(role);
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    private async Task LogAsync(string action, string description, string level)
    {
        await _logRepository.AddAsync(new SystemLog
        {
            Action = action,
            Description = description,
            Level = level,
            CreatedAt = DateTime.UtcNow
        });
        await _logRepository.SaveChangesAsync();
    }
}
