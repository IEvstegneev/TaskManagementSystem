namespace TaskManagementSystem.Core
{
    public class OperationResult
    {
        public bool IsFailed { get; protected set; }
        public string? ErrorMessage { get; protected set; }

        protected OperationResult() { }
        protected OperationResult(string message)
        {
            ErrorMessage = message;
            IsFailed = true;
        }

        public static OperationResult Ok() => new();
        public static OperationResult Failed(string message) => new(message);
        public static OperationResult NotFoundById(Guid id) 
            => new($"Issue with such Id is not found: {id}");
    }

    public class OperationResult<T> : OperationResult
    {
        public T? Value { get; private set; }

        private OperationResult() : base() { }
        private OperationResult(string message) : base(message) { }
        private OperationResult(T value)
        {
            Value = value;
        }

        public static OperationResult<T> Ok(T value) => new(value);
        public static new OperationResult<T> Ok() => new();
        public static new OperationResult<T> Failed(string message) => new(message);
        public static new OperationResult<T> NotFoundById(Guid id)
            => new($"Issue with such Id is not found: {id}");
    }
}
