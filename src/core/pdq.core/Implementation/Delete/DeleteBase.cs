using System;
using pdq.core.common;
using pdq.core.state;

namespace pdq.core.Implementation.Delete
{
	internal abstract class DeleteBase : IBuilder, IFluentApi
	{
		private readonly IDeleteQueryContext context;
		private readonly IQueryInternal query;

		protected DeleteBase(
			IQueryInternal query,
			IDeleteQueryContext context)
		{
			this.query = query;
			this.context = context;
		}

		protected IDeleteQueryContext Context => this.context;

		protected IQueryInternal Query => this.query;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string GetHash() => this.GetType().GUID.ToString();

        public string GetSql()
        {
            throw new NotImplementedException();
        }
    }
}

