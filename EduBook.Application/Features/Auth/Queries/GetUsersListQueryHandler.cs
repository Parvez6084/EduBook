using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Auth.Queries;

public class GetUsersListQueryHandler : BaseHandler, IRequestHandler<GetUsersListQuery, GetUsersListResponse>
{
    public GetUsersListQueryHandler(IApplicationDbContext context) : base(context) { }

    public async Task<GetUsersListResponse> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
    {
        var query = Context.Users
            .Where(u => u.DeletedAt == null)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(u =>
                u.FullName.Contains(request.Search) ||
                (u.Email != null && u.Email.Contains(request.Search)) ||
                (u.PhoneNumber != null && u.PhoneNumber.Contains(request.Search)));

        if (!string.IsNullOrEmpty(request.Role))
        {
            var role = Enum.Parse<UserRole>(request.Role);
            query = query.Where(u => u.Role == role);
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            var status = Enum.Parse<UserStatus>(request.Status);
            query = query.Where(u => u.Status == status);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new UserListDto(
                u.Id,
                u.FullName,
                u.Email,
                u.PhoneNumber,
                u.Role.ToString(),
                u.Status.ToString(),
                u.CreatedAt,
                u.LastLoginAt))
            .ToListAsync(cancellationToken);

        return new GetUsersListResponse(users, totalCount, request.Page, request.PageSize);
    }
}