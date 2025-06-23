using System;
using System.Runtime.Serialization;

namespace decaf.db.common.Exceptions;

[Serializable]
public class ServiceNotFoundException : Exception
{
    public ServiceNotFoundException(string serviceName)
        : base($"Service {serviceName} could not be found.") { }


    protected ServiceNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}