namespace EduBook.Application.Common;

public class ApiResponse<T>
{
    public string Status { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Status = "success",
            StatusCode = statusCode,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Fail(string message, int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            Status = "error",
            StatusCode = statusCode,
            Message = message,
            Data = default
        };
    }
}