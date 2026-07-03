using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace EduBook.Application.Features.Reading.Commands;


public class DeleteBookmarkCommandHandler : BaseHandler, IRequestHandler<DeleteBookmarkCommand, bool>
{
    public DeleteBookmarkCommandHandler(IApplicationDbContext context) : base(context) { }

    public async Task<bool> Handle(DeleteBookmarkCommand request, CancellationToken cancellationToken)
    {
        var bookmark = await Context.Bookmarks
            .FirstOrDefaultAsync(b => b.Id == request.BookmarkId && b.UserId == request.UserId, cancellationToken);

        if (bookmark == null)
            throw new NotFoundException("Bookmark not found");

        Context.Bookmarks.Remove(bookmark);
        await Context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeleteHighlightCommandHandler : BaseHandler, IRequestHandler<DeleteHighlightCommand, bool>
{
    public DeleteHighlightCommandHandler(IApplicationDbContext context) : base(context) { }

    public async Task<bool> Handle(DeleteHighlightCommand request, CancellationToken cancellationToken)
    {
        var highlight = await Context.Highlights
            .FirstOrDefaultAsync(h => h.Id == request.HighlightId && h.UserId == request.UserId, cancellationToken);

        if (highlight == null)
            throw new NotFoundException("Highlight not found");

        Context.Highlights.Remove(highlight);
        await Context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeleteNoteCommandHandler : BaseHandler, IRequestHandler<DeleteNoteCommand, bool>
{
    public DeleteNoteCommandHandler(IApplicationDbContext context) : base(context) { }

    public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await Context.Notes
            .FirstOrDefaultAsync(n => n.Id == request.NoteId && n.UserId == request.UserId, cancellationToken);

        if (note == null)
            throw new NotFoundException("Note not found");

        Context.Notes.Remove(note);
        await Context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class ResetReadingProgressCommandHandler : BaseHandler, IRequestHandler<ResetReadingProgressCommand, bool>
{
    public ResetReadingProgressCommandHandler(IApplicationDbContext context) : base(context) { }

    public async Task<bool> Handle(ResetReadingProgressCommand request, CancellationToken cancellationToken)
    {
        var progress = await Context.ReadingProgresses
            .FirstOrDefaultAsync(r => r.UserId == request.UserId && r.BookId == request.BookId, cancellationToken);

        if (progress == null)
            throw new NotFoundException("Reading progress not found");

        Context.ReadingProgresses.Remove(progress);
        await Context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

