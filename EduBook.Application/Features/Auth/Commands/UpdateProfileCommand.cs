using MediatR;

namespace EduBook.Application.Features.Auth.Commands;

public record UpdateProfileCommand(
    Guid UserId,
    string FullName,
    string? ProfileImageUrl
) : IRequest<UpdateProfileResponse>;

public record UpdateProfileResponse(
    Guid UserId,
    string FullName,
    string? Email,
    string? PhoneNumber,
    string? ProfileImageUrl,
    string Role
);