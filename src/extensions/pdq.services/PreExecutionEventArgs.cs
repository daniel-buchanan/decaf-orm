using System;
using pdq.common;

namespace pdq.services
{
    public class PreExecutionEventArgs : EventArgs
    {
        public PreExecutionEventArgs(IQueryContext context)
        {
            Context = context;
        }

        public IQueryContext Context { get; }
    }
}

