namespace Intercon.Application.CustomExceptions;

public record ExceptionDetails(
    int Status,
    string Type,
    string Title,
    string Detail,
    IEnumerable<object>? Errors);