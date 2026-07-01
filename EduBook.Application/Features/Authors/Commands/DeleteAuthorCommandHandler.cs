using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Authors.Commands;

public class DeleteAuthorCommandHandler : BaseHandler, IRequestHandler<DeleteAuthorCommand, bool>
{
    public DeleteAuthorCommandHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await Context.Authors
            .FirstOrDefaultAsync(a => a.Id == request.Id && a.DeletedAt == null, cancellationToken);

        if (author == null)
            throw new NotFoundException("Author not found");

        author.DeletedAt = DateTime.UtcNow;
        author.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync(cancellationToken);

        return true;
    }
}