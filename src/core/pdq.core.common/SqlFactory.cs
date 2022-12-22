using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common.Templates;
using pdq.common.Utilities;

namespace pdq.common
{
    public abstract class SqlFactory : ISqlFactory
    {
        private readonly Dictionary<string, SqlTemplate> cache;

        protected SqlFactory()
        {
            this.cache = new Dictionary<string, SqlTemplate>();
        }

        /// <inheritdoc/>
        public SqlTemplate ParseTemplate(IQueryContext context)
        {
            SqlTemplate template = null;
            var key = context.GetHash();
            var existing = this.cache.TryGetValue(key, out template);

            if (existing) return template;

            if (context.Kind == QueryTypes.Select)
                template = ParseSelectQuery(context);
            if (context.Kind == QueryTypes.Delete)
                template = ParseDeleteQuery(context);
            if (context.Kind == QueryTypes.Update)
                template = ParseUpdateQuery(context);
            if (context.Kind == QueryTypes.Insert)
                template = ParseInsertQuery(context);

            if (template == null) return null;
            this.cache.Add(key, template);
            return template;
        }

        /// <inheritdoc/>
        public object ParseParameters(IQueryContext context, SqlTemplate template)
        {
            Dictionary<string, object> values = null;

            if (context.Kind == QueryTypes.Select)
                values = ParseSelectParameters(context, template);
            if (context.Kind == QueryTypes.Delete)
                values = ParseDeleteParameters(context, template);
            if (context.Kind == QueryTypes.Update)
                values = ParseUpdateParameters(context, template);
            if (context.Kind == QueryTypes.Insert)
                values = ParseInsertParameters(context, template);

            if (values == null) return new { };

            return MapDictionary(values);
        }

        protected abstract SqlTemplate ParseSelectQuery(IQueryContext context);

        protected abstract SqlTemplate ParseDeleteQuery(IQueryContext context);

        protected abstract SqlTemplate ParseUpdateQuery(IQueryContext context);

        protected abstract SqlTemplate ParseInsertQuery(IQueryContext context);

        protected abstract Dictionary<string, object> ParseSelectParameters(IQueryContext context, SqlTemplate template);

        protected abstract Dictionary<string, object> ParseDeleteParameters(IQueryContext context, SqlTemplate template);

        protected abstract Dictionary<string, object> ParseUpdateParameters(IQueryContext context, SqlTemplate template);

        protected abstract Dictionary<string, object> ParseInsertParameters(IQueryContext context, SqlTemplate template);

        private object MapDictionary(Dictionary<string, object> dict)
            => DynamicDictionary.FromDictionary(dict);
    }
}

