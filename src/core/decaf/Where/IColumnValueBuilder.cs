using System;
using System.Collections.Generic;

namespace decaf
{
    public interface IColumnValueBuilder
    {
        void EqualTo<T>(T value);

        IColumnMatchBuilder EqualTo();

        void Like<T>(T value);

        void StartsWith<T>(T value);

        void EndsWith<T>(T value);

        void In<T>(params T[] values);

        void In<T>(IEnumerable<T> values);

        void LessThan<T>(T value);

        IColumnMatchBuilder LessThan();

        void LessThanOrEqualTo<T>(T value);

        IColumnMatchBuilder LessThanOrEqualTo();

        void GreaterThan<T>(T value);

        IColumnMatchBuilder GreaterThan();

        void GreaterThanOrEqualTo<T>(T value);

        IColumnMatchBuilder GreaterThanOrEqualTo();

        void Between<T>(T start, T end);

        void Null();

        void NullOrWhitespace();
    }
}

