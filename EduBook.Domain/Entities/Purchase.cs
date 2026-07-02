using EduBook.Domain.Common;
using EduBook.Domain.Enums;

namespace EduBook.Domain.Entities;

public class Purchase : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public decimal PricePaid { get; set; }
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;
    public Guid? TransactionId { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Book Book { get; set; } = null!;
    public PaymentTransaction? Transaction { get; set; }
}