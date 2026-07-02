using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Enums;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace EduBook.Application.Features.Auth.Commands;

public class RegisterCommandHandler : BaseHandler, IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher,
        ILogger<RegisterCommandHandler> logger) : base(context)
    {
        _logger = logger;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting user registration for: {FullName}", request.FullName);

        // Check for existing user by email or phone in a single query
        var existingUser = await Context.Users
            .FirstOrDefaultAsync(u =>
                (request.Email != null && u.Email == request.Email) ||
                (request.PhoneNumber != null && u.PhoneNumber == request.PhoneNumber),
                cancellationToken);

        if (existingUser != null)
        {
            if (request.Email != null && existingUser.Email == request.Email)
            {
                _logger.LogWarning("Registration failed: Email already exists - {Email}", request.Email);
                throw new DuplicateEmailException(request.Email);
            }

            if (request.PhoneNumber != null && existingUser.PhoneNumber == request.PhoneNumber)
            {
                _logger.LogWarning("Registration failed: Phone already exists - {Phone}", request.PhoneNumber);
                throw new DuplicatePhoneException(request.PhoneNumber);
            }
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = passwordHash,
            Role = Enum.Parse<UserRole>(request.Role),
            Status = UserStatus.PendingVerification
        };

        Context.Users.Add(user);
        await Context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User created successfully: {UserId}", user.Id);

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        Context.RefreshTokens.Add(refreshTokenEntity);
        await Context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Registration completed successfully for user: {UserId}", user.Id);

        return new RegisterResponse(user.Id, user.FullName, accessToken, refreshToken);
    }
}