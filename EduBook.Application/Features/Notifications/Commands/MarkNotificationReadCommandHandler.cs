using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Notifications.Commands;

public class MarkNotificationReadCommandHandler : BaseHandler, IRequestHandler<MarkNotificationReadCommand, bool>
{
    public MarkNotificationReadCommandHandler(IApplicationDbContext context) : base(context) { }

    public async Task<bool> Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await Context.Notifications
            .FirstOrDefaultAsync(n =>
                n.Id == request.NotificationId &&
                n.UserId == request.UserId,
                cancellationToken);

        if (notification == null)
            throw new NotFoundException("Notification not found");

        notification.IsRead = true;
        notification.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync(cancellationToken);

        return true;
    }
}