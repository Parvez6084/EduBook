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
public class SubscriptionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new CreateSubscriptionCommand(
            userId,
            request.Plan,
            request.Gateway,
            request.IdempotencyKey
        );

        var result = await _mediator.Send(command);
        return Ok(ApiResponse<CreateSubscriptionResponse>.Success(result, "Subscription created successfully", 201));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveSubscription()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _mediator.Send(new GetActiveSubscriptionQuery(userId));

        if (result == null)
            return Ok(ApiResponse<string>.Success("No active subscription", "No active subscription found"));

        return Ok(ApiResponse<SubscriptionDto>.Success(result, "Active subscription retrieved"));
    }

    [HttpDelete("{subscriptionId}")]
    public async Task<IActionResult> CancelSubscription(Guid subscriptionId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _mediator.Send(new CancelSubscriptionCommand(subscriptionId, userId));
        return Ok(ApiResponse<string>.Success("Subscription cancelled successfully", "Subscription cancelled"));
    }
}

public record CreateSubscriptionRequest(
    string Plan,
    string Gateway,
    string IdempotencyKey
);