using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Reading.Queries;

public class GetBookmarksQueryHandler : BaseHandler, IRequestHandler<GetBookmarksQuery, List<BookmarkDto>>
{
    public GetBookmarksQueryHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<BookmarkDto>> Handle(GetBookmarksQuery request, CancellationToken cancellationToken)
    {
        return await Context.Bookmarks
            .Where(b => b.UserId == request.UserId && b.BookId == request.BookId)
            .OrderBy(b => b.PageNumber)
            .Select(b => new BookmarkDto(
                b.Id,
                b.PageNumber,
                b.Note,
                b.ChapterTitle,
                b.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}