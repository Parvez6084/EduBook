using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Publishers.Queries;

public class GetPublisherByIdQueryHandler : BaseHandler, IRequestHandler<GetPublisherByIdQuery, PublisherDto>
{
    public GetPublisherByIdQueryHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<PublisherDto> Handle(GetPublisherByIdQuery request, CancellationToken cancellationToken)
    {
        var publisher = await Context.Publishers
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.DeletedAt == null, cancellationToken);

        if (publisher == null)
            throw new NotFoundException("Publisher not found");

        return new PublisherDto(
            publisher.Id,
            publisher.Name,
            publisher.Description,
            publisher.LogoUrl,
            publisher.Website);
    }
}
