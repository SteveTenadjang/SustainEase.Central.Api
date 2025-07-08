namespace Central.Application.Exceptions;

public class ConflictException : ApplicationException
{
    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException(string name, object key)
        : base($"Entity \"{name}\" ({key}) already exists.")
    {
    }
}