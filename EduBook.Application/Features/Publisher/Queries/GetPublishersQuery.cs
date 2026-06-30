using MediatR;

namespace EduBook.Application.Features.Publishers.Queries;

public record GetPublishersQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null
) : IRequest<GetPublishersResponse>;

public record PublisherDto(
    Guid Id,
    string Name,
    string? Description,
    string? LogoUrl,
    string? Website
);

public record GetPublishersResponse(
    List<PublisherDto> Publishers,
    int TotalCount,
    int Page,
    int PageSize
);