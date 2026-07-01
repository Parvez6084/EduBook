using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduBook.Application.Features.Auth.Commands;

public class LoginCommandHandler : BaseHandler, IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher,
        ILogger<LoginCommandHandler> logger) : base(context)
    {
        _logger = logger;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for: {EmailOrPhone}", request.EmailOrPhone);

        var user = await Context.Users
            .FirstOrDefaultAsync(u =>
                u.Email == request.EmailOrPhone ||
                u.PhoneNumber == request.EmailOrPhone,
                cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found - {EmailOrPhone}", request.EmailOrPhone);
            throw new InvalidCredentialsException();
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash!))
        {
            _logger.LogWarning("Login failed: Invalid password for user - {UserId}", user.Id);
            throw new InvalidCredentialsException();
        }

        if (user.Status == Domain.Enums.UserStatus.Banned)
        {
            _logger.LogWarning("Login failed: User banned - {UserId}", user.Id);
            throw new UnauthorizedException("Your account has been banned");
        }

        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        await Context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Login successful for user: {UserId}", user.Id);

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