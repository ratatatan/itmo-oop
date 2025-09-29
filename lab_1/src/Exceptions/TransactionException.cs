public class TransactionException : Exception
{
    public TransactionException() : base() { }
    public TransactionException(string message) : base(message) { }
    public TransactionException(string message, Exception inner) : base(message, inner) { }
}
