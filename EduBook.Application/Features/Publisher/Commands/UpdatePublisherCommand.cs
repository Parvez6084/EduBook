using MediatR;

namespace EduBook.Application.Features.Publishers.Commands;

public record UpdatePublisherCommand(
    Guid Id,
    string Name,
    string? Description,
    string? LogoUrl,
    string? Website
) : IRequest<UpdatePublisherResponse>;

public record UpdatePublisherResponse(
    Guid PublisherId,
    string Name
);