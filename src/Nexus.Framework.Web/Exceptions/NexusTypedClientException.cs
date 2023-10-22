namespace Nexus.Framework.Web.Exceptions;

public class NexusTypedClientException : Exception
{
    public const string UnableToRegisterTypedClient = "Unable to register typed client.";
    
    public NexusTypedClientException(string message) : base(message)
    {
    }

    public NexusTypedClientException(string message, Exception innerException) : base(message, innerException)
    {
    }
}