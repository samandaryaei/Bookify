namespace Bookify.Application.Exceptions;

public class ConcurrencyException(string message, Exception innerException) : Exception(message, innerException)
{
    
}