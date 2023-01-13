using System;
using pdq.common.Templates;

namespace pdq.common
{
    public interface ISqlFactory
    {
        /// <summary>
        /// Parse a <see cref="IQueryContext"/> and return a template.
        /// </summary>
        /// <param name="context">The <see cref="IQueryContext"/> to parse.</param>
        /// <returns>Returns a <see cref="SqlTemplate"/> parsed from the provided <see cref="IQueryContext"/>.</returns>
        SqlTemplate ParseTemplate(IQueryContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        object ParseParameters(IQueryContext context, SqlTemplate template);
    }
}

