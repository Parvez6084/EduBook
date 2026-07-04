using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Books.Queries;

public class SearchBooksQueryHandler : BaseHandler, IRequestHandler<SearchBooksQuery, SearchBooksResponse>
{
    public SearchBooksQueryHandler(IApplicationDbContext context) : base(context) { }

    public async Task<SearchBooksResponse> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
    {
        var query = Context.Books
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
            .Where(b => b.DeletedAt == null && b.Status == BookStatus.Published)
            .AsQueryable();

        // Full text search
        if (!string.IsNullOrEmpty(request.SearchTerm))
            query = query.Where(b =>
                b.Title.Contains(request.SearchTerm) ||
                (b.Description != null && b.Description.Contains(request.SearchTerm)) ||
                (b.ISBN != null && b.ISBN.Contains(request.SearchTerm)));

        // Format filter
        if (!string.IsNullOrEmpty(request.Format))
        {
            var format = Enum.Parse<BookFormat>(request.Format);
            query = query.Where(b => b.Format == format);
        }

        // Price range filter
        if (request.MinPrice.HasValue)
            query = query.Where(b => b.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(b => b.Price <= request.MaxPrice.Value);

        // Grade level filter
        if (request.GradeLevel.HasValue)
            query = query.Where(b => b.GradeLevel == request.GradeLevel.Value);

        // Category filter
        if (request.CategoryId.HasValue)
            query = query.Where(b => b.BookCategories
                .Any(bc => bc.CategoryId == request.CategoryId.Value));

        // Author filter
        if (request.AuthorId.HasValue)
            query = query.Where(b => b.BookAuthors
                .Any(ba => ba.AuthorId == request.AuthorId.Value));

        // Publisher filter
        if (request.PublisherId.HasValue)
            query = query.Where(b => b.PublisherId == request.PublisherId.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var books = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => new BookSearchDto(
                b.Id,
                b.Title,
                b.Description,
                b.Price,
                b.Format.ToString(),
                b.Status.ToString(),
                b.CoverImageUrl,
                b.TotalPages,
                b.Language,
                b.GradeLevel,
                b.BookAuthors.Select(ba => ba.Author.FullName).ToList(),
                b.BookCategories.Select(bc => bc.Category.Name).ToList()
            ))
            .ToListAsync(cancellationToken);

        return new SearchBooksResponse(
            books,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages,
            request.SearchTerm
        );
    }
}