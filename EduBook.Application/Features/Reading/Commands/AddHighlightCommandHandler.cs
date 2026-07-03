using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using MediatR;

namespace EduBook.Application.Features.Reading.Commands;

public class AddHighlightCommandHandler : BaseHandler, IRequestHandler<AddHighlightCommand, AddHighlightResponse>
{
    public AddHighlightCommandHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<AddHighlightResponse> Handle(AddHighlightCommand request, CancellationToken cancellationToken)
    {
        var highlight = new Highlight
        {
            UserId = request.UserId,
            BookId = request.BookId,
            PageNumber = request.PageNumber,
            SelectedText = request.SelectedText,
            Color = request.Color,
            Note = request.Note
        };

        Context.Highlights.Add(highlight);
        await Context.SaveChangesAsync(cancellationToken);

        return new AddHighlightResponse(
            highlight.Id,
            highlight.PageNumber,
            highlight.SelectedText,
            highlight.Color,
            highlight.Note
        );
    }
}