using AutoMapper;
using EmployeeTaskManagement.Application.DTOs.LeaveRequest;
using EmployeeTaskManagement.Application.Interfaces.Repositories;
using EmployeeTaskManagement.Application.Interfaces.Services;
using EmployeeTaskManagement.Domain.Entities;

namespace EmployeeTaskManagement.Application.Services;

public class LeaveRequestService : ILeaveRequestService
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISystemLogRepository _logRepository;
    private readonly IMapper _mapper;

    public LeaveRequestService(
        ILeaveRequestRepository leaveRequestRepository,
        IUserRepository userRepository,
        ISystemLogRepository logRepository,
        IMapper mapper)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _userRepository = userRepository;
        _logRepository = logRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LeaveRequestResponseDto>> GetAllLeaveRequestsAsync()
    {
        var leaveRequests = await _leaveRequestRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<LeaveRequestResponseDto>>(leaveRequests);
    }

    public async Task<LeaveRequestResponseDto?> GetLeaveRequestByIdAsync(int id)
    {
        var leaveRequest = await _leaveRequestRepository.GetLeaveRequestWithUserAsync(id);
        return leaveRequest == null ? null : _mapper.Map<LeaveRequestResponseDto>(leaveRequest);
    }

    public async Task<LeaveRequestResponseDto> CreateLeaveRequestAsync(CreateLeaveRequestDto createLeaveRequestDto, int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("Kullanıcı bulunamadı.");
        }

        var leaveRequest = _mapper.Map<LeaveRequest>(createLeaveRequestDto);
        leaveRequest.UserId = userId;
        leaveRequest.CreatedAt = DateTime.UtcNow;

        await _leaveRequestRepository.AddAsync(leaveRequest);
        await _leaveRequestRepository.SaveChangesAsync();

        await LogAsync("Leave Request Created", $"Leave request created by: {user.FullName}", "Info");

        var result = await _leaveRequestRepository.GetLeaveRequestWithUserAsync(leaveRequest.Id);
        return _mapper.Map<LeaveRequestResponseDto>(result);
    }

    public async Task<LeaveRequestResponseDto?> UpdateLeaveRequestStatusAsync(int id, string status)
    {
        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
        if (leaveRequest == null)
        {
            return null;
        }

        leaveRequest.Status = status;

        await _leaveRequestRepository.UpdateAsync(leaveRequest);
        await _leaveRequestRepository.SaveChangesAsync();

        var user = await _userRepository.GetByIdAsync(leaveRequest.UserId);
        await LogAsync("Leave Request Updated", $"Leave request {status} for: {user?.FullName}", "Info");

        var result = await _leaveRequestRepository.GetLeaveRequestWithUserAsync(id);
        return _mapper.Map<LeaveRequestResponseDto>(result);
    }

    public async Task<IEnumerable<LeaveRequestResponseDto>> GetLeaveRequestsByUserIdAsync(int userId)
    {
        var leaveRequests = await _leaveRequestRepository.GetLeaveRequestsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<LeaveRequestResponseDto>>(leaveRequests);
    }

    public async Task<IEnumerable<LeaveRequestResponseDto>> GetPendingLeaveRequestsAsync()
    {
        var leaveRequests = await _leaveRequestRepository.GetPendingLeaveRequestsAsync();
        return _mapper.Map<IEnumerable<LeaveRequestResponseDto>>(leaveRequests);
    }

    public async Task<IEnumerable<LeaveRequestResponseDto>> GetLeaveRequestsByStatusAsync(string status)
    {
        var leaveRequests = await _leaveRequestRepository.GetLeaveRequestsByStatusAsync(status);
        return _mapper.Map<IEnumerable<LeaveRequestResponseDto>>(leaveRequests);
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
