using EduBook.Application.Common;
using EduBook.Application.Features.Auth.Commands;
using EduBook.Application.Features.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduBook.API.Controllers.V1;

[ApiController]
[Route("api/v1/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _mediator.Send(new GetProfileQuery(userId));
        return Ok(ApiResponse<ProfileDto>.Success(result, "Profile retrieved successfully"));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new UpdateProfileCommand(userId, request.FullName, request.ProfileImageUrl);
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<UpdateProfileResponse>.Success(result, "Profile updated successfully"));
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword);
        await _mediator.Send(command);
        return Ok(ApiResponse<string>.Success("Password changed successfully", "Password changed"));
    }
}

public record UpdateProfileRequest(string FullName, string? ProfileImageUrl);
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);