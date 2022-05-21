using System;
using pdq.core.common;

namespace pdq.core.Attributes
{
    public class IgnoreColumnForAttribute : Attribute
    {
        public QueryType QueryType { get; set; }

        public IgnoreColumnForAttribute() { QueryType = QueryType.None; }

        public IgnoreColumnForAttribute(QueryType toIgnore)
        {
            this.QueryType = toIgnore;
        }
    }
}

