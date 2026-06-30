using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Categories.Queries;

public class GetCategoriesQueryHandler : BaseHandler, IRequestHandler<GetCategoriesQuery, GetCategoriesResponse>
{
    public GetCategoriesQueryHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<GetCategoriesResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = Context.Categories
            .Where(c => c.DeletedAt == null)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(c => c.Name.Contains(request.Search));

        var totalCount = await query.CountAsync(cancellationToken);

        var categories = await query
            .OrderBy(c => c.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.Description,
                c.IconUrl,
                c.ParentCategoryId))
            .ToListAsync(cancellationToken);

        return new GetCategoriesResponse(categories, totalCount, request.Page, request.PageSize);
    }
}