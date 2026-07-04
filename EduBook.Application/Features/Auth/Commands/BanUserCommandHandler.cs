using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Enums;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Auth.Commands;

public class BanUserCommandHandler : BaseHandler, IRequestHandler<BanUserCommand, bool>
{
    public BanUserCommandHandler(IApplicationDbContext context) : base(context) { }

    public async Task<bool> Handle(BanUserCommand request, CancellationToken cancellationToken)
    {
        var user = await Context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId && u.DeletedAt == null, cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found");

        user.Status = request.Ban ? UserStatus.Banned : UserStatus.Active;
        user.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync(cancellationToken);

        return true;
    }
}