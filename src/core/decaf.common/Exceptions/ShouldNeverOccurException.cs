using System;
using System.Runtime.Serialization;

namespace decaf.common.Exceptions
{
    [Serializable]
    public class ShouldNeverOccurException : Exception
    {
        public ShouldNeverOccurException(string message)
            : base(message)
        {
        }

        protected ShouldNeverOccurException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}

