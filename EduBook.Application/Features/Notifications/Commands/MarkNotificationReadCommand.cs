using MediatR;

namespace EduBook.Application.Features.Notifications.Commands;

public record MarkNotificationReadCommand(
    Guid NotificationId,
    Guid UserId
) : IRequest<bool>;