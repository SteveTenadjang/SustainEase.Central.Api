namespace Central.Application.Exceptions;

public class ForbiddenException(string message) : ApplicationException(message)
{
    public ForbiddenException() : this("Access forbidden.")
    {
    }
}