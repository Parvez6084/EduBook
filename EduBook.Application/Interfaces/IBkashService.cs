namespace EduBook.Application.Interfaces;

public interface IBkashService
{
    Task<string> GetTokenAsync();
    Task<BkashPaymentResponse> CreatePaymentAsync(decimal amount, string merchantInvoiceNumber);
    Task<BkashPaymentResponse> ExecutePaymentAsync(string paymentId);
    Task<BkashQueryResponse> QueryPaymentAsync(string paymentId);
}

public record BkashPaymentResponse(
    string PaymentId,
    string PaymentUrl,
    string Status,
    string? TransactionId
);

public record BkashQueryResponse(
    string PaymentId,
    string TransactionId,
    string Status,
    decimal Amount
);