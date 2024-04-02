using System;
using System.Runtime.Serialization;

namespace decaf.tests.common.Mocks.Exceptions
{
    public class CommitException : Exception
    {
        public CommitException(string reason)
            : base(reason) { }

        public CommitException(Exception innerException, string reason = null)
            : base(reason, innerException) { }


        protected CommitException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}