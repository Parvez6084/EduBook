using MediatR;

namespace EduBook.Application.Features.Reading.Queries;

public record GetReadingProgressQuery(
    Guid UserId,
    Guid BookId
) : IRequest<ReadingProgressDto?>;

public record ReadingProgressDto(
    Guid Id,
    int CurrentPage,
    int TotalPages,
    double ProgressPercentage,
    DateTime LastReadAt
);