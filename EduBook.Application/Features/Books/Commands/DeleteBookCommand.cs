using MediatR;

namespace EduBook.Application.Features.Books.Commands;

public record DeleteBookCommand(Guid Id) : IRequest<bool>;