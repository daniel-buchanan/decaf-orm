using System;
using System.Collections.Generic;
using System.Linq;
using pdq.common;
using pdq.common.Exceptions;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.state.Conditionals;

namespace pdq.state
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
            SqlTemplate template;
            var key = context.GetHash();
            var existing = this.cache.TryGetValue(key, out template);

            if (existing) return template;

            if (context is ISelectQueryContext selectContext)
                template = ParseQuery(selectContext);
            if (context is IDeleteQueryContext deleteContext)
                template = ParseQuery(deleteContext);
            if (context is IUpdateQueryContext updateContext)
                template = ParseQuery(updateContext);
            if (context is IInsertQueryContext insertContext)
                template = ParseQuery(insertContext);

            if (template == null) return null;
            this.cache.Add(key, template);
            return template;
        }

        /// <inheritdoc/>
        public object ParseParameters(IQueryContext context, SqlTemplate template)
        {
            IDictionary<string, object> values = null;

            if (context is ISelectQueryContext selectContext)
                values = ParseParameters(selectContext);
            if (context is IDeleteQueryContext deleteContext)
                values = ParseParameters(deleteContext);
            if (context is IUpdateQueryContext updateContext)
                values = ParseParameters(updateContext);
            if (context is IInsertQueryContext insertContext)
                values = ParseParameters(insertContext);

            if (values == null) return new { };

            if (!CheckParameters(values.Keys.ToArray(), template))
                throw new SqlTemplateMismatchException("Provided template, does not match provided parameters.");

            return MapDictionary(values);
        }

        /// <summary>
        /// Parse a select query into a SQL Template
        /// </summary>
        /// <param name="context">The context to parse.</param>
        /// <returns>A SQL Template ready to execute.</returns>
        protected abstract SqlTemplate ParseQuery(ISelectQueryContext context);

        /// <summary>
        /// Parse a delete query into a SQL Template
        /// </summary>
        /// <param name="context">The context to parse.</param>
        /// <returns>A SQL Template ready to execute.</returns>
        protected abstract SqlTemplate ParseQuery(IDeleteQueryContext context);

        /// <summary>
        /// Parse an update query into a SQL Template
        /// </summary>
        /// <param name="context">The context to parse.</param>
        /// <returns>A SQL Template ready to execute.</returns>
        protected abstract SqlTemplate ParseQuery(IUpdateQueryContext context);

        /// <summary>
        /// Parse an insert query into a SQL Template
        /// </summary>
        /// <param name="context">The context to parse.</param>
        /// <returns>A SQL Template ready to execute.</returns>
        protected abstract SqlTemplate ParseQuery(IInsertQueryContext context);

        /// <summary>
        /// Parse the parameters from a select context, with their values.
        /// </summary>
        /// <param name="context">The context to parse.</param>
        /// <returns>A dictionary containing the parameters.</returns>
        protected abstract IDictionary<string, object> ParseParameters(ISelectQueryContext context);

        /// <summary>
        /// Parse the parameters from a delete context, with their values.
        /// </summary>
        /// <param name="context">The context to parse.</param>
        /// <returns>A dictionary containing the parameters.</returns>
        protected abstract IDictionary<string, object> ParseParameters(IDeleteQueryContext context);

        /// <summary>
        /// Parse the parameters from an update context, with their values.
        /// </summary>
        /// <param name="context">The context to parse.</param>
        /// <returns>A dictionary containing the parameters.</returns>
        protected abstract IDictionary<string, object> ParseParameters(IUpdateQueryContext context);

        /// <summary>
        /// Parse the parameters from an insert context, with their values.
        /// </summary>
        /// <param name="context">The context to parse.</param>
        /// <returns>A dictionary containing the parameters.</returns>
        protected abstract IDictionary<string, object> ParseParameters(IInsertQueryContext context);

        /// <summary>
        /// Map dictionary values to anonymous objects.
        /// </summary>
        /// <param name="dict">The dictionary to parse.</param>
        /// <returns>An anonymous object with the properties in the source dictionary.</returns>
        private object MapDictionary(IDictionary<string, object> dict)
            => DynamicDictionary.FromDictionary(dict);

        /// <summary>
        /// Check the provided parameters against the template to ensure they are correct.
        /// </summary>
        /// <param name="parameterNames">THe names of the parameters.</param>
        /// <param name="template">An existing SQL Template to check against.</param>
        /// <returns>True if the parameters are valid, False if they aren't.</returns>
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

