using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using MediatR;

namespace EduBook.Application.Features.Publishers.Commands;

public class CreatePublisherCommandHandler : BaseHandler, IRequestHandler<CreatePublisherCommand, CreatePublisherResponse>
{
    public CreatePublisherCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<CreatePublisherResponse> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = new Publisher
        {
            Name = request.Name,
            Description = request.Description,
            LogoUrl = request.LogoUrl,
            Website = request.Website
        };

        Context.Publishers.Add(publisher);
        await Context.SaveChangesAsync(cancellationToken);

        return new CreatePublisherResponse(publisher.Id, publisher.Name);
    }
}