using EduBook.Domain.Common;

namespace EduBook.Domain.Entities;

public class Author : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
