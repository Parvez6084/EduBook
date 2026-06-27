using EduBook.Domain.Common;

namespace EduBook.Domain.Entities;

public class Publisher : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? LogoUrl { get; set; }

    public string? Website { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
