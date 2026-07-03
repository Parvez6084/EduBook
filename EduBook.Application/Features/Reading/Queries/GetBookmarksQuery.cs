using MediatR;

namespace EduBook.Application.Features.Reading.Queries;

public record GetBookmarksQuery(
    Guid UserId,
    Guid BookId
) : IRequest<List<BookmarkDto>>;

public record BookmarkDto(
    Guid Id,
    int PageNumber,
    string? Note,
    string? ChapterTitle,
    DateTime CreatedAt
);