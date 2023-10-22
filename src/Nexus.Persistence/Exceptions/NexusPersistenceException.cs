namespace Nexus.Persistence;

public class NexusPersistenceException : Exception
{
    public const string UnableToRegisterNexusPersistence = "Unable to register Nexus Persistence.";
    
    public NexusPersistenceException(string message) : base(message)
    {
    }

    public NexusPersistenceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}