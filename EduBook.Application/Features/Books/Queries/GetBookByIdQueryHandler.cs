using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Books.Queries;

public class GetBookByIdQueryHandler : BaseHandler, IRequestHandler<GetBookByIdQuery, BookDetailDto>
{
    public GetBookByIdQueryHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<BookDetailDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await Context.Books
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedAt == null, cancellationToken);

        if (book == null)
            throw new NotFoundException("Book not found");

        return new BookDetailDto(
            book.Id,
            book.Title,
            book.Description,
            book.ISBN,
            book.Price,
            book.TotalPages,
            book.Language,
            book.Format.ToString(),
            book.Status.ToString(),
            book.CoverImageUrl,
            book.GradeLevel,
            book.PublishedDate,
            book.BookAuthors.Select(ba => ba.Author.FullName).ToList(),
            book.BookCategories.Select(bc => bc.Category.Name).ToList()
        );
    }
}