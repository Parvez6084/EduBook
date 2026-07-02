using MediatR;

namespace EduBook.Application.Features.Purchases.Queries;

public record GetActiveSubscriptionQuery(Guid UserId) : IRequest<SubscriptionDto?>;

public record SubscriptionDto(
    Guid Id,
    string Plan,
    string Status,
    DateTime StartDate,
    DateTime EndDate,
    decimal PricePaid,
    bool AutoRenew,
    bool IsActive
);