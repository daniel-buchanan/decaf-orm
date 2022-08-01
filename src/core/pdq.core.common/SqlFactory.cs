using System;
namespace pdq.common
{
    public abstract class SqlFactory : ISqlFactory
    {
        public SqlFactory()
        {
        }

        public SqlTemplate ParseContext(IQueryContext context)
        {
            throw new NotImplementedException();
        }
    }
}

