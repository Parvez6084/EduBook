using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Auth.Queries;

public class GetDashboardStatsQueryHandler : BaseHandler, IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    public GetDashboardStatsQueryHandler(IApplicationDbContext context) : base(context) { }

    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var totalUsers = await Context.Users
            .CountAsync(u => u.DeletedAt == null, cancellationToken);

        var totalBooks = await Context.Books
            .CountAsync(b => b.DeletedAt == null, cancellationToken);

        var totalPurchases = await Context.Purchases
            .CountAsync(p => p.Status == PurchaseStatus.Completed, cancellationToken);

        var activeSubscriptions = await Context.Subscriptions
            .CountAsync(s => s.Status == SubscriptionStatus.Active &&
                            s.EndDate > DateTime.UtcNow, cancellationToken);

        var totalRevenue = await Context.PaymentTransactions
            .Where(t => t.Status == PaymentStatus.Completed)
            .SumAsync(t => t.Amount, cancellationToken);

        var newUsersToday = await Context.Users
            .CountAsync(u => u.CreatedAt.Date == today && u.DeletedAt == null, cancellationToken);

        var newPurchasesToday = await Context.Purchases
            .CountAsync(p => p.CreatedAt.Date == today &&
                            p.Status == PurchaseStatus.Completed, cancellationToken);

        return new DashboardStatsDto(
            totalUsers,
            totalBooks,
            totalPurchases,
            activeSubscriptions,
            totalRevenue,
            newUsersToday,
            newPurchasesToday
        );
    }
}