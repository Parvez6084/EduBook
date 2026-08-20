namespace EduBook.Application.Common;

public class StorageSettings
{
    public string Provider { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string ServiceUrl { get; set; } = string.Empty;
    public string Region { get; set; } = "us-east-1";
}