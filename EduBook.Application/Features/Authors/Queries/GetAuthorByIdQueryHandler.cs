using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Authors.Queries;

public class GetAuthorByIdQueryHandler : BaseHandler, IRequestHandler<GetAuthorByIdQuery, AuthorDto>
{
    public GetAuthorByIdQueryHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await Context.Authors
            .FirstOrDefaultAsync(a => a.Id == request.Id && a.DeletedAt == null, cancellationToken);

        if (author == null)
            throw new NotFoundException("Author not found");

        return new AuthorDto(author.Id, author.FullName, author.Bio, author.ImageUrl);
    }
}