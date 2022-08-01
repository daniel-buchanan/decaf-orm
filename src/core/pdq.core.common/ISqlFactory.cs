using System;
namespace pdq.common
{
    public interface ISqlFactory
    {
        SqlTemplate ParseContext(IQueryContext context);
    }
}

