using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Notifications.Queries;

public class GetNotificationsQueryHandler : BaseHandler, IRequestHandler<GetNotificationsQuery, GetNotificationsResponse>
{
    public GetNotificationsQueryHandler(IApplicationDbContext context) : base(context) { }

    public async Task<GetNotificationsResponse> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var query = Context.Notifications
            .Where(n => n.UserId == request.UserId)
            .AsQueryable();

        if (request.UnreadOnly == true)
            query = query.Where(n => !n.IsRead);

        var totalCount = await query.CountAsync(cancellationToken);

        var unreadCount = await Context.Notifications
            .CountAsync(n => n.UserId == request.UserId && !n.IsRead, cancellationToken);

        var notifications = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(n => new NotificationDto(
                n.Id,
                n.Title,
                n.Message,
                n.Type.ToString(),
                n.IsRead,
                n.Data,
                n.CreatedAt))
            .ToListAsync(cancellationToken);

        return new GetNotificationsResponse(
            notifications,
            totalCount,
            unreadCount,
            request.Page,
            request.PageSize
        );
    }
}