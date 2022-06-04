using pdq.common;
using pdq.state;

namespace pdq.Implementation.Delete
{
	internal class Delete : IDelete, IDeleteFrom
	{
        private readonly IQueryInternal query;
        private readonly IDeleteQueryContext context;

        private Delete(IQueryInternal query)
        {
            this.query = query;
            this.context = DeleteQueryContext.Create();
        }

        public static Delete Create(IQueryInternal query) => new Delete(query);

        public void Dispose() => this.context.Dispose();

        public IDeleteFrom From(string name, string alias = null, string schema = null)
        {
            context.From(Table.Create(name, alias, schema));
            return this;
        }

        public IDeleteFrom Where(state.IWhere where)
        {
            context.Where(where);
            return this;
        }

        public string GetSql()
        {
            throw new System.NotImplementedException();
        }
    }
}

