using MediatR;

namespace EduBook.Application.Features.Reading.Queries;

public record GetSignedUrlQuery(
    Guid BookId,
    Guid UserId
) : IRequest<SignedUrlDto>;

public record SignedUrlDto(
    string SignedUrl,
    DateTime ExpiresAt
);