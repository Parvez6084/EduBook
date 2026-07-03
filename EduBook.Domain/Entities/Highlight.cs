using EduBook.Domain.Common;

namespace EduBook.Domain.Entities;

public class Highlight : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public int PageNumber { get; set; }
    public string SelectedText { get; set; } = string.Empty;
    public string Color { get; set; } = "#FFFF00";
    public string? Note { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Book Book { get; set; } = null!;
}