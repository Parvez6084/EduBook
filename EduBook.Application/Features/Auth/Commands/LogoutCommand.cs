using MediatR;

namespace EduBook.Application.Features.Auth.Commands;

public record LogoutCommand(string RefreshToken) : IRequest<bool>;