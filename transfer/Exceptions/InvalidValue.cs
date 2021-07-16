using System;

namespace transfer.Exceptions
{
    public class InvalidValue : Exception
    {
        public InvalidValue() { }

        public InvalidValue(string message)
            : base(message) { }

        public InvalidValue(string message, Exception inner)
            : base(message, inner) { }
    }
}
