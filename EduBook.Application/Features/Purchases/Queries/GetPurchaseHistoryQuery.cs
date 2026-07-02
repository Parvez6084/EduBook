using MediatR;

namespace EduBook.Application.Features.Purchases.Queries;

public record GetPurchaseHistoryQuery(
    Guid UserId,
    int Page = 1,
    int PageSize = 10
) : IRequest<GetPurchaseHistoryResponse>;

public record PurchaseDto(
    Guid Id,
    Guid BookId,
    string BookTitle,
    decimal PricePaid,
    string Status,
    DateTime PurchasedAt
);

public record GetPurchaseHistoryResponse(
    List<PurchaseDto> Purchases,
    int TotalCount,
    int Page,
    int PageSize
);