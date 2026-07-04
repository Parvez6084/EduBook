using MediatR;

namespace EduBook.Application.Features.Auth.Queries;

public record GetProfileQuery(Guid UserId) : IRequest<ProfileDto>;

public record ProfileDto(
    Guid Id,
    string FullName,
    string? Email,
    string? PhoneNumber,
    string? ProfileImageUrl,
    string Role,
    string Status,
    bool IsEmailVerified,
    bool IsPhoneVerified,
    DateTime? LastLoginAt,
    DateTime CreatedAt
);