using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Authors.Queries;

public class GetAuthorsQueryHandler : BaseHandler, IRequestHandler<GetAuthorsQuery, GetAuthorsResponse>
{
    public GetAuthorsQueryHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<GetAuthorsResponse> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var query = Context.Authors
            .Where(a => a.DeletedAt == null)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(a => a.FullName.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var authors = await query
            .OrderBy(a => a.FullName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new AuthorDto(a.Id, a.FullName, a.Bio, a.ImageUrl))
            .ToListAsync(cancellationToken);

        return new GetAuthorsResponse(authors, totalCount, request.Page, request.PageSize);
    }
}