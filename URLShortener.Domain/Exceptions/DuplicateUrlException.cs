namespace URLShortener.Domain.Exceptions;

public class DuplicateUrlException: Exception
{
    public DuplicateUrlException(string message) : base(message)
    {
    }
}