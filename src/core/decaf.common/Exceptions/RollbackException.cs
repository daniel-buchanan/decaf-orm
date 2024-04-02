using System;
using System.Runtime.Serialization;

namespace decaf.tests.common.Mocks.Exceptions
{
    public class RollbackException : Exception
    {
        public RollbackException(string reason)
            : base(reason) { }

        public RollbackException(Exception innerException, string reason = null)
            : base(reason, innerException) { }


        protected RollbackException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}