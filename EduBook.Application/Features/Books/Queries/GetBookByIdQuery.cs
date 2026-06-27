using MediatR;

namespace EduBook.Application.Features.Books.Queries;

public record GetBookByIdQuery(Guid Id) : IRequest<BookDetailDto>;

public record BookDetailDto(
    Guid Id,
    string Title,
    string? Description,
    string? ISBN,
    decimal Price,
    int TotalPages,
    string Language,
    string Format,
    string Status,
    string? CoverImageUrl,
    int? GradeLevel,
    DateTime? PublishedDate,
    List<string> Authors,
    List<string> Categories
);