using EduBook.Application.Common;
using EduBook.Application.Features.Books.Commands;
using EduBook.Application.Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduBook.API.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? category = null,
        [FromQuery] string? format = null)
    {
        var query = new GetBooksQuery(page, pageSize, search, category, format);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<GetBooksResponse>.Success(result, "Books retrieved successfully"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(Guid id)
    {
        var result = await _mediator.Send(new GetBookByIdQuery(id));
        return Ok(ApiResponse<BookDetailDto>.Success(result, "Book retrieved successfully"));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<CreateBookResponse>.Success(result, "Book created successfully", 201));
    }
}