using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetActiveUsersAsync();
    Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
    Task<bool> EmailExistsAsync(string email);
}
