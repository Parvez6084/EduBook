using MediatR;

namespace EduBook.Application.Features.Books.Queries;

public record GetBooksQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    string? Category = null,
    string? Format = null
) : IRequest<GetBooksResponse>;

public record BookDto(
    Guid Id,
    string Title,
    string? Description,
    decimal Price,
    string Format,
    string Status,
    string? CoverImageUrl,
    int TotalPages,
    string Language,
    int? GradeLevel
);

public record GetBooksResponse(
    List<BookDto> Books,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);