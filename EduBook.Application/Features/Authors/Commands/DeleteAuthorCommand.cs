using MediatR;

namespace EduBook.Application.Features.Authors.Commands;

public record DeleteAuthorCommand(Guid Id) : IRequest<bool>;