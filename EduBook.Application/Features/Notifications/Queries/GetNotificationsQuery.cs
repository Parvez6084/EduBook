using MediatR;

namespace EduBook.Application.Features.Notifications.Queries;

public record GetNotificationsQuery(
    Guid UserId,
    int Page = 1,
    int PageSize = 20,
    bool? UnreadOnly = null
) : IRequest<GetNotificationsResponse>;

public record NotificationDto(
    Guid Id,
    string Title,
    string Message,
    string Type,
    bool IsRead,
    string? Data,
    DateTime CreatedAt
);

public record GetNotificationsResponse(
    List<NotificationDto> Notifications,
    int TotalCount,
    int UnreadCount,
    int Page,
    int PageSize
);