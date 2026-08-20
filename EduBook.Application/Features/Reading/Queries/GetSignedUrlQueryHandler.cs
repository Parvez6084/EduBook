using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Enums;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Reading.Queries;

public class GetSignedUrlQueryHandler : BaseHandler, IRequestHandler<GetSignedUrlQuery, SignedUrlDto>
{
    private readonly IStorageService _storageService;

    public GetSignedUrlQueryHandler(
        IApplicationDbContext context,
        IStorageService storageService) : base(context)
    {
        _storageService = storageService;
    }

    public async Task<SignedUrlDto> Handle(GetSignedUrlQuery request, CancellationToken cancellationToken)
    {
        // Check user has access
        var hasPurchased = await Context.Purchases
            .AnyAsync(p =>
                p.UserId == request.UserId &&
                p.BookId == request.BookId &&
                p.Status == PurchaseStatus.Completed,
                cancellationToken);

        var hasSubscription = await Context.Subscriptions
            .AnyAsync(s =>
                s.UserId == request.UserId &&
                s.Status == SubscriptionStatus.Active &&
                s.EndDate > DateTime.UtcNow,
                cancellationToken);

        if (!hasPurchased && !hasSubscription)
            throw new UnauthorizedException("You do not have access to this book");

        // Get book file
        var bookFile = await Context.BookFiles
            .FirstOrDefaultAsync(f => f.BookId == request.BookId, cancellationToken);

        if (bookFile == null)
            throw new NotFoundException("Book file not found");

        // Generate signed URL (valid for 15 minutes)
        var signedUrl = await _storageService.GenerateSignedUrlAsync(bookFile.StorageKey, 15);

        return new SignedUrlDto(signedUrl, DateTime.UtcNow.AddMinutes(15));
    }
}