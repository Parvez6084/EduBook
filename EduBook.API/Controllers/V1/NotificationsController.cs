using EduBook.Application.Common;
using EduBook.Application.Features.Notifications.Commands;
using EduBook.Application.Features.Notifications.Queries;
using EduBook.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduBook.API.Controllers.V1;

[ApiController]
[Route("api/v1/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool? unreadOnly = null)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _mediator.Send(new GetNotificationsQuery(userId, page, pageSize, unreadOnly));
        return Ok(ApiResponse<GetNotificationsResponse>.Success(result, "Notifications retrieved successfully"));
    }

    [HttpPut("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(Guid notificationId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _mediator.Send(new MarkNotificationReadCommand(notificationId, userId));
        return Ok(ApiResponse<string>.Success("Notification marked as read", "Notification marked as read"));
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _mediator.Send(new MarkAllNotificationsReadCommand(userId));
        return Ok(ApiResponse<string>.Success("All notifications marked as read", "All notifications marked as read"));
    }

    [HttpPost("send")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest request)
    {
        var command = new SendNotificationCommand(
            request.UserId,
            request.Title,
            request.Message,
            Enum.Parse<NotificationType>(request.Type),
            request.Data
        );

        var result = await _mediator.Send(command);
        return Ok(ApiResponse<SendNotificationResponse>.Success(result, "Notification sent successfully", 201));
    }

    [HttpPost("send-all")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> SendNotificationToAll([FromBody] SendToAllRequest request)
    {
        var command = new SendNotificationToAllCommand(
            request.Title,
            request.Message,
            Enum.Parse<NotificationType>(request.Type),
            request.Data
        );

        var result = await _mediator.Send(command);
        return Ok(ApiResponse<SendToAllResponse>.Success(result, $"Notification sent to {result.TotalSent} users", 201));
    }


}

public record SendNotificationRequest(
    Guid UserId,
    string Title,
    string Message,
    string Type,
    string? Data
);

public record SendToAllRequest(
    string Title,
    string Message,
    string Type,
    string? Data
);