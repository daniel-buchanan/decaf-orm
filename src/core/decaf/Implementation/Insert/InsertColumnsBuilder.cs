using System;
namespace decaf.Implementation
{
    public class InsertColumnBuilder : IInsertColumnBuilder
    {
        private InsertColumnBuilder() { }

        public static IInsertColumnBuilder Create()
            => new InsertColumnBuilder();

        public T Is<T>() => default(T);
    }
}

