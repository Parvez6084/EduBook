using MediatR;

namespace EduBook.Application.Features.Authors.Commands;

public record UpdateAuthorCommand(
    Guid Id,
    string FullName,
    string? Bio,
    string? ImageUrl
) : IRequest<UpdateAuthorResponse>;

public record UpdateAuthorResponse(
    Guid AuthorId,
    string FullName
);