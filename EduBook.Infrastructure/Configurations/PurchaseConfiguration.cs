using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduBook.Infrastructure.Configurations;

public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable("purchases");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PricePaid)
               .HasColumnType("decimal(10,2)");

        builder.HasOne(x => x.User)
               .WithMany(x => x.Purchases)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Book)
               .WithMany(x => x.Purchases)
               .HasForeignKey(x => x.BookId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.BookId }).IsUnique();
    }
}