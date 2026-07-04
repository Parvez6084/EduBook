using EduBook.Domain.Enums;
using MediatR;

namespace EduBook.Application.Features.Notifications.Commands;

public record SendNotificationCommand(
    Guid UserId,
    string Title,
    string Message,
    NotificationType Type,
    string? Data = null
) : IRequest<SendNotificationResponse>;

public record SendNotificationResponse(
    Guid NotificationId,
    string Title,
    string Message
);