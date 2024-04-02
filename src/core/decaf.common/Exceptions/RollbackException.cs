using System;
using System.Runtime.Serialization;

namespace decaf.common.Exceptions
{
    public class RollbackException : SqlException
    {
        public RollbackException(string reason)
            : base(reason) { }

        public RollbackException(Exception innerException, string reason = null)
            : base(innerException, reason) { }


        protected RollbackException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}