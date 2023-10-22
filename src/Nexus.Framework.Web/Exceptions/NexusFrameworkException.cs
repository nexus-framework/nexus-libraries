namespace Nexus.Framework.Web.Exceptions;

public class NexusFrameworkException: Exception
{
    public const string NullAssemblyException = "The calling assembly is null.";
    
    public NexusFrameworkException(string message) : base(message)
    {
    }

    public NexusFrameworkException(string message, Exception innerException) : base(message, innerException)
    {
    }
}