using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduBook.Infrastructure.Configurations;

public class BookCategoryConfiguration : IEntityTypeConfiguration<BookCategory>
{
    public void Configure(EntityTypeBuilder<BookCategory> builder)
    {
        builder.ToTable("book_categories");
        builder.HasKey(x => new { x.BookId, x.CategoryId });

        builder.HasOne(x => x.Book)
               .WithMany(x => x.BookCategories)
               .HasForeignKey(x => x.BookId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category)
               .WithMany(x => x.BookCategories)
               .HasForeignKey(x => x.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}