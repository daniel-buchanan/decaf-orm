using System;
using System.Runtime.Serialization;

namespace pdq.common.Connections
{
    [Serializable]
	public class ConnectionModificationException : Exception
	{
		public ConnectionModificationException() { }

        protected ConnectionModificationException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

        public ConnectionModificationException(string message) : base(message) { }
	}
}

