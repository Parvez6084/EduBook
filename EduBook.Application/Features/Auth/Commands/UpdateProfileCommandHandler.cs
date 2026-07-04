using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Auth.Commands;

public class UpdateProfileCommandHandler : BaseHandler, IRequestHandler<UpdateProfileCommand, UpdateProfileResponse>
{
    public UpdateProfileCommandHandler(IApplicationDbContext context) : base(context) { }

    public async Task<UpdateProfileResponse> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await Context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId && u.DeletedAt == null, cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found");

        user.FullName = request.FullName;
        user.ProfileImageUrl = request.ProfileImageUrl;
        user.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync(cancellationToken);

        return new UpdateProfileResponse(
            user.Id,
            user.FullName,
            user.Email,
            user.PhoneNumber,
            user.ProfileImageUrl,
            user.Role.ToString()
        );
    }
}