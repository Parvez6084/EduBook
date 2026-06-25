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
    public string? ProfilePictureUrl { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<OtpCode> OtpCodes { get; set; } = new List<OtpCode>();

}
