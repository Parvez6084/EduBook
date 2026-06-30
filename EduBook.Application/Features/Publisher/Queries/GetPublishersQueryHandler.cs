using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Publishers.Queries;

public class GetPublishersQueryHandler : BaseHandler, IRequestHandler<GetPublishersQuery, GetPublishersResponse>
{
    public GetPublishersQueryHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<GetPublishersResponse> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
    {
        var query = Context.Publishers
            .Where(p => p.DeletedAt == null)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(p => p.Name.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var publishers = await query
            .OrderBy(p => p.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new PublisherDto(
                p.Id,
                p.Name,
                p.Description,
                p.LogoUrl,
                p.Website))
            .ToListAsync(cancellationToken);

        return new GetPublishersResponse(publishers, totalCount, request.Page, request.PageSize);
    }
}