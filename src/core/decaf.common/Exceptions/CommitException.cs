using System;
using System.Runtime.Serialization;

namespace decaf.common.Exceptions
{
    public class CommitException : SqlException
    {
        public CommitException(string reason)
            : base(reason) { }

        public CommitException(Exception innerException, string reason = null)
            : base(innerException, reason) { }


        protected CommitException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}