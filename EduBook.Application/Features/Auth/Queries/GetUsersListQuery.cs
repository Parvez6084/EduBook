using MediatR;

namespace EduBook.Application.Features.Auth.Queries;

public record GetUsersListQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    string? Role = null,
    string? Status = null
) : IRequest<GetUsersListResponse>;

public record UserListDto(
    Guid Id,
    string FullName,
    string? Email,
    string? PhoneNumber,
    string Role,
    string Status,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);

public record GetUsersListResponse(
    List<UserListDto> Users,
    int TotalCount,
    int Page,
    int PageSize
);