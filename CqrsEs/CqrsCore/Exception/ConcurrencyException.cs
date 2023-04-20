namespace CqrsCore.Exception;

public class ConcurrencyException : System.Exception
{
    public ConcurrencyException()
    {
    }
    
    public ConcurrencyException(string message) : base(message)
    {
    }
}