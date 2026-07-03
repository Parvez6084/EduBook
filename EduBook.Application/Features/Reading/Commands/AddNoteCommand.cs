using MediatR;

namespace EduBook.Application.Features.Reading.Commands;

public record AddNoteCommand(
    Guid UserId,
    Guid BookId,
    int PageNumber,
    string Content
) : IRequest<AddNoteResponse>;

public record AddNoteResponse(
    Guid Id,
    int PageNumber,
    string Content
);