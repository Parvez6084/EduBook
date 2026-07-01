using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using MediatR;

namespace EduBook.Application.Features.Categories.Commands;

public class CreateCategoryCommandHandler : BaseHandler, IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    public CreateCategoryCommandHandler(IApplicationDbContext context) : base(context)
    {
    }

    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
            IconUrl = request.IconUrl,
            ParentCategoryId = request.ParentCategoryId
        };

        Context.Categories.Add(category);
        await Context.SaveChangesAsync(cancellationToken);

        return new CreateCategoryResponse(category.Id, category.Name);
    }
}