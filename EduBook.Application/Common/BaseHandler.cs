using EduBook.Application.Interfaces;

namespace EduBook.Application.Common;

public abstract class BaseHandler
{
    protected readonly IApplicationDbContext Context;
    protected readonly IJwtService JwtService;
    protected readonly IPasswordHasher PasswordHasher;

    protected BaseHandler(
        IApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher)
    {
        Context = context;
        JwtService = jwtService;
        PasswordHasher = passwordHasher;
    }
}