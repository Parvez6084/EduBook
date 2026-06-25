using EduBook.Domain.Entities;

namespace EduBook.Application.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}