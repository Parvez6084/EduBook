using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Publishers.Commands;

public class DeletePublisherCommandHandler : BaseHandler, IRequestHandler<DeletePublisherCommand, bool>
{
    public DeletePublisherCommandHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> Handle(DeletePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = await Context.Publishers
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.DeletedAt == null, cancellationToken);

        if (publisher == null)
            throw new NotFoundException("Publisher not found");

        publisher.DeletedAt = DateTime.UtcNow;
        publisher.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
