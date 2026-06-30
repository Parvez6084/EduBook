using MediatR;

namespace EduBook.Application.Features.Categories.Commands;

public record DeleteCategoryCommand(Guid Id) : IRequest<bool>;