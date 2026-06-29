using MediatR;

namespace EduBook.Application.Features.Authors.Queries;

public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorDto>;