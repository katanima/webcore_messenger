namespace webcore_backend.Shared.Exceptions;

public class MissingPermissionException : Exception
{
    public MissingPermissionException() { }

    public MissingPermissionException(string message) : base(message) { }

    public MissingPermissionException(string message, Exception innerException)
        : base(message, innerException) { }
}
