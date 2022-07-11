using System;
using System.Linq.Expressions;

namespace pdq.Implementation
{
    public class SelectColumnBuilder : ISelectColumnBuilder
    {
        private SelectColumnBuilder() { }

        public static ISelectColumnBuilder Create() => new SelectColumnBuilder();

        public object Is(string column) => null;

        public object Is(string column, string tableAlias) => null;

        public object Is<T>(Expression<Func<T, object>> expression) => null;
    }
}

