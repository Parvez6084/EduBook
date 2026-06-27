using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Books.Commands;

public class UpdateBookCommandHandler : BaseHandler, IRequestHandler<UpdateBookCommand, UpdateBookResponse>
{
    public UpdateBookCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<UpdateBookResponse> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await Context.Books
            .Include(b => b.BookAuthors)
            .Include(b => b.BookCategories)
            .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedAt == null, cancellationToken);

        if (book == null)
            throw new NotFoundException("Book not found");

        // Update properties
        book.Title = request.Title;
        book.Description = request.Description;
        book.ISBN = request.ISBN;
        book.Price = request.Price;
        book.TotalPages = request.TotalPages;
        book.Language = request.Language;
        book.GradeLevel = request.GradeLevel;
        book.UpdatedAt = DateTime.UtcNow;

        // Update authors
        Context.BookAuthors.RemoveRange(book.BookAuthors);
        foreach (var authorId in request.AuthorIds)
        {
            Context.BookAuthors.Add(new BookAuthor
            {
                BookId = book.Id,
                AuthorId = authorId
            });
        }

        // Update categories
        Context.BookCategories.RemoveRange(book.BookCategories);
        foreach (var categoryId in request.CategoryIds)
        {
            Context.BookCategories.Add(new BookCategory
            {
                BookId = book.Id,
                CategoryId = categoryId
            });
        }

        await Context.SaveChangesAsync(cancellationToken);

        return new UpdateBookResponse(book.Id, book.Title, book.Status.ToString());
    }
}