namespace Intercon.Application.Exceptions;

public class InternalException : BaseException
{
    public InternalException() : base(("Operation failed due to an internal server error. Please try again later."))
    {
    }
}

public class NotFoundException : BaseException
{
    public NotFoundException(string obj) : base($"The {obj} not found")
    {
    }
}