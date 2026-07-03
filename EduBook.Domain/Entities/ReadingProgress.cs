using EduBook.Domain.Common;

namespace EduBook.Domain.Entities;

public class ReadingProgress : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public double ProgressPercentage => TotalPages > 0 ? (double)CurrentPage / TotalPages * 100 : 0;
    public DateTime LastReadAt { get; set; } = DateTime.UtcNow;
    public int TotalReadingMinutes { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Book Book { get; set; } = null!;
}