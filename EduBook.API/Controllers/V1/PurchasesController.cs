using EduBook.Application.Common;
using EduBook.Application.Features.Purchases.Commands;
using EduBook.Application.Features.Purchases.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduBook.API.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class PurchasesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchasesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public record CreatePurchaseRequest(Guid BookId, string Gateway, string IdempotencyKey);

    [HttpPost]
    public async Task<IActionResult> CreatePurchase([FromBody] CreatePurchaseRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new CreatePurchaseCommand(
            userId,
            request.BookId,
            request.Gateway,
            request.IdempotencyKey
        );

        var result = await _mediator.Send(command);
        return Ok(ApiResponse<CreatePurchaseResponse>.Success(result, "Purchase initiated successfully", 201));
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetPurchaseHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _mediator.Send(new GetPurchaseHistoryQuery(userId, page, pageSize));
        return Ok(ApiResponse<GetPurchaseHistoryResponse>.Success(result, "Purchase history retrieved successfully"));
    }
}