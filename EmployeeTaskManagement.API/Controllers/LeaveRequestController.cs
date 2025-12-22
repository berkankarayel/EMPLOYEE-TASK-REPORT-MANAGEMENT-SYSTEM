using EmployeeTaskManagement.Application.DTOs.LeaveRequest;
using EmployeeTaskManagement.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeTaskManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LeaveRequestController : ControllerBase
{
    private readonly ILeaveRequestService _leaveRequestService;
    private readonly IValidator<CreateLeaveRequestDto> _createValidator;
    private readonly IValidator<UpdateLeaveRequestDto> _updateValidator;

    public LeaveRequestController(
        ILeaveRequestService leaveRequestService,
        IValidator<CreateLeaveRequestDto> createValidator,
        IValidator<UpdateLeaveRequestDto> updateValidator)
    {
        _leaveRequestService = leaveRequestService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLeaveRequests()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if (userRole == "Admin")
        {
            var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync();
            return Ok(leaveRequests);
        }
        else
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var leaveRequests = await _leaveRequestService.GetLeaveRequestsByUserIdAsync(userId);
            return Ok(leaveRequests);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeaveRequestById(int id)
    {
        var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
        if (leaveRequest == null)
        {
            return NotFound(new { message = "İzin talebi bulunamadı." });
        }

        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        if (userRole != "Admin" && leaveRequest.UserId != userId)
        {
            return Forbid();
        }

        return Ok(leaveRequest);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestDto createLeaveRequestDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createLeaveRequestDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        try
        {
            var leaveRequest = await _leaveRequestService.CreateLeaveRequestAsync(createLeaveRequestDto, userId);
            return CreatedAtAction(nameof(GetLeaveRequestById), new { id = leaveRequest.Id }, leaveRequest);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateLeaveRequestStatus(int id, [FromBody] UpdateLeaveRequestDto updateDto)
    {
        if (id != updateDto.Id)
        {
            return BadRequest(new { message = "ID uyuşmazlığı." });
        }

        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var leaveRequest = await _leaveRequestService.UpdateLeaveRequestStatusAsync(id, updateDto.Status);
        if (leaveRequest == null)
        {
            return NotFound(new { message = "İzin talebi bulunamadı." });
        }

        return Ok(leaveRequest);
    }

    [HttpGet("pending")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPendingLeaveRequests()
    {
        var leaveRequests = await _leaveRequestService.GetPendingLeaveRequestsAsync();
        return Ok(leaveRequests);
    }
}
