using MediatR;

namespace EduBook.Application.Features.Purchases.Commands;

public record CreatePurchaseCommand(
    Guid UserId,
    Guid BookId,
    string Gateway,
    string IdempotencyKey
) : IRequest<CreatePurchaseResponse>;

public record CreatePurchaseResponse(
    Guid PurchaseId,
    Guid TransactionId,
    decimal Amount,
    string Status,
    string PaymentUrl
);