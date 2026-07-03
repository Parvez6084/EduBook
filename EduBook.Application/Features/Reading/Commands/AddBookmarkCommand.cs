using MediatR;

namespace EduBook.Application.Features.Reading.Commands;

public record AddBookmarkCommand(
    Guid UserId,
    Guid BookId,
    int PageNumber,
    string? Note,
    string? ChapterTitle
) : IRequest<AddBookmarkResponse>;

public record AddBookmarkResponse(
    Guid Id,
    int PageNumber,
    string? Note,
    string? ChapterTitle
);