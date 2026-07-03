using MediatR;

namespace EduBook.Application.Features.Reading.Commands;


public record DeleteBookmarkCommand(Guid BookmarkId, Guid UserId) : IRequest<bool>;
public record DeleteHighlightCommand(Guid HighlightId, Guid UserId) : IRequest<bool>;
public record DeleteNoteCommand(Guid NoteId, Guid UserId) : IRequest<bool>;
public record ResetReadingProgressCommand(Guid UserId, Guid BookId) : IRequest<bool>;

