using System;

namespace pdq
{
	public interface ISelect : IDisposable
	{
		/// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="alias"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
		ISelectFrom From(string table, string alias, string schema = null);

		/// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
		ISelectFrom From(Action<ISelect> query, string alias);
	}

	public interface ISelectWithAlias : ISelect
    {
        /// <summary>
        /// 
        /// </summary>
		string Alias { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
		ISelectWithAlias KnownAs(string alias);
    }
}

