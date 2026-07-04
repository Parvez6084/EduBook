using MediatR;

namespace EduBook.Application.Features.Books.Queries;

public record SearchBooksQuery(
    string SearchTerm,
    int Page = 1,
    int PageSize = 10,
    string? Format = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    int? GradeLevel = null,
    Guid? CategoryId = null,
    Guid? AuthorId = null,
    Guid? PublisherId = null
) : IRequest<SearchBooksResponse>;

public record SearchBooksResponse(
    List<BookSearchDto> Books,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages,
    string SearchTerm
);

public record BookSearchDto(
    Guid Id,
    string Title,
    string? Description,
    decimal Price,
    string Format,
    string Status,
    string? CoverImageUrl,
    int TotalPages,
    string Language,
    int? GradeLevel,
    List<string> Authors,
    List<string> Categories
);