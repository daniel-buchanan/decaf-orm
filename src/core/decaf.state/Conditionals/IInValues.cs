using System;
using System.Collections.Generic;

namespace decaf.state.Conditionals;

public interface IInValues
{
    state.Column Column { get; }

    IReadOnlyCollection<object> GetValues();

    int CountValues { get; }

    Type ValueType { get; }
}