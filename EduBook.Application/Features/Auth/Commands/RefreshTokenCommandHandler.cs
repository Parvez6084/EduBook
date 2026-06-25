using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Auth.Commands;

public class RefreshTokenCommandHandler : BaseHandler, IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public RefreshTokenCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<RefreshTokenResponse> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var refreshToken = await Context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == request.RefreshToken, cancellationToken);

        if (refreshToken == null)
            throw new UnauthorizedException("Invalid refresh token");

        if (!refreshToken.IsActive)
            throw new UnauthorizedException("Refresh token has expired or been revoked");

        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.UpdatedAt = DateTime.UtcNow;

        var newAccessToken = JwtService.GenerateAccessToken(refreshToken.User);
        var newRefreshToken = JwtService.GenerateRefreshToken();

        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = refreshToken.UserId,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        Context.RefreshTokens.Add(newRefreshTokenEntity);
        await Context.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResponse(newAccessToken, newRefreshToken);
    }
}