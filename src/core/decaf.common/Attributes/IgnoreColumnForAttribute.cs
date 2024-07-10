using System;

namespace decaf.common.Attributes
{
    public class IgnoreColumnForAttribute : Attribute
    {
        public QueryTypes QueryType { get; set; }

        public IgnoreColumnForAttribute() { QueryType = QueryTypes.None; }

        public IgnoreColumnForAttribute(QueryTypes toIgnore)
        {
            QueryType = toIgnore;
        }
    }
}

