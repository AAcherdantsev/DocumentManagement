using FluentResults;

namespace DocumentManagement.PublicModels.Errors;

/// <summary>
/// Represents an error indicating that a specified resource, such as a document, was not found.
/// </summary>
public sealed record NotFoundError : Error
{
    public NotFoundError(string message)
    {
        Message = message;
    }
}

/// <summary>
/// Represents an error indicating a conflict has occurred, such as attempting to create or update a resource
/// that already exists with the same identifier.
/// </summary>
public sealed record ConflictError : Error
{
    public ConflictError(string message)
    {
        Message = message;
    }
}

/// <summary>
/// Represents an error indicating that an operation has timed out before completion.
/// </summary>
public sealed record TimeoutError : Error
{
    public TimeoutError(string message)
    {
        Message = message;
    }
}

/// <summary>
/// Provides a base class for representing errors.
/// </summary>
public abstract record Error : IError
{
    public string Message { get; init; } = string.Empty;
    public Dictionary<string, object> Metadata { get; init; } = [];
    public List<IError> Reasons { get; init; } = [];
}