using System;
using System.Runtime.Serialization;

namespace decaf.common.Exceptions
{
    [Serializable]
    public class DecafOptionsInvalidException : Exception
	{
		public DecafOptionsInvalidException(string reason)
            : base(reason) { }


        protected DecafOptionsInvalidException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}

