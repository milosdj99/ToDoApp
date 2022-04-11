namespace ToDoApi.Exceptions
{
    [Serializable]
    public class UpdateException : Exception
    {
        
        public UpdateException(string message) : base(message) { }
        public UpdateException(string message, Exception inner) : base(message, inner) { }

        
    }
}
