using MediatR;

namespace EduBook.Application.Features.Auth.Commands;

public record LoginCommand(
    string EmailOrPhone,
    string Password
) : IRequest<LoginResponse>;

public record LoginResponse(
    Guid UserId,
    string FullName,
    string Role,
    string AccessToken,
    string RefreshToken
);