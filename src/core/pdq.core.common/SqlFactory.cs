using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common.Exceptions;
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
                values = ParseSelectParameters(context);
            if (context.Kind == QueryTypes.Delete)
                values = ParseDeleteParameters(context);
            if (context.Kind == QueryTypes.Update)
                values = ParseUpdateParameters(context);
            if (context.Kind == QueryTypes.Insert)
                values = ParseInsertParameters(context);

            if (values == null) return new { };

            if (!CheckParameters(values.Keys.ToArray(), template))
                throw new SqlTemplateMismatchException("Provided template, does not match provided parameters.");

            return MapDictionary(values);
        }

        protected abstract SqlTemplate ParseSelectQuery(IQueryContext context);

        protected abstract SqlTemplate ParseDeleteQuery(IQueryContext context);

        protected abstract SqlTemplate ParseUpdateQuery(IQueryContext context);

        protected abstract SqlTemplate ParseInsertQuery(IQueryContext context);

        protected abstract Dictionary<string, object> ParseSelectParameters(IQueryContext context);

        protected abstract Dictionary<string, object> ParseDeleteParameters(IQueryContext context);

        protected abstract Dictionary<string, object> ParseUpdateParameters(IQueryContext context);

        protected abstract Dictionary<string, object> ParseInsertParameters(IQueryContext context);

        private object MapDictionary(Dictionary<string, object> dict)
            => DynamicDictionary.FromDictionary(dict);

        private bool CheckParameters(string[] parameterNames, SqlTemplate template)
        {
            if (parameterNames.Length != (template.Parameters?.Count() ?? 0))
                return false;

            var result = true;
            for(var i = 0; i < parameterNames.Length; i++)
            {
                var existing = template.Parameters.FirstOrDefault(p => p.Name.EndsWith(parameterNames[i]));
                result &= existing != null;
            }

            return result;
        }
    }
}

