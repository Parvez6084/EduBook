using EduBook.Domain.Common;

namespace EduBook.Domain.Entities;

public class Note : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public int PageNumber { get; set; }
    public string Content { get; set; } = string.Empty;

    // Navigation
    public User User { get; set; } = null!;
    public Book Book { get; set; } = null!;
}