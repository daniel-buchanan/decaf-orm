using System;
namespace pdq.common.Connections
{
    [Serializable]
	public class ConnectionModificationException : Exception
	{
		public ConnectionModificationException() { }

		public ConnectionModificationException(string message) : base(message) { }
	}
}

