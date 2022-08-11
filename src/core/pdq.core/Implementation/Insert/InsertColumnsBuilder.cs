using System;
namespace pdq.Implementation
{
    public class InsertColumnBuilder : IInsertColumnBuilder
    {
        private InsertColumnBuilder() { }

        public static IInsertColumnBuilder Create()
            => new InsertColumnBuilder();

        public T Is<T>() => default(T);
    }
}

