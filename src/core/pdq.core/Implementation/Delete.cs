﻿using pdq.common;
using pdq.state;

namespace pdq.Implementation
{
	internal class Delete : IDelete, IDeleteFrom
	{
        private readonly IQueryInternal query;
        private readonly IDeleteQueryContext context;

        private Delete(IQuery query)
        {
            this.query = (IQueryInternal)query;
            this.context = DeleteQueryContext.Create();
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

        public string GetSql()
        {
            throw new System.NotImplementedException();
        }
    }
}
