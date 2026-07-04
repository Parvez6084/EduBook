using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Notifications.Commands;

public record MarkAllNotificationsReadCommand(Guid UserId) : IRequest<bool>;

public class MarkAllNotificationsReadCommandHandler : BaseHandler, IRequestHandler<MarkAllNotificationsReadCommand, bool>
{
    public MarkAllNotificationsReadCommandHandler(IApplicationDbContext context) : base(context) { }

    public async Task<bool> Handle(MarkAllNotificationsReadCommand request, CancellationToken cancellationToken)
    {
        var notifications = await Context.Notifications
            .Where(n => n.UserId == request.UserId && !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.UpdatedAt = DateTime.UtcNow;
        }

        await Context.SaveChangesAsync(cancellationToken);

        return true;
    }
}