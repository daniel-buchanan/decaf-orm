using System;
namespace pdq.core.common.Connections
{
	public class ConnectionModificationException : Exception
	{
		public ConnectionModificationException() { }

		public ConnectionModificationException(string message) : base(message) { }
	}
}

