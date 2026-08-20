namespace EduBook.Application.Interfaces;

public interface IStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task<string> GenerateSignedUrlAsync(string storageKey, int expiryMinutes = 15);
    Task DeleteFileAsync(string storageKey);
}