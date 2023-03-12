using Social.Application.Enums;

namespace Social.Application.Models;

public class OperationResult<T>
{
    public T Payload { get; set; }
    public bool IsError { get; private set; }
    public List<Error> Errors { get;} = new();

/// <summary>
/// Adds an error to the error list and sets IsError flag to true
/// </summary>
/// <param name="code"></param>
/// <param name="message"></param>
    public void AddError(ErrorCode code, string message)
    {
        HandleError(code, message);
    }

    /// <summary>
    /// Adds a default error to the error list with the error code UnknownError
    /// </summary>
    public void AddUnknownError(string message)
    {
        HandleError(ErrorCode.UnknownError, message);
    }

/// <summary>
/// Sets to IsError flag to false
/// </summary>
    public void ResetIsErrorFlag()
    {
        IsError = false;
    }

#region MyRegion

private void HandleError(ErrorCode code, string message)
{
    Errors.Add(new Error { Code = code, Message = message }) ;
    IsError = true;
}

#endregion

}