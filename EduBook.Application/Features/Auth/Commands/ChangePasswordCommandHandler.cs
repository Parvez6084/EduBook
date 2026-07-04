using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Auth.Commands;

public class ChangePasswordCommandHandler : BaseHandler, IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher) : base(context)
    {
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await Context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId && u.DeletedAt == null, cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found");

        if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash!))
            throw new UnauthorizedException("Current password is incorrect");

        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync(cancellationToken);

        return true;
    }
}