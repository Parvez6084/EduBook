using MediatR;

namespace EduBook.Application.Features.Categories.Commands;

public record CreateCategoryCommand(
    string Name,
    string? Description,
    string? IconUrl,
    Guid? ParentCategoryId
) : IRequest<CreateCategoryResponse>;

public record CreateCategoryResponse(
    Guid CategoryId,
    string Name
);