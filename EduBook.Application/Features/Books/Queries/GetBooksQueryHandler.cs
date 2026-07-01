using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Books.Queries;

public class GetBooksQueryHandler : BaseHandler, IRequestHandler<GetBooksQuery, GetBooksResponse>
{
    public GetBooksQueryHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<GetBooksResponse> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var query = Context.Books
            .Where(b => b.DeletedAt == null)
            .AsQueryable();

        // Search filter
        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(b => b.Title.Contains(request.Search));

        // Format filter
        if (!string.IsNullOrEmpty(request.Format))
        {
            var format = Enum.Parse<BookFormat>(request.Format);
            query = query.Where(b => b.Format == format);
        }

        // Total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Pagination
        var books = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => new BookDto(
                b.Id,
                b.Title,
                b.Description,
                b.Price,
                b.Format.ToString(),
                b.Status.ToString(),
                b.CoverImageUrl,
                b.TotalPages,
                b.Language,
                b.GradeLevel
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetBooksResponse(books, totalCount, request.Page, request.PageSize, totalPages);
    }
}