using EduBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<OtpCode> OtpCodes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}