using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Enums;
using EduBook.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduBook.API.Controllers.V1;

[ApiController]
[Route("api/v1/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IBkashService _bkashService;
    private readonly AppDbContext _context;

    public PaymentsController(IBkashService bkashService, AppDbContext context)
    {
        _bkashService = bkashService;
        _context = context;
    }

    [HttpPost("bkash/initiate")]
    [Authorize]
    public async Task<IActionResult> InitiateBkashPayment([FromBody] InitiatePaymentRequest request)
    {
        var transaction = await _context.PaymentTransactions
            .FirstOrDefaultAsync(t => t.Id == request.TransactionId);

        if (transaction == null)
            return NotFound(ApiResponse<string>.Fail("Transaction not found", 404));

        var payment = await _bkashService.CreatePaymentAsync(
            transaction.Amount,
            transaction.Id.ToString());

        transaction.GatewayTransactionId = payment.PaymentId;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<BkashPaymentResponse>.Success(payment, "Payment initiated"));
    }

    [HttpGet("bkash/callback")]
    public async Task<IActionResult> BkashCallback(
        [FromQuery] string paymentID,
        [FromQuery] string status)
    {
        if (status != "success")
            return Redirect("https://yourdomain.com/payment/failed");

        var executeResult = await _bkashService.ExecutePaymentAsync(paymentID);

        if (executeResult.Status == "0000")
        {
            var transaction = await _context.PaymentTransactions
                .FirstOrDefaultAsync(t => t.GatewayTransactionId == paymentID);

            if (transaction != null)
            {
                transaction.Status = PaymentStatus.Completed;
                transaction.GatewayResponse = executeResult.TransactionId;

                var purchase = await _context.Purchases
                    .FirstOrDefaultAsync(p => p.TransactionId == transaction.Id);

                if (purchase != null)
                    purchase.Status = PurchaseStatus.Completed;

                await _context.SaveChangesAsync();
            }

            return Redirect("https://yourdomain.com/payment/success");
        }

        return Redirect("https://yourdomain.com/payment/failed");
    }
}

public record InitiatePaymentRequest(Guid TransactionId);