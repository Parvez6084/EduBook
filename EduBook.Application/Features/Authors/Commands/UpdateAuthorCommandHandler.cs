using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Authors.Commands;

public class UpdateAuthorCommandHandler : BaseHandler, IRequestHandler<UpdateAuthorCommand, UpdateAuthorResponse>
{
    public UpdateAuthorCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<UpdateAuthorResponse> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await Context.Authors
            .FirstOrDefaultAsync(a => a.Id == request.Id && a.DeletedAt == null, cancellationToken);

        if (author == null)
            throw new NotFoundException("Author not found");

        author.FullName = request.FullName;
        author.Bio = request.Bio;
        author.ImageUrl = request.ImageUrl;
        author.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync(cancellationToken);

        return new UpdateAuthorResponse(author.Id, author.FullName);
    }
}