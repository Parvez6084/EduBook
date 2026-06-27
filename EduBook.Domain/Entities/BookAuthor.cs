namespace EduBook.Domain.Entities;

public class BookAuthor
{
    public Guid BookId { get; set; }
    public Guid AuthorId { get; set; }
    public int DisplayOrder { get; set; } = 1;

    // Navigation
    public Book Book { get; set; } = null!;
    public Author Author { get; set; } = null!;
}