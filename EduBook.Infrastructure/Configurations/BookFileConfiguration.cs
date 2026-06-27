using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduBook.Infrastructure.Configurations;

public class BookFileConfiguration : IEntityTypeConfiguration<BookFile>
{
    public void Configure(EntityTypeBuilder<BookFile> builder)
    {
        builder.ToTable("book_files");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.StorageKey)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(x => x.FileName)
               .IsRequired()
               .HasMaxLength(255);

        builder.HasOne(x => x.Book)
               .WithMany(x => x.BookFiles)
               .HasForeignKey(x => x.BookId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}