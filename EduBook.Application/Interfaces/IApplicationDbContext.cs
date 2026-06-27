using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<OtpCode> OtpCodes { get; }
    DbSet<Book> Books { get; }
    DbSet<Author> Authors { get; }
    DbSet<Publisher> Publishers { get; }
    DbSet<Category> Categories { get; }
    DbSet<BookAuthor> BookAuthors { get; }
    DbSet<BookCategory> BookCategories { get; }
    DbSet<BookFile> BookFiles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}