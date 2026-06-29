using EduBook.Application.Common;
using EduBook.Application.Features.Authors.Commands;
using EduBook.Application.Features.Authors.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduBook.API.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAuthors(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(new GetAuthorsQuery(page, pageSize, search));
        return Ok(ApiResponse<GetAuthorsResponse>.Success(result, "Authors retrieved successfully"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthorById(Guid id)
    {
        var result = await _mediator.Send(new GetAuthorByIdQuery(id));
        return Ok(ApiResponse<AuthorDto>.Success(result, "Author retrieved successfully"));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<CreateAuthorResponse>.Success(result, "Author created successfully", 201));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthorCommand command)
    {
        var updatedCommand = command with { Id = id };
        var result = await _mediator.Send(updatedCommand);
        return Ok(ApiResponse<UpdateAuthorResponse>.Success(result, "Author updated successfully"));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> DeleteAuthor(Guid id)
    {
        await _mediator.Send(new DeleteAuthorCommand(id));
        return Ok(ApiResponse<string>.Success("Author deleted successfully", "Author deleted successfully"));
    }
}