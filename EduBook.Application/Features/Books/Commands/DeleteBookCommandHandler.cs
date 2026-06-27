using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Books.Commands;

public class DeleteBookCommandHandler : BaseHandler, IRequestHandler<DeleteBookCommand, bool>
{
    public DeleteBookCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await Context.Books
            .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedAt == null, cancellationToken);

        if (book == null)
            throw new NotFoundException("Book not found");

        // Soft delete
        book.DeletedAt = DateTime.UtcNow;
        book.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync(cancellationToken);

        return true;
    }
}