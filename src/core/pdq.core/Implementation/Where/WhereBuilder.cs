using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common;
using pdq.Exceptions;

namespace pdq.Implementation
{
    public class WhereBuilder : IWhereBuilder
	{
        private readonly List<state.IWhere> clauses;
        private readonly IQueryContext queryContext;
        private ClauseHandlingValues defaultClauseHandling;

		private WhereBuilder(IQueryContext queryContext)
		{
            this.defaultClauseHandling = ClauseHandlingValues.Unspecified;
            this.queryContext = queryContext;
            this.clauses = new List<state.IWhere>();
		}

        internal static IWhereBuilder Create(IQueryContext queryContext) => new WhereBuilder(queryContext);

        /// <inheritdoc />
        public IWhereBuilder And(Action<IWhereBuilder> builder)
        {
            var b = Create(this.queryContext);
            b.ClauseHandling().DefaultToAnd();
            builder(b);

            this.clauses.Add(b.GetClauses().First());
            return this;
        }

        /// <inheritdoc />
        public IClauseHandlingBehaviour ClauseHandling() => ClauseHandlingBehaviour.Create(this);

        /// <inheritdoc />
        public IColumnWhereBuilder Column(string name, string targetAlias = null)
        {
            var target = string.IsNullOrWhiteSpace(targetAlias) ?
                null :
                this.queryContext.QueryTargets.FirstOrDefault(t => t.Alias == targetAlias);
            var c = state.Column.Create(name, target);
            return ColumnWhereBuilder.Create(this, c);
        }

        /// <inheritdoc />
        public IWhereBuilder Or(Action<IWhereBuilder> builder)
        {
            var b = Create(this.queryContext);
            b.ClauseHandling().DefaultToOr();
            builder(b);

            this.clauses.Add(b.GetClauses().First());
            return this;
        }

        /// <inheritdoc />
        void IWhereBuilder.AddClause(state.IWhere item) => this.clauses.Add(item);

        /// <inheritdoc />
        IEnumerable<state.IWhere> IWhereBuilder.GetClauses()
        {
            if (this.defaultClauseHandling == ClauseHandlingValues.And)
                return new[] { state.Conditionals.And.Where(this.clauses) };

            if (this.defaultClauseHandling == ClauseHandlingValues.Or)
                return new[] { state.Conditionals.Or.Where(this.clauses) };

            throw new WhereBuildFailedException();
        }

        private enum ClauseHandlingValues
        {
            Unspecified,
            And,
            Or
        }

        private class ClauseHandlingBehaviour : IClauseHandlingBehaviour
        {
            private readonly WhereBuilder builder;

            private ClauseHandlingBehaviour(WhereBuilder builder)
                => this.builder = builder;

            public static IClauseHandlingBehaviour Create(WhereBuilder builder)
                => new ClauseHandlingBehaviour(builder);

            /// <inheritdoc />
            public void DefaultToAnd()
                => this.builder.defaultClauseHandling = ClauseHandlingValues.And;

            /// <inheritdoc />
            public void DefaultToOr()
                => this.builder.defaultClauseHandling = ClauseHandlingValues.Or;
        }
    }
}

