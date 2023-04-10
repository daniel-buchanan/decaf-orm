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
		SqlTemplate Execute(T context);

        SqlTemplate Execute(T context, IPipelineStageInput input);

        IDictionary<string, object> GetParameterValues(T context);
	}
}

