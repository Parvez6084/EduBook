using MediatR;

namespace EduBook.Application.Features.Purchases.Commands;

public record CancelSubscriptionCommand(
    Guid SubscriptionId,
    Guid UserId
) : IRequest<bool>;