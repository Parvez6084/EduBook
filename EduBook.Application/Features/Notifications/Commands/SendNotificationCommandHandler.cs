using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using MediatR;

namespace EduBook.Application.Features.Notifications.Commands;

public class SendNotificationCommandHandler : BaseHandler, IRequestHandler<SendNotificationCommand, SendNotificationResponse>
{
    public SendNotificationCommandHandler(IApplicationDbContext context) : base(context) { }

    public async Task<SendNotificationResponse> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification
        {
            UserId = request.UserId,
            Title = request.Title,
            Message = request.Message,
            Type = request.Type,
            Data = request.Data
        };

        Context.Notifications.Add(notification);
        await Context.SaveChangesAsync(cancellationToken);

        return new SendNotificationResponse(
            notification.Id,
            notification.Title,
            notification.Message
        );
    }
}