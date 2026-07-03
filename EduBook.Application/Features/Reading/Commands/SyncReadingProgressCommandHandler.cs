using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Reading.Commands;

public class SyncReadingProgressCommandHandler : BaseHandler, IRequestHandler<SyncReadingProgressCommand, SyncReadingProgressResponse>
{
    public SyncReadingProgressCommandHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<SyncReadingProgressResponse> Handle(SyncReadingProgressCommand request, CancellationToken cancellationToken)
    {
        // Check book exists
        var book = await Context.Books
            .FirstOrDefaultAsync(b => b.Id == request.BookId && b.DeletedAt == null, cancellationToken);

        if (book == null)
            throw new NotFoundException("Book not found");

        // Check user has access
        var hasPurchased = await Context.Purchases
            .AnyAsync(p => p.UserId == request.UserId &&
                          p.BookId == request.BookId &&
                          p.Status == Domain.Enums.PurchaseStatus.Completed,
                          cancellationToken);

        var hasSubscription = await Context.Subscriptions
            .AnyAsync(s => s.UserId == request.UserId &&
                          s.Status == Domain.Enums.SubscriptionStatus.Active &&
                          s.EndDate > DateTime.UtcNow,
                          cancellationToken);

        if (!hasPurchased && !hasSubscription)
            throw new UnauthorizedException("You do not have access to this book");

        // Upsert reading progress
        var progress = await Context.ReadingProgresses
            .FirstOrDefaultAsync(r => r.UserId == request.UserId &&
                                     r.BookId == request.BookId,
                                     cancellationToken);

        if (progress == null)
        {
            progress = new ReadingProgress
            {
                UserId = request.UserId,
                BookId = request.BookId,
                CurrentPage = request.CurrentPage,
                TotalPages = request.TotalPages,
                LastReadAt = DateTime.UtcNow
            };
            Context.ReadingProgresses.Add(progress);
        }
        else
        {
            progress.CurrentPage = request.CurrentPage;
            progress.TotalPages = request.TotalPages;
            progress.LastReadAt = DateTime.UtcNow;
            progress.UpdatedAt = DateTime.UtcNow;
        }

        await Context.SaveChangesAsync(cancellationToken);

        return new SyncReadingProgressResponse(
            progress.Id,
            progress.CurrentPage,
            progress.TotalPages,
            progress.ProgressPercentage,
            progress.LastReadAt
        );
    }
}