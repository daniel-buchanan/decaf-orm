using System;
namespace pdq.common
{
    public interface ISqlFactory
    {
        /// <summary>
        /// Parse a <see cref="IQueryContext"/> and return a template.
        /// </summary>
        /// <param name="context">The <see cref="IQueryContext"/> to parse.</param>
        /// <returns>Returns a <see cref="SqlTemplate"/> parsed from the provided <see cref="IQueryContext"/>.</returns>
        SqlTemplate ParseContext(IQueryContext context);
    }
}

