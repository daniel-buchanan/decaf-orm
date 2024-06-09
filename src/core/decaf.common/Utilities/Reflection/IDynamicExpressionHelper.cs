﻿using System.Collections.Generic;
using System.Linq.Expressions;
using decaf.common.Utilities.Reflection.Dynamic;

namespace decaf.common.Utilities.Reflection
{
    public interface IDynamicExpressionHelper
    {
        /// <summary>
        /// Gets the properties of a newly created object in an <see cref="Expression"/>.<br/>
        /// This is done whether it is a "dynamic" object or a "concrete" object.
        /// </summary>
        /// <param name="expr">The <see cref="Expression"/> to parse.</param>
        /// <param name="context">The <see cref="IQueryContextExtended"/> to use</param>
        /// <returns>
        /// An <see cref="IEnumerable{DynamicPropertyInfo}"/> containing the set
        /// of properties in the object returned from the <see cref="Expression"/>.
        /// </returns>
        IEnumerable<DynamicColumnInfo> GetProperties(
            Expression expr,
            IQueryContextExtended context);
    }
}