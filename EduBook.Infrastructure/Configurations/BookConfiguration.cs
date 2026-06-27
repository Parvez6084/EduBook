using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduBook.Infrastructure.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(x => x.Description)
               .HasMaxLength(2000);

        builder.Property(x => x.ISBN)
               .HasMaxLength(20);

        builder.Property(x => x.CoverImageUrl)
               .HasMaxLength(500);

        builder.Property(x => x.Language)
               .HasMaxLength(50)
               .HasDefaultValue("Bengali");

        builder.Property(x => x.Price)
               .HasColumnType("decimal(10,2)");

        builder.HasOne(x => x.Publisher)
               .WithMany(x => x.Books)
               .HasForeignKey(x => x.PublisherId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.Status);
    }
}