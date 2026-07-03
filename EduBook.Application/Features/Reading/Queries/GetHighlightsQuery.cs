using MediatR;

namespace EduBook.Application.Features.Reading.Queries;

public record GetHighlightsQuery(
    Guid UserId,
    Guid BookId
) : IRequest<List<HighlightDto>>;

public record HighlightDto(
    Guid Id,
    int PageNumber,
    string SelectedText,
    string Color,
    string? Note,
    DateTime CreatedAt
);