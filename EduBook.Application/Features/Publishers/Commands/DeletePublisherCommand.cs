using MediatR;

namespace EduBook.Application.Features.Publishers.Commands;

public record DeletePublisherCommand(Guid Id) : IRequest<bool>;
