using pdq.common;
using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
	internal class Delete : Execute, IDelete, IDeleteFrom
	{
        private readonly IDeleteQueryContext context;

        private Delete(IQuery query) : base((IQueryInternal)query)
        {
            this.context = DeleteQueryContext.Create();
            this.query.SetContext(this.context);
        }

        public static Delete Create(IQuery query) => new Delete(query);

        public void Dispose() => this.context.Dispose();

        public IDeleteFrom From(string name, string alias = null, string schema = null)
        {
            context.From(state.QueryTargets.TableTarget.Create(name, alias, schema));
            return this;
        }

        public IDeleteFrom Where(state.IWhere where)
        {
            context.Where(where);
            return this;
        }
    }
}

