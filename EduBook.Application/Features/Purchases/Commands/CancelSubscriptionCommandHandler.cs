using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Enums;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Purchases.Commands;

public class CancelSubscriptionCommandHandler : BaseHandler, IRequestHandler<CancelSubscriptionCommand, bool>
{
    public CancelSubscriptionCommandHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await Context.Subscriptions
            .FirstOrDefaultAsync(s =>
                s.Id == request.SubscriptionId &&
                s.UserId == request.UserId,
                cancellationToken);

        if (subscription == null)
            throw new NotFoundException("Subscription not found");

        if (subscription.Status == SubscriptionStatus.Cancelled)
            throw new ValidationException("Subscription is already cancelled");

        subscription.Status = SubscriptionStatus.Cancelled;
        subscription.AutoRenew = false;
        subscription.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync(cancellationToken);

        return true;
    }
}