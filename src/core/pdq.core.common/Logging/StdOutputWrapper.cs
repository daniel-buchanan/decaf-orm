using System;

namespace pdq.common.Logging
{
    public interface IStdOutputWrapper
    {
        void WriteOut(string message);
        void WriteErr(string message);
    }
    
    public class StdOutputWrapper : IStdOutputWrapper
    {
        public void WriteOut(string message)
            => Console.WriteLine(message);

        public void WriteErr(string message) 
            => Console.Error.WriteLine(message);
    }
}