namespace URLShortener.Domain.Exceptions;

public class UrlException: Exception
{
    public UrlException(string message) : base(message)
    {
    }
}