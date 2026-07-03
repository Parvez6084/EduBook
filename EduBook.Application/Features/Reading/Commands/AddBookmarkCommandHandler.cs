using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using MediatR;

namespace EduBook.Application.Features.Reading.Commands;

public class AddBookmarkCommandHandler : BaseHandler, IRequestHandler<AddBookmarkCommand, AddBookmarkResponse>
{
    public AddBookmarkCommandHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<AddBookmarkResponse> Handle(AddBookmarkCommand request, CancellationToken cancellationToken)
    {
        var bookmark = new Bookmark
        {
            UserId = request.UserId,
            BookId = request.BookId,
            PageNumber = request.PageNumber,
            Note = request.Note,
            ChapterTitle = request.ChapterTitle
        };

        Context.Bookmarks.Add(bookmark);
        await Context.SaveChangesAsync(cancellationToken);

        return new AddBookmarkResponse(
            bookmark.Id,
            bookmark.PageNumber,
            bookmark.Note,
            bookmark.ChapterTitle
        );
    }
}