namespace Intercon.Application.Exceptions;

public class InternalException : BaseException
{
    public InternalException() : base(("Operation failed due to an internal server error. Please try again later."))
    {
    }
}