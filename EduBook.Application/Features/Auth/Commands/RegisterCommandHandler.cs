using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Enums;
using MediatR;

namespace EduBook.Application.Features.Auth.Commands;

public class RegisterCommandHandler : BaseHandler, IRequestHandler<RegisterCommand, RegisterResponse>
{
    public RegisterCommandHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher) : base(context, jwtService, passwordHasher)
    {
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (request.Email != null && Context.Users.Any(u => u.Email == request.Email))
            throw new Exception("Email already exists");

        if (request.PhoneNumber != null && Context.Users.Any(u => u.PhoneNumber == request.PhoneNumber))
            throw new Exception("Phone number already exists");

        var passwordHash = PasswordHasher.Hash(request.Password);

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

        var accessToken = JwtService.GenerateAccessToken(user);
        var refreshToken = JwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        Context.RefreshTokens.Add(refreshTokenEntity);
        await Context.SaveChangesAsync(cancellationToken);

        return new RegisterResponse(user.Id, user.FullName, accessToken, refreshToken);
    }
}