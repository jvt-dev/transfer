using System;

namespace transfer.Exceptions
{
    public class InvalidTransactionId : Exception
    {
        public InvalidTransactionId() { }

        public InvalidTransactionId(string message)
            : base(message) { }

        public InvalidTransactionId(string message, Exception inner)
            : base(message, inner) { }
    }
}
