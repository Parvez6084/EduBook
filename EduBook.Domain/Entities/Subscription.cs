using EduBook.Domain.Common;
using EduBook.Domain.Enums;

namespace EduBook.Domain.Entities;

public class Subscription : BaseEntity
{
    public Guid UserId { get; set; }
    public SubscriptionPlan Plan { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool AutoRenew { get; set; } = true;
    public decimal PricePaid { get; set; }
    public Guid? TransactionId { get; set; }
    public bool IsActive => Status == SubscriptionStatus.Active && DateTime.UtcNow <= EndDate;

    // Navigation
    public User User { get; set; } = null!;
    public PaymentTransaction? Transaction { get; set; }
}