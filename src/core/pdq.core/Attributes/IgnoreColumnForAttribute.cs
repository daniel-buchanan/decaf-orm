using System;
using pdq.common;

namespace pdq.Attributes
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

