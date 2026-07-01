namespace EduBook.Domain.Exceptions;

public class DuplicatePhoneException : Exception
{
    public DuplicatePhoneException(string phoneNumber) 
        : base($"A user with phone number '{phoneNumber}' already exists.") { }
}
