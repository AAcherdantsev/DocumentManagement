using FluentResults;

namespace DocumentManagement.API.Errors;

public record Error : IError
{
    public string Message { get; init; }
    public Dictionary<string, object> Metadata { get; init; }
    public List<IError> Reasons { get; init; }
}

public sealed record NotFoundError : Error
{
    public NotFoundError(string message)
    {
        Message = message;
    }
}

public sealed record ConflictError : Error
{
    public ConflictError(string message)
    {
        Message = message;
    }
}

public sealed record TimeoutError : Error
{
    public TimeoutError(string message)
    {
        Message = message;
    }
}