namespace EduBook.Domain.Enums;

public enum PaymentGateway
{
    Bkash = 1,
    Nagad = 2,
    Rocket = 3,
    SSLCommerz = 4
}

public enum PaymentStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Cancelled = 4,
    Refunded = 5
}

public enum PurchaseStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Refunded = 4
}

public enum SubscriptionPlan
{
    Monthly = 1,
    Yearly = 2
}

public enum SubscriptionStatus
{
    Active = 1,
    Expired = 2,
    Cancelled = 3,
    PastDue = 4
}