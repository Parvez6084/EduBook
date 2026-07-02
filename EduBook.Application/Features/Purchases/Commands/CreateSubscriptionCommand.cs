using MediatR;

namespace EduBook.Application.Features.Purchases.Commands;

public record CreateSubscriptionCommand(
    Guid UserId,
    string Plan,
    string Gateway,
    string IdempotencyKey
) : IRequest<CreateSubscriptionResponse>;

public record CreateSubscriptionResponse(
    Guid SubscriptionId,
    Guid TransactionId,
    string Plan,
    DateTime StartDate,
    DateTime EndDate,
    decimal Amount,
    string Status,
    string PaymentUrl
);