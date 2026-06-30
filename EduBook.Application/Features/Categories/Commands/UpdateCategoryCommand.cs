using MediatR;

namespace EduBook.Application.Features.Categories.Commands;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string? Description,
    string? IconUrl
) : IRequest<UpdateCategoryResponse>;

public record UpdateCategoryResponse(
    Guid CategoryId,
    string Name
);