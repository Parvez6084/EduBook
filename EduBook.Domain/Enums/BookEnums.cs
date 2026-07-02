namespace EduBook.Domain.Enums;

public enum BookStatus
{
    Draft = 1,
    UnderReview = 2,
    Published = 3,
    Unpublished = 4,
    Rejected = 5
}

public enum BookFormat
{
    PDF = 1,
    EPUB = 2,
    Both = 3
}