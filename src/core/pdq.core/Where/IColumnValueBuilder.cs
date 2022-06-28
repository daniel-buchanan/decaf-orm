using System;
namespace pdq
{
    public interface IColumnValueBuilder
    {
        void EqualTo<T>(T value);

        void Like<T>(T value);
        void StartsWith<T>(T value);
        void EndsWith<T>(T value);

        void LessThan(int value);
        void LessThan(uint value);
        void LessThan(short value);
        void LessThan(double value);
        void LessThan(long value);
        void LessThan(DateTime value);

        void LessThanOrEqualTo(int value);
        void LessThanOrEqualTo(uint value);
        void LessThanOrEqualTo(short value);
        void LessThanOrEqualTo(double value);
        void LessThanOrEqualTo(long value);
        void LessThanOrEqualTo(DateTime value);

        void GreaterThan(int value);
        void GreaterThan(uint value);
        void GreaterThan(short value);
        void GreaterThan(double value);
        void GreaterThan(long value);
        void GreaterThan(DateTime value);

        void GreaterThanOrEqualTo(int value);
        void GreaterThanOrEqualTo(uint value);
        void GreaterThanOrEqualTo(short value);
        void GreaterThanOrEqualTo(double value);
        void GreaterThanOrEqualTo(long value);
        void GreaterThanOrEqualTo(DateTime value);

        void IsBetween<T>(T start, T end);
    }
}

