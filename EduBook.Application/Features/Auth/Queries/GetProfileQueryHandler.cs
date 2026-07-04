using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Auth.Queries;

public class GetProfileQueryHandler : BaseHandler, IRequestHandler<GetProfileQuery, ProfileDto>
{
    public GetProfileQueryHandler(IApplicationDbContext context) : base(context) { }

    public async Task<ProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await Context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId && u.DeletedAt == null, cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found");

        return new ProfileDto(
            user.Id,
            user.FullName,
            user.Email,
            user.PhoneNumber,
            user.ProfileImageUrl,
            user.Role.ToString(),
            user.Status.ToString(),
            user.IsEmailVerified,
            user.IsPhoneVerified,
            user.LastLoginAt,
            user.CreatedAt
        );
    }
}