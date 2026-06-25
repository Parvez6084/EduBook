using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Find user by email or phone
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Email == request.EmailOrPhone ||
                u.PhoneNumber == request.EmailOrPhone,
                cancellationToken);

        if (user == null)
            throw new UnauthorizedException("Invalid credentials");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash!))
            throw new UnauthorizedException("Invalid credentials");

        if (user.Status == Domain.Enums.UserStatus.Banned)
            throw new UnauthorizedException("Your account has been banned");

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        // Generate tokens
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Revoke old refresh tokens
        var oldTokens = _context.RefreshTokens
            .Where(r => r.UserId == user.Id && r.RevokedAt == null);

        await foreach (var token in oldTokens.AsAsyncEnumerable())
        {
            token.RevokedAt = DateTime.UtcNow;
        }

        // Save new refresh token
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResponse(
            user.Id,
            user.FullName,
            user.Role.ToString(),
            accessToken,
            refreshToken
        );
    }
}