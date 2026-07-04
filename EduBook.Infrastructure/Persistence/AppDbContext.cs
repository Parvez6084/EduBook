using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<OtpCode> OtpCodes { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Publisher> Publishers { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<BookAuthor> BookAuthors { get; set; } = null!;
    public DbSet<BookCategory> BookCategories { get; set; } = null!;
    public DbSet<BookFile> BookFiles { get; set; } = null!;
    public DbSet<Purchase> Purchases { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;
    public DbSet<ReadingProgress> ReadingProgresses { get; set; } = null!;
    public DbSet<Bookmark> Bookmarks { get; set; } = null!;
    public DbSet<Highlight> Highlights { get; set; } = null!;
    public DbSet<Note> Notes { get; set; } = null!;
    public DbSet<Notification> Notifications { get; } = null!;



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}