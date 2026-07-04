using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Notifications.Commands;

public record SendNotificationToAllCommand(
    string Title,
    string Message,
    NotificationType Type,
    string? Data = null
) : IRequest<SendToAllResponse>;

public record SendToAllResponse(
    int TotalSent,
    string Title,
    string Message
);

public class SendNotificationToAllCommandHandler : BaseHandler, IRequestHandler<SendNotificationToAllCommand, SendToAllResponse>
{
    public SendNotificationToAllCommandHandler(IApplicationDbContext context) : base(context) { }

    public async Task<SendToAllResponse> Handle(SendNotificationToAllCommand request, CancellationToken cancellationToken)
    {
        var users = await Context.Users
            .Where(u => u.DeletedAt == null)
            .Select(u => u.Id)
            .ToListAsync(cancellationToken);

        var notifications = users.Select(userId => new Notification
        {
            UserId = userId,
            Title = request.Title,
            Message = request.Message,
            Type = request.Type,
            Data = request.Data
        }).ToList();

        await Context.Notifications.AddRangeAsync(notifications, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        return new SendToAllResponse(notifications.Count, request.Title, request.Message);
    }
}