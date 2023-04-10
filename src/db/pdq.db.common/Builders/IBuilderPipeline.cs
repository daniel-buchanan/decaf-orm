using System;
using System.Collections.Generic;
using pdq.common;
using pdq.common.Templates;

namespace pdq.db.common.Builders
{
	public interface IPipelineStageInput
	{
		/// <summary>
		/// The <see cref="IParameterManager"/> used for building a query.
		/// </summary>
        IParameterManager Parameters { get; }

		/// <summary>
		/// The <see cref="ISqlBuilder"/> used to build the query.
		/// </summary>
        ISqlBuilder Builder { get; }
    }

	public interface IPipelineStageInput<out T>
		: IPipelineStageInput
		where T : IQueryContext
	{
		/// <summary>
		/// The <see cref="T"/> context for the pipeline stage.
		/// </summary>
		T Context { get; }
	}

	public interface IBuilderPipeline<T>
		where T : IQueryContext
	{
		/// <summary>
		/// Generate a SQL template based on the provided context.
		/// </summary>
		/// <param name="context">The context to parse.</param>
		/// <returns>A SQL Template, one is created if it doesn't already exist.</returns>
		SqlTemplate Execute(T context);

        /// <summary>
        /// Generate a SQL template based on the provided context and <see cref="IPipelineStageInput"/> input.
        /// </summary>
        /// <param name="context">The context to parse.</param>
        /// <param name="input">The <see cref="IPipelineStageInput"/> to use.</param>
        /// <returns>A SQL Template, one is created if it doesn't already exist.</returns>
        SqlTemplate Execute(T context, IPipelineStageInput input);

		/// <summary>
		/// Get the Parameter values from the context as a dictionary.
		/// </summary>
		/// <param name="context">The context to parse.</param>
		/// <returns>The parameters from the context, as an <see cref="IDictionary{string, object}"/></returns>
        IDictionary<string, object> GetParameterValues(T context);
	}
}

