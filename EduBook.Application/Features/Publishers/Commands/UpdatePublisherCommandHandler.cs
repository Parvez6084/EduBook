using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Publishers.Commands;

public class UpdatePublisherCommandHandler : BaseHandler, IRequestHandler<UpdatePublisherCommand, UpdatePublisherResponse>
{
    public UpdatePublisherCommandHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<UpdatePublisherResponse> Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = await Context.Publishers
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.DeletedAt == null, cancellationToken);

        if (publisher == null)
            throw new NotFoundException("Publisher not found");

        publisher.Name = request.Name;
        publisher.Description = request.Description;
        publisher.LogoUrl = request.LogoUrl;
        publisher.Website = request.Website;
        publisher.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync(cancellationToken);

        return new UpdatePublisherResponse(publisher.Id, publisher.Name);
    }
}
