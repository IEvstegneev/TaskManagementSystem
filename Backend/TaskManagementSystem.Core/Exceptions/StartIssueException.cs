using System.Runtime.Serialization;

namespace TaskManagementSystem.Core.Exceptions
{
    [Serializable]
    internal class StartIssueException : Exception
    {
        public StartIssueException()
        {
        }

        public StartIssueException(string? message) : base(message)
        {
        }

        public StartIssueException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected StartIssueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}