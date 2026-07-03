using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduBook.Infrastructure.Configurations;

public class HighlightConfiguration : IEntityTypeConfiguration<Highlight>
{
    public void Configure(EntityTypeBuilder<Highlight> builder)
    {
        builder.ToTable("highlights");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SelectedText)
               .IsRequired()
               .HasMaxLength(2000);

        builder.Property(x => x.Color)
               .HasMaxLength(20)
               .HasDefaultValue("#FFFF00");

        builder.Property(x => x.Note)
               .HasMaxLength(500);

        builder.HasOne(x => x.User)
               .WithMany(x => x.Highlights)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Book)
               .WithMany(x => x.Highlights)
               .HasForeignKey(x => x.BookId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}