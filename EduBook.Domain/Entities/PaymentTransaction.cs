using EduBook.Domain.Common;
using EduBook.Domain.Enums;

namespace EduBook.Domain.Entities;

public class PaymentTransaction : BaseEntity
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "BDT";
    public PaymentGateway Gateway { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string? GatewayTransactionId { get; set; }
    public string? GatewayResponse { get; set; }
    public string IdempotencyKey { get; set; } = Guid.NewGuid().ToString();

    // Navigation
    public User User { get; set; } = null!;
}