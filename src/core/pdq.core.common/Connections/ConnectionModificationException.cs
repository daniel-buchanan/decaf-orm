using System;
namespace pdq.common.Connections
{
	public class ConnectionModificationException : Exception
	{
		public ConnectionModificationException() { }

		public ConnectionModificationException(string message) : base(message) { }
	}
}

