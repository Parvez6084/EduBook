using MediatR;

namespace EduBook.Application.Features.Auth.Queries;

public record GetDashboardStatsQuery : IRequest<DashboardStatsDto>;

public record DashboardStatsDto(
    int TotalUsers,
    int TotalBooks,
    int TotalPurchases,
    int ActiveSubscriptions,
    decimal TotalRevenue,
    int NewUsersToday,
    int NewPurchasesToday
);