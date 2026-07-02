namespace EduBook.Domain.Enums;

public enum UserRole
{
    Student = 1,
    Teacher = 2,
    Parent = 3,
    Institution = 4,
    Admin = 5,
    SuperAdmin = 6
}

public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Banned = 3,
    PendingVerification = 4
}

public enum OtpType
{
    EmailVerification = 1,
    PhoneVerification = 2,
    PasswordReset = 3,
    Login = 4
}