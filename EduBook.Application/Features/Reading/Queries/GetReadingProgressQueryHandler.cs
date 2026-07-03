using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Reading.Queries;

public class GetReadingProgressQueryHandler : BaseHandler, IRequestHandler<GetReadingProgressQuery, ReadingProgressDto?>
{
    public GetReadingProgressQueryHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<ReadingProgressDto?> Handle(GetReadingProgressQuery request, CancellationToken cancellationToken)
    {
        var progress = await Context.ReadingProgresses
            .FirstOrDefaultAsync(r => r.UserId == request.UserId &&
                                     r.BookId == request.BookId,
                                     cancellationToken);

        if (progress == null)
            return null;

        return new ReadingProgressDto(
            progress.Id,
            progress.CurrentPage,
            progress.TotalPages,
            progress.ProgressPercentage,
            progress.LastReadAt
        );
    }
}