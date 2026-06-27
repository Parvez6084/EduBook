using MediatR;

namespace EduBook.Application.Features.Books.Commands;

public record CreateBookCommand(
    string Title,
    string? Description,
    string? ISBN,
    decimal Price,
    int TotalPages,
    string Language,
    string Format,
    int? GradeLevel,
    Guid? PublisherId,
    List<Guid> AuthorIds,
    List<Guid> CategoryIds
) : IRequest<CreateBookResponse>;

public record CreateBookResponse(
    Guid BookId,
    string Title,
    string Status
);