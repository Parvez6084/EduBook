using EduBook.Domain.Common;
using EduBook.Domain.Enums;

namespace EduBook.Domain.Entities;

public class OtpCode : BaseEntity
{
    public string Target { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public OtpType Type { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public int AttemptCount { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
}
