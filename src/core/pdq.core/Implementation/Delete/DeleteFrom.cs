using pdq.core.common;
using pdq.core.state;

namespace pdq.core.Implementation.Delete
{
	internal class DeleteFrom : DeleteBase, IDeleteFrom
	{
        public DeleteFrom(
            IQueryInternal query,
            IDeleteQueryContext context)
            : base(query, context)
        {
        }

        public IDeleteFrom Where(state.IWhere where)
        {
            Context.Where(where);
            return Query.GetFluent<IDeleteFrom>();
        }
    }
}

