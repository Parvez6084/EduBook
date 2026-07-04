using EduBook.Domain.Common;
using EduBook.Domain.Enums;

namespace EduBook.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; } = false;
    public string? Data { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}