using EduBook.Domain.Common;
using EduBook.Domain.Enums;
namespace EduBook.Domain.Entities;

public class  User :BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PasswordHash { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsPhoneVerified { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<OtpCode> OtpCodes { get; set; } = new List<OtpCode>();
    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();


    public ICollection<ReadingProgress> ReadingProgresses { get; set; } = new List<ReadingProgress>();
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    public ICollection<Highlight> Highlights { get; set; } = new List<Highlight>();
    public ICollection<Note> Notes { get; set; } = new List<Note>();

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
