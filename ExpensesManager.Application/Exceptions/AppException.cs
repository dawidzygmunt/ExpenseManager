namespace ExpensesManager.Application.Exceptions;

public class AppException : Exception
{
    public AppException(int statusCode, string message, IEnumerable<string>? errors = null)
        : base(message)
    {
        StatusCode = statusCode;
        Errors = errors?.ToList() ?? new List<string> { message };
    }

    public int StatusCode { get; }
    public IReadOnlyList<string> Errors { get; }
}

public sealed class ConflictException(string message) : AppException(409, message);

public sealed class NotFoundException(string message) : AppException(404, message);

public sealed class UnauthorizedException(string message) : AppException(401, message);

public sealed class ValidationException(IEnumerable<string> errors)
    : AppException(400, "Validation failed", errors);