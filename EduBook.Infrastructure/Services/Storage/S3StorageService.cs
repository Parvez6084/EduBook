using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace EduBook.Infrastructure.Services.Storage;

public class S3StorageService : IStorageService
{
    private readonly StorageSettings _settings;
    private readonly AmazonS3Client _s3Client;

    public S3StorageService(IOptions<StorageSettings> settings)
    {
        _settings = settings.Value;

        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USEast1,
            ServiceURL = _settings.ServiceUrl,
            ForcePathStyle = true // Required for MinIO
        };

        _s3Client = new AmazonS3Client(
            _settings.AccessKey,
            _settings.SecretKey,
            config);
    }

    public async Task<string> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var storageKey = $"books/{Guid.NewGuid()}/{fileName}";

        var uploadRequest = new TransferUtilityUploadRequest
        {
            BucketName = _settings.BucketName,
            Key = storageKey,
            InputStream = fileStream,
            ContentType = contentType,
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };

        var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(uploadRequest, cancellationToken);

        return storageKey;
    }

    public async Task<string> GenerateSignedUrlAsync(string storageKey, int expiryMinutes = 15)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = storageKey,
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Verb = HttpVerb.GET
        };

        return await Task.FromResult(_s3Client.GetPreSignedURL(request));
    }

    public async Task DeleteFileAsync(string storageKey)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = storageKey
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
    }
}