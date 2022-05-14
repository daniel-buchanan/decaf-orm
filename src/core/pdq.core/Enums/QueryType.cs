using System;
namespace pdq.core.Enums
{
    [Flags]
	public enum QueryType
	{
        None,
        Select,
        Insert,
        Update,
        Delete,
        StoredProcedure,
        Function
    }
}

