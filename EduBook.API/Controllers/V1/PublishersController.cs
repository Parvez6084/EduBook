using EduBook.Application.Common;
using EduBook.Application.Features.Publishers.Commands;
using EduBook.Application.Features.Publishers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduBook.API.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class PublishersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PublishersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPublishers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(new GetPublishersQuery(page, pageSize, search));
        return Ok(ApiResponse<GetPublishersResponse>.Success(result, "Publishers retrieved successfully"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPublisherById(Guid id)
    {
        var result = await _mediator.Send(new GetPublisherByIdQuery(id));
        return Ok(ApiResponse<PublisherDto>.Success(result, "Publisher retrieved successfully"));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> CreatePublisher([FromBody] CreatePublisherCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<CreatePublisherResponse>.Success(result, "Publisher created successfully", 201));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> UpdatePublisher(Guid id, [FromBody] UpdatePublisherCommand command)
    {
        var updatedCommand = command with { Id = id };
        var result = await _mediator.Send(updatedCommand);
        return Ok(ApiResponse<UpdatePublisherResponse>.Success(result, "Publisher updated successfully"));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> DeletePublisher(Guid id)
    {
        await _mediator.Send(new DeletePublisherCommand(id));
        return Ok(ApiResponse<string>.Success("Publisher deleted successfully", "Publisher deleted successfully"));
    }
}