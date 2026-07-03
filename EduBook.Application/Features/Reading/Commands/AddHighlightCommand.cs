using MediatR;

namespace EduBook.Application.Features.Reading.Commands;

public record AddHighlightCommand(
    Guid UserId,
    Guid BookId,
    int PageNumber,
    string SelectedText,
    string Color,
    string? Note
) : IRequest<AddHighlightResponse>;

public record AddHighlightResponse(
    Guid Id,
    int PageNumber,
    string SelectedText,
    string Color,
    string? Note
);