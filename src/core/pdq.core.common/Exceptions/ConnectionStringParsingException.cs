using System;
using System.Runtime.Serialization;

namespace pdq.common.Exceptions
{
    [Serializable]
    public class ConnectionStringParsingException : Exception
	{
		public ConnectionStringParsingException(string reason)
            : base(reason) { }

        public ConnectionStringParsingException(Exception innerException, string reason = null)
            : base(reason, innerException) { }


        protected ConnectionStringParsingException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}

