using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Enums;
using EduBook.Domain.Exceptions;
using MediatR;

namespace EduBook.Application.Features.Books.Commands;

public class CreateBookCommandHandler : BaseHandler, IRequestHandler<CreateBookCommand, CreateBookResponse>
{
    public CreateBookCommandHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<CreateBookResponse> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        // Create book
        var book = new Book
        {
            Title = request.Title,
            Description = request.Description,
            ISBN = request.ISBN,
            Price = request.Price,
            TotalPages = request.TotalPages,
            Language = request.Language,
            Format = Enum.Parse<BookFormat>(request.Format),
            GradeLevel = request.GradeLevel,
            PublisherId = request.PublisherId,
            Status = BookStatus.Draft
        };

        Context.Books.Add(book);
        await Context.SaveChangesAsync(cancellationToken);

        // Add authors
        foreach (var authorId in request.AuthorIds)
        {
            Context.BookAuthors.Add(new BookAuthor
            {
                BookId = book.Id,
                AuthorId = authorId
            });
        }

        // Add categories
        foreach (var categoryId in request.CategoryIds)
        {
            Context.BookCategories.Add(new BookCategory
            {
                BookId = book.Id,
                CategoryId = categoryId
            });
        }

        await Context.SaveChangesAsync(cancellationToken);

        return new CreateBookResponse(book.Id, book.Title, book.Status.ToString());
    }
}