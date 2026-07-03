using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduBook.Infrastructure.Configurations;

public class ReadingProgressConfiguration : IEntityTypeConfiguration<ReadingProgress>
{
    public void Configure(EntityTypeBuilder<ReadingProgress> builder)
    {
        builder.ToTable("reading_progress");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.UserId, x.BookId }).IsUnique();

        builder.HasOne(x => x.User)
               .WithMany(x => x.ReadingProgresses)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Book)
               .WithMany(x => x.ReadingProgresses)
               .HasForeignKey(x => x.BookId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}