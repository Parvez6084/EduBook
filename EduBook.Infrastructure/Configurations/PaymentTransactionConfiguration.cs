using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduBook.Infrastructure.Configurations;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.ToTable("payment_transactions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
               .HasColumnType("decimal(10,2)");

        builder.Property(x => x.Currency)
               .HasMaxLength(10)
               .HasDefaultValue("BDT");

        builder.Property(x => x.GatewayTransactionId)
               .HasMaxLength(200);

        builder.Property(x => x.IdempotencyKey)
               .HasMaxLength(100);

        builder.HasIndex(x => x.IdempotencyKey).IsUnique();

        builder.HasOne(x => x.User)
               .WithMany(x => x.PaymentTransactions)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}