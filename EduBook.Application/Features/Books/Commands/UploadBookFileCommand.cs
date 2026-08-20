using EduBook.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EduBook.Application.Features.Books.Commands;

public record UploadBookFileCommand(
    Guid BookId,
    IFormFile File,
    string Format
) : IRequest<UploadBookFileResponse>;

public record UploadBookFileResponse(
    Guid FileId,
    string FileName,
    string Format,
    long FileSizeBytes,
    string StorageKey
);