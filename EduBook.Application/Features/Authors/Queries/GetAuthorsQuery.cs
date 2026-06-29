using MediatR;

namespace EduBook.Application.Features.Authors.Queries;

public record GetAuthorsQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null
) : IRequest<GetAuthorsResponse>;

public record AuthorDto(
    Guid Id,
    string FullName,
    string? Bio,
    string? ImageUrl
);

public record GetAuthorsResponse(
    List<AuthorDto> Authors,
    int TotalCount,
    int Page,
    int PageSize
);