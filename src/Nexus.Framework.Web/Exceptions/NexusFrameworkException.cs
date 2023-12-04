namespace Nexus.Framework.Web.Exceptions;

/// <summary>
/// Exception class for Nexus Framework
/// </summary>
public class NexusFrameworkException: Exception
{
    /// <summary>
    /// Message indicating that the calling assembly is null.
    /// </summary>
    public const string NullAssemblyException = "The calling assembly is null.";
    
    /// <summary>
    /// Initializes a new instance of the <see cref="NexusFrameworkException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NexusFrameworkException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NexusFrameworkException"/> class with a specified
    /// error message and a reference to the inner exception that is the cause of this exception. 
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public NexusFrameworkException(string message, Exception innerException) : base(message, innerException)
    {
    }
}