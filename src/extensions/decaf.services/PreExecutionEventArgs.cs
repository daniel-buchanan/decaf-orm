using System;
using decaf.common;

namespace decaf.services;

public class PreExecutionEventArgs : EventArgs
{
    public PreExecutionEventArgs(IQueryContext context)
    {
        Context = context;
    }

    public IQueryContext Context { get; }
}