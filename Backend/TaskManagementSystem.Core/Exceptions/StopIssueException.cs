using System.Runtime.Serialization;

namespace TaskManagementSystem.Core.Exceptions
{
    [Serializable]
    internal class StopIssueException : Exception
    {
        public StopIssueException()
        {
        }

        public StopIssueException(string? message) : base(message)
        {
        }

        public StopIssueException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected StopIssueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}