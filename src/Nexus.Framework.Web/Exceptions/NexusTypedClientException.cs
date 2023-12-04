namespace Nexus.Framework.Web.Exceptions;

/// <summary>
/// The NexusTypedClientException class defines exception types for issues related to NexusTypedClient
/// </summary>
public class NexusTypedClientException : Exception
{
    /// <summary>
    /// Message for the case when a typed client cannot be registered
    /// </summary>
    public const string UnableToRegisterTypedClient = "Unable to register typed client.";
    
    /// <summary>
    /// Initializes a new instance of the NexusTypedClientException class with a specified error message
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public NexusTypedClientException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the NexusTypedClientException class with a specified error message and a reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not null reference, the current exception is raised in a catch block that handles the inner exception</param>
    public NexusTypedClientException(string message, Exception innerException) : base(message, innerException)
    {
    }
}