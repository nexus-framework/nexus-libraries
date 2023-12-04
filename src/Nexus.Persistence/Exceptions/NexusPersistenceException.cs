namespace Nexus.Persistence;

/// <summary>
/// Represents errors that occur during Nexus persistence operations.
/// </summary>
public class NexusPersistenceException : Exception
{
    /// <summary>
    /// Constant value representing an error message when Nexus Persistence registration fails.
    /// </summary>
    public const string UnableToRegisterNexusPersistence = "Unable to register Nexus Persistence.";
    
    /// <summary>
    /// Initializes a new instance of the <see cref="NexusPersistenceException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NexusPersistenceException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NexusPersistenceException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
    public NexusPersistenceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}