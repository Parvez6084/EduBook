using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Auth.Commands;

public class LoginCommandHandler : BaseHandler, IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context)
    {
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {

        var user = await Context.Users
            .FirstOrDefaultAsync(u =>
                u.Email == request.EmailOrPhone ||
                u.PhoneNumber == request.EmailOrPhone,
                cancellationToken);

        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash!))
        {
            throw new InvalidCredentialsException();
        }

        if (user.Status == Domain.Enums.UserStatus.Banned)
        {
            throw new UnauthorizedException("Your account has been banned");
        }

        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        await Context.SaveChangesAsync(cancellationToken);

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var oldTokens = Context.RefreshTokens
            .Where(r => r.UserId == user.Id && r.RevokedAt == null);

        await foreach (var token in oldTokens.AsAsyncEnumerable())
        {
            token.RevokedAt = DateTime.UtcNow;
        }

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        Context.RefreshTokens.Add(refreshTokenEntity);
        await Context.SaveChangesAsync(cancellationToken);

        return new LoginResponse(user.Id, user.FullName, user.Role.ToString(), accessToken, refreshToken);
    }
}