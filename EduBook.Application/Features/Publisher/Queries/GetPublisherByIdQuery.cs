using MediatR;

namespace EduBook.Application.Features.Publishers.Queries;

public record GetPublisherByIdQuery(Guid Id) : IRequest<PublisherDto>;