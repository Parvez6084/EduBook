using MediatR;

namespace EduBook.Application.Features.Categories.Queries;

public record GetCategoriesQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null
) : IRequest<GetCategoriesResponse>;

public record CategoryDto(
    Guid Id,
    string Name,
    string? Description,
    string? IconUrl,
    Guid? ParentCategoryId
);

public record GetCategoriesResponse(
    List<CategoryDto> Categories,
    int TotalCount,
    int Page,
    int PageSize
);