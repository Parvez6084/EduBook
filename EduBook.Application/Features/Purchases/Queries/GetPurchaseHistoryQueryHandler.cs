using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Purchases.Queries;

public class GetPurchaseHistoryQueryHandler : BaseHandler, IRequestHandler<GetPurchaseHistoryQuery, GetPurchaseHistoryResponse>
{
    public GetPurchaseHistoryQueryHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<GetPurchaseHistoryResponse> Handle(GetPurchaseHistoryQuery request, CancellationToken cancellationToken)
    {
        var query = Context.Purchases
            .Include(p => p.Book)
            .Where(p => p.UserId == request.UserId)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var purchases = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new PurchaseDto(
                p.Id,
                p.BookId,
                p.Book.Title,
                p.PricePaid,
                p.Status.ToString(),
                p.CreatedAt))
            .ToListAsync(cancellationToken);

        return new GetPurchaseHistoryResponse(purchases, totalCount, request.Page, request.PageSize);
    }
}