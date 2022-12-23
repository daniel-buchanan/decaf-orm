using System;
using System.Runtime.Serialization;

namespace pdq.common.Exceptions
{
    [Serializable]
	public class SqlTemplateMismatchException : Exception
    {
        public SqlTemplateMismatchException(string reason)
            : base(reason) { }


        protected SqlTemplateMismatchException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}

