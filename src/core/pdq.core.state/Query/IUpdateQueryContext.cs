using System;
using System.Collections.Generic;
using pdq.common;

namespace pdq.state
{
    public interface IUpdateQueryContext : IQueryContext
    {
		/// <summary>
        /// 
        /// </summary>
		ITableTarget Table { get; }

		/// <summary>
        /// 
        /// </summary>
		IReadOnlyCollection<IUpdateValueSource> Updates { get; }

		/// <summary>
        /// 
        /// </summary>
		IQueryTarget Source { get; }

		/// <summary>
        /// 
        /// </summary>
		IWhere WhereClause { get; }

		/// <summary>
        /// 
        /// </summary>
		IReadOnlyCollection<Output> Outputs { get; }

		/// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
		void Update(ITableTarget target);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
		void From(IQueryTarget source);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
		void Where(IWhere where);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
		void Output(Output output);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
		void Set(IUpdateValueSource value);
    }
}

