using EduBook.Application.Common;
using EduBook.Application.Features.Auth.Commands;
using EduBook.Application.Features.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduBook.API.Controllers.V1;

[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var result = await _mediator.Send(new GetDashboardStatsQuery());
        return Ok(ApiResponse<DashboardStatsDto>.Success(result, "Dashboard stats retrieved successfully"));
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? role = null,
        [FromQuery] string? status = null)
    {
        var result = await _mediator.Send(new GetUsersListQuery(page, pageSize, search, role, status));
        return Ok(ApiResponse<GetUsersListResponse>.Success(result, "Users retrieved successfully"));
    }

    [HttpPut("users/{userId}/ban")]
    public async Task<IActionResult> BanUser(Guid userId, [FromBody] BanUserRequest request)
    {
        await _mediator.Send(new BanUserCommand(userId, request.Ban));
        var message = request.Ban ? "User banned successfully" : "User unbanned successfully";
        return Ok(ApiResponse<string>.Success(message, message));
    }
}

public record BanUserRequest(bool Ban);