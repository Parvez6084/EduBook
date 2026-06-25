using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Enums;
using MediatR;

namespace EduBook.Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (request.Email != null && _context.Users.Any(u => u.Email == request.Email))
            throw new Exception("Email already exists");

        if (request.PhoneNumber != null && _context.Users.Any(u => u.PhoneNumber == request.PhoneNumber))
            throw new Exception("Phone number already exists");

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

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterResponse(user.Id, user.FullName, accessToken, refreshToken);
    }
}