namespace webcore_backend.Features.Guilds.Exceptions;

public class MissingPermission : Exception
{
    public MissingPermission() { }

    public MissingPermission(string message) : base(message) { }

    public MissingPermission(string message, Exception innerException)
        : base(message, innerException) { }
}
