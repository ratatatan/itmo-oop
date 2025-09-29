public class ItemException : Exception
{
    public ItemException() : base() { }
    public ItemException(string message) : base(message) { }
    public ItemException(string message, Exception inner) : base(message, inner) { }
}
