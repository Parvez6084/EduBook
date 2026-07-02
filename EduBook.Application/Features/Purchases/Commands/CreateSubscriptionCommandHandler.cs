using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Enums;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Purchases.Commands;

public class CreateSubscriptionCommandHandler : BaseHandler, IRequestHandler<CreateSubscriptionCommand, CreateSubscriptionResponse>
{
    public CreateSubscriptionCommandHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<CreateSubscriptionResponse> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        // Check if user already has active subscription
        var existingSubscription = await Context.Subscriptions
            .FirstOrDefaultAsync(s =>
                s.UserId == request.UserId &&
                s.Status == SubscriptionStatus.Active &&
                s.EndDate > DateTime.UtcNow,
                cancellationToken);

        if (existingSubscription != null)
            throw new ValidationException("You already have an active subscription");

        // Check idempotency key
        var existingTransaction = await Context.PaymentTransactions
            .FirstOrDefaultAsync(t => t.IdempotencyKey == request.IdempotencyKey, cancellationToken);

        if (existingTransaction != null)
            throw new ValidationException("Duplicate request — this transaction already exists");


        // Calculate plan details
        var plan = Enum.Parse<SubscriptionPlan>(request.Plan);
        var startDate = DateTime.UtcNow;
        var endDate = plan == SubscriptionPlan.Monthly
            ? startDate.AddMonths(1)
            : startDate.AddYears(1);

        var amount = plan == SubscriptionPlan.Monthly ? 199m : 1999m;

        // Create payment transaction
        var transaction = new PaymentTransaction
        {
            UserId = request.UserId,
            Amount = amount,
            Gateway = Enum.Parse<PaymentGateway>(request.Gateway),
            Status = PaymentStatus.Pending,
            IdempotencyKey = request.IdempotencyKey
        };

        Context.PaymentTransactions.Add(transaction);
        await Context.SaveChangesAsync(cancellationToken);

        // Create subscription
        var subscription = new Subscription
        {
            UserId = request.UserId,
            Plan = plan,
            Status = SubscriptionStatus.Active,
            StartDate = startDate,
            EndDate = endDate,
            AutoRenew = true,
            PricePaid = amount,
            TransactionId = transaction.Id
        };

        Context.Subscriptions.Add(subscription);
        await Context.SaveChangesAsync(cancellationToken);

        return new CreateSubscriptionResponse(
            subscription.Id,
            transaction.Id,
            subscription.Plan.ToString(),
            subscription.StartDate,
            subscription.EndDate,
            amount,
            subscription.Status.ToString(),
            $"https://payment.edubook.com/pay/{transaction.Id}"
        );
    }
}