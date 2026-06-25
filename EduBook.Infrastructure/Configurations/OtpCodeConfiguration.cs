using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduBook.Infrastructure.Configurations;

public class OtpCodeConfiguration : IEntityTypeConfiguration<OtpCode>
{
    public void Configure(EntityTypeBuilder<OtpCode> builder)
    {
        builder.ToTable("otp_codes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Target)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(x => x.Code)
               .IsRequired()
               .HasMaxLength(10);
    }
}