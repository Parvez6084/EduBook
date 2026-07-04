using MediatR;

namespace EduBook.Application.Features.Auth.Commands;

public record BanUserCommand(Guid UserId, bool Ban) : IRequest<bool>;