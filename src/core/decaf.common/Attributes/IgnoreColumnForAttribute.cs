using System;
using decaf.common;

namespace decaf.Attributes
{
    public class IgnoreColumnForAttribute : Attribute
    {
        public QueryTypes QueryType { get; set; }

        public IgnoreColumnForAttribute() { QueryType = QueryTypes.None; }

        public IgnoreColumnForAttribute(QueryTypes toIgnore)
        {
            this.QueryType = toIgnore;
        }
    }
}

