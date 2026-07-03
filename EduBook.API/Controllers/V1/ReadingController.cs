using EduBook.Application.Common;
using EduBook.Application.Features.Reading.Commands;
using EduBook.Application.Features.Reading.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduBook.API.Controllers.V1;

[ApiController]
[Route("api/v1/reading")]
[Authorize]
public class ReadingController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReadingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{bookId}/progress")]
    public async Task<IActionResult> SyncProgress(Guid bookId, [FromBody] SyncProgressRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new SyncReadingProgressCommand(userId, bookId, request.CurrentPage, request.TotalPages);
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<SyncReadingProgressResponse>.Success(result, "Progress synced successfully"));
    }

    [HttpGet("{bookId}/progress")]
    public async Task<IActionResult> GetProgress(Guid bookId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _mediator.Send(new GetReadingProgressQuery(userId, bookId));
        if (result == null)
            return Ok(ApiResponse<string>.Success("No progress found", "No reading progress found"));
        return Ok(ApiResponse<ReadingProgressDto>.Success(result, "Progress retrieved successfully"));
    }

    [HttpPost("{bookId}/bookmarks")]
    public async Task<IActionResult> AddBookmark(Guid bookId, [FromBody] AddBookmarkRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new AddBookmarkCommand(userId, bookId, request.PageNumber, request.Note, request.ChapterTitle);
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<AddBookmarkResponse>.Success(result, "Bookmark added successfully", 201));
    }

    [HttpGet("{bookId}/bookmarks")]
    public async Task<IActionResult> GetBookmarks(Guid bookId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _mediator.Send(new GetBookmarksQuery(userId, bookId));
        return Ok(ApiResponse<List<BookmarkDto>>.Success(result, "Bookmarks retrieved successfully"));
    }

    [HttpPost("{bookId}/highlights")]
    public async Task<IActionResult> AddHighlight(Guid bookId, [FromBody] AddHighlightRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new AddHighlightCommand(userId, bookId, request.PageNumber, request.SelectedText, request.Color, request.Note);
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<AddHighlightResponse>.Success(result, "Highlight added successfully", 201));
    }

    [HttpGet("{bookId}/highlights")]
    public async Task<IActionResult> GetHighlights(Guid bookId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _mediator.Send(new GetHighlightsQuery(userId, bookId));
        return Ok(ApiResponse<List<HighlightDto>>.Success(result, "Highlights retrieved successfully"));
    }

    [HttpPost("{bookId}/notes")]
    public async Task<IActionResult> AddNote(Guid bookId, [FromBody] AddNoteRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new AddNoteCommand(userId, bookId, request.PageNumber, request.Content);
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<AddNoteResponse>.Success(result, "Note added successfully", 201));
    }
}

public record SyncProgressRequest(int CurrentPage, int TotalPages);
public record AddBookmarkRequest(int PageNumber, string? Note, string? ChapterTitle);
public record AddHighlightRequest(int PageNumber, string SelectedText, string Color, string? Note);
public record AddNoteRequest(int PageNumber, string Content);