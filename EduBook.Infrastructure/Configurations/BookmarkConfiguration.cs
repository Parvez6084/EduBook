using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduBook.Infrastructure.Configurations;

public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(EntityTypeBuilder<Bookmark> builder)
    {
        builder.ToTable("bookmarks");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Note)
               .HasMaxLength(500);

        builder.Property(x => x.ChapterTitle)
               .HasMaxLength(200);

        builder.HasOne(x => x.User)
               .WithMany(x => x.Bookmarks)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Book)
               .WithMany(x => x.Bookmarks)
               .HasForeignKey(x => x.BookId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}