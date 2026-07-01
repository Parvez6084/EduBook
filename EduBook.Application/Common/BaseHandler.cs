using EduBook.Application.Interfaces;

namespace EduBook.Application.Common;

public abstract class BaseHandler
{
    protected readonly IApplicationDbContext Context;

    protected BaseHandler(IApplicationDbContext context)
    {
        Context = context;
    }
}
