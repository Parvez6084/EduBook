using MediatR;

namespace EduBook.Application.Features.Auth.Commands;

public record RegisterCommand(
    string FullName,
    string? Email,
    string? PhoneNumber,
    string Password,
    string Role
) : IRequest<RegisterResponse>;

public record RegisterResponse(
    Guid UserId,
    string FullName,
    string AccessToken,
    string RefreshToken
);