using MediatR;

namespace EduBook.Application.Features.Authors.Commands;

public record CreateAuthorCommand(
    string FullName,
    string? Bio,
    string? ImageUrl
) : IRequest<CreateAuthorResponse>;

public record CreateAuthorResponse(
    Guid AuthorId,
    string FullName
);
