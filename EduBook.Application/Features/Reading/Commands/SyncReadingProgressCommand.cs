using MediatR;

namespace EduBook.Application.Features.Reading.Commands;

public record SyncReadingProgressCommand(
    Guid UserId,
    Guid BookId,
    int CurrentPage,
    int TotalPages
) : IRequest<SyncReadingProgressResponse>;

public record SyncReadingProgressResponse(
    Guid Id,
    int CurrentPage,
    int TotalPages,
    double ProgressPercentage,
    DateTime LastReadAt
);