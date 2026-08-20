using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using EduBook.Domain.Entities;
using EduBook.Domain.Enums;
using EduBook.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduBook.Application.Features.Books.Commands;

public class UploadBookFileCommandHandler : BaseHandler, IRequestHandler<UploadBookFileCommand, UploadBookFileResponse>
{
    private readonly IStorageService _storageService;

    public UploadBookFileCommandHandler(
        IApplicationDbContext context,
        IStorageService storageService) : base(context)
    {
        _storageService = storageService;
    }

    public async Task<UploadBookFileResponse> Handle(UploadBookFileCommand request, CancellationToken cancellationToken)
    {
        // Check book exists
        var book = await Context.Books
            .FirstOrDefaultAsync(b => b.Id == request.BookId && b.DeletedAt == null, cancellationToken);

        if (book == null)
            throw new NotFoundException("Book not found");

        // Validate file type
        var allowedTypes = new[] { "application/pdf", "application/epub+zip" };
        if (!allowedTypes.Contains(request.File.ContentType))
            throw new ValidationException("Only PDF and EPUB files are allowed");

        // Validate file size (max 100MB)
        if (request.File.Length > 100 * 1024 * 1024)
            throw new ValidationException("File size cannot exceed 100MB");

        // Upload to storage
        using var stream = request.File.OpenReadStream();
        var storageKey = await _storageService.UploadFileAsync(
            stream,
            request.File.FileName,
            request.File.ContentType,
            cancellationToken);

        // Save file record
        var bookFile = new BookFile
        {
            BookId = request.BookId,
            StorageKey = storageKey,
            FileName = request.File.FileName,
            FileSizeBytes = request.File.Length,
            Format = Enum.Parse<BookFormat>(request.Format),
            IsEncrypted = true
        };

        Context.BookFiles.Add(bookFile);
        await Context.SaveChangesAsync(cancellationToken);

        return new UploadBookFileResponse(
            bookFile.Id,
            bookFile.FileName,
            bookFile.Format.ToString(),
            bookFile.FileSizeBytes,
            bookFile.StorageKey
        );
    }
}