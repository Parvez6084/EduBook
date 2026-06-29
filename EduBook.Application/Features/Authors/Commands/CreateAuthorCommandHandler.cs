using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using MediatR;

namespace EduBook.Application.Features.Authors.Commands;

public class CreateAuthorCommandHandler : BaseHandler, IRequestHandler<CreateAuthorCommand, CreateAuthorResponse>
{
    public CreateAuthorCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<CreateAuthorResponse> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = new Author
        {
            FullName = request.FullName,
            Bio = request.Bio,
            ImageUrl = request.ImageUrl
        };

        Context.Authors.Add(author);
        await Context.SaveChangesAsync(cancellationToken);

        return new CreateAuthorResponse(author.Id, author.FullName);
    }
}