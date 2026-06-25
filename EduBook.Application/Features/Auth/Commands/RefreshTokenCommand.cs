using MediatR;

namespace EduBook.Application.Features.Auth.Commands;

public record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResponse>;

public record RefreshTokenResponse( string AccessToken,string RefreshToken);