using System;
namespace pdq
{
    public interface IColumnMatchBuilder
    {
        void Column(string name, string targetAlias = null);
    }
}

