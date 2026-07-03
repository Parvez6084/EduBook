using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Reading.Queries;

public class GetHighlightsQueryHandler : BaseHandler, IRequestHandler<GetHighlightsQuery, List<HighlightDto>>
{
    public GetHighlightsQueryHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<HighlightDto>> Handle(GetHighlightsQuery request, CancellationToken cancellationToken)
    {
        return await Context.Highlights
            .Where(h => h.UserId == request.UserId && h.BookId == request.BookId)
            .OrderBy(h => h.PageNumber)
            .Select(h => new HighlightDto(
                h.Id,
                h.PageNumber,
                h.SelectedText,
                h.Color,
                h.Note,
                h.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}