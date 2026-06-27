using EduBook.Domain.Common;
using EduBook.Domain.Enums;

namespace EduBook.Domain.Entities;

public class BookFile : BaseEntity
{
    public Guid BookId { get; set; }
    public string StorageKey { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public BookFormat Format { get; set; }
    public bool IsEncrypted { get; set; } = true;

    // Navigation
    public Book Book { get; set; } = null!;
}