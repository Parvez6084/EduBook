using EduBook.Domain.Common;

namespace EduBook.Domain.Entities;

public class Bookmark : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public int PageNumber { get; set; }
    public string? Note { get; set; }
    public string? ChapterTitle { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Book Book { get; set; } = null!;
}