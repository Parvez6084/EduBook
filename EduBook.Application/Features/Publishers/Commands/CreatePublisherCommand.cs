using MediatR;

namespace EduBook.Application.Features.Publishers.Commands;

public record CreatePublisherCommand(
    string Name,
    string? Description,
    string? LogoUrl,
    string? Website
) : IRequest<CreatePublisherResponse>;

public record CreatePublisherResponse(
    Guid PublisherId,
    string Name
);
