using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Enums;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Purchases.Commands;

public class CreatePurchaseCommandHandler : BaseHandler, IRequestHandler<CreatePurchaseCommand, CreatePurchaseResponse>
{
    public CreatePurchaseCommandHandler(
        IApplicationDbContext context) : base(context)
    {
    }

    public async Task<CreatePurchaseResponse> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        // Get book
        var book = await Context.Books
            .FirstOrDefaultAsync(b => b.Id == request.BookId && b.DeletedAt == null, cancellationToken);

        if (book == null)
            throw new NotFoundException("Book not found");

        // Check if already purchased
        var existingPurchase = await Context.Purchases
            .FirstOrDefaultAsync(p => p.BookId == request.BookId, cancellationToken);

        if (existingPurchase != null && existingPurchase.Status == PurchaseStatus.Completed)
            throw new ValidationException("You have already purchased this book");

        // Create payment transaction
        var transaction = new PaymentTransaction
        {
            UserId = request.UserId,
            Amount = book.Price,
            Gateway = Enum.Parse<PaymentGateway>(request.Gateway),
            Status = PaymentStatus.Pending,
            IdempotencyKey = request.IdempotencyKey
        };

        Context.PaymentTransactions.Add(transaction);
        await Context.SaveChangesAsync(cancellationToken);

        // Create purchase
        var purchase = new Purchase
        {
            UserId = request.UserId,
            BookId = request.BookId,
            PricePaid = book.Price,
            Status = PurchaseStatus.Pending,
            TransactionId = transaction.Id
        };

        Context.Purchases.Add(purchase);
        await Context.SaveChangesAsync(cancellationToken);

        return new CreatePurchaseResponse(
            purchase.Id,
            transaction.Id,
            book.Price,
            purchase.Status.ToString(),
            $"https://payment.edubook.com/pay/{transaction.Id}"
        );
    }
}