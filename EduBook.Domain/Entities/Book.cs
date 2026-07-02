using EduBook.Domain.Common;
using EduBook.Domain.Enums;

namespace EduBook.Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ISBN { get; set; }
    public string? CoverImageUrl { get; set; }
    public decimal Price { get; set; }
    public int TotalPages { get; set; }
    public string Language { get; set; } = "Bengali";
    public BookStatus Status { get; set; } = BookStatus.Draft;
    public BookFormat Format { get; set; } = BookFormat.PDF;
    public int? GradeLevel { get; set; }
    public DateTime? PublishedDate { get; set; }
    public Guid? PublisherId { get; set; }

    // Navigation
    public Publisher? Publisher { get; set; }
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    public ICollection<BookFile> BookFiles { get; set; } = new List<BookFile>();
    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

}