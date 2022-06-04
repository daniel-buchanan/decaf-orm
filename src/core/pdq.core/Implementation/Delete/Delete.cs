using pdq.core.common;
using pdq.core.state;

namespace pdq.core.Implementation.Delete
{
	internal class Delete : DeleteBase, IDelete
	{
		public Delete(
            IQueryInternal query,
            IDeleteQueryContext context)
            : base(query, context)
		{
		}

        public IDeleteFrom From(string name, string? alias = null, string? schema = null)
        {
            Context.From(Table.Create(name, alias, schema));
            return Query.GetFluent<IDeleteFrom>();
        }
    }
}

