using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using MediatR;

namespace EduBook.Application.Features.Reading.Commands;

public class AddNoteCommandHandler : BaseHandler, IRequestHandler<AddNoteCommand, AddNoteResponse>
{
    public AddNoteCommandHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<AddNoteResponse> Handle(AddNoteCommand request, CancellationToken cancellationToken)
    {
        var note = new Note
        {
            UserId = request.UserId,
            BookId = request.BookId,
            PageNumber = request.PageNumber,
            Content = request.Content
        };

        Context.Notes.Add(note);
        await Context.SaveChangesAsync(cancellationToken);

        return new AddNoteResponse(note.Id, note.PageNumber, note.Content);
    }
}