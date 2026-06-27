using MediatR;

namespace EduBook.Application.Features.Books.Commands;

public record UpdateBookCommand(
    Guid Id,
    string Title,
    string? Description,
    string? ISBN,
    decimal Price,
    int TotalPages,
    string Language,
    int? GradeLevel,
    List<Guid> AuthorIds,
    List<Guid> CategoryIds
) : IRequest<UpdateBookResponse>;

public record UpdateBookResponse(
    Guid BookId,
    string Title,
    string Status
);