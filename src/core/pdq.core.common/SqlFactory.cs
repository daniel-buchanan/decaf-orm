using System;
namespace pdq.common
{
    public abstract class SqlFactory : ISqlFactory
    {
        protected SqlFactory() { }

        /// <inheritdoc/>
        public SqlTemplate ParseContext(IQueryContext context)
        {
            switch(context.Kind)
            {
                case QueryTypes.Select: return ParseSelect(context);
                case QueryTypes.Delete: return ParseDelete(context);
                case QueryTypes.Update: return ParseUpdate(context);
                case QueryTypes.Insert: return ParseInsert(context);
                default: return null;
            }
        }

        protected abstract SqlTemplate ParseSelect(IQueryContext context);

        protected abstract SqlTemplate ParseDelete(IQueryContext context);

        protected abstract SqlTemplate ParseUpdate(IQueryContext context);

        protected abstract SqlTemplate ParseInsert(IQueryContext context);
    }
}

