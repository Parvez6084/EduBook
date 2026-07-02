using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Purchases.Queries;

public class GetActiveSubscriptionQueryHandler : BaseHandler, IRequestHandler<GetActiveSubscriptionQuery, SubscriptionDto?>
{
    public GetActiveSubscriptionQueryHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<SubscriptionDto?> Handle(GetActiveSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var subscription = await Context.Subscriptions
            .FirstOrDefaultAsync(s =>
                s.UserId == request.UserId &&
                s.Status == SubscriptionStatus.Active,
                cancellationToken);

        if (subscription == null)
            return null;

        return new SubscriptionDto(
            subscription.Id,
            subscription.Plan.ToString(),
            subscription.Status.ToString(),
            subscription.StartDate,
            subscription.EndDate,
            subscription.PricePaid,
            subscription.AutoRenew,
            subscription.IsActive
        );
    }
}