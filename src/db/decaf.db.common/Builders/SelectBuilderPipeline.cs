using decaf.common.Templates;
using decaf.state;

namespace decaf.db.common.Builders;

public abstract class SelectBuilderPipeline : BuilderPipeline<ISelectQueryContext>
{
    private readonly IWhereClauseBuilder whereBuilder;

    protected SelectBuilderPipeline(
        DecafOptions options,
        IConstants constants,
        IParameterManager parameterManager,
        IWhereClauseBuilder whereBuilder)
        : base(options, constants, parameterManager)
    {
        this.whereBuilder = whereBuilder;

        Add(AddSelect, providesParameters: false);
        Add(AddLimit, providesParameters: true, condition: LimitBeforeGroupBy);
        Add(AddColumns, providesParameters: false);
        Add(AddTables, providesParameters: false);
        Add(AddJoins, providesParameters: true);
        Add(AddWhere, providesParameters: true);
        Add(AddOrderBy, providesParameters: false);
        Add(AddGroupBy, providesParameters: false);
        Add(AddLimit, providesParameters: true, condition: LimitBeforeGroupBy);
    }

    /// <summary>
    /// Whether the a limit (if specified) should be before or after the group by clause.
    /// </summary>
    protected abstract bool LimitBeforeGroupBy { get; }

    private void AddSelect(IPipelineStageInput<ISelectQueryContext> input)
        => input.Builder.AppendLine(Builders.Constants.Select);

    /// <summary>
    /// Add any selected columns to the query.
    /// </summary>
    /// <param name="input">The input for the pipeline stage.</param>
    protected abstract void AddColumns(IPipelineStageInput<ISelectQueryContext> input);

    /// <summary>
    /// Add any selected tables to the query.
    /// </summary>
    /// <param name="input">The input for the pipeline stage.</param>
    protected abstract void AddTables(IPipelineStageInput<ISelectQueryContext> input);

    /// <summary>
    /// Add any joins for the query.
    /// </summary>
    /// <param name="input">The input for the pipeline stage.</param>
    protected abstract void AddJoins(IPipelineStageInput<ISelectQueryContext> input);

    /// <summary>
    /// Add order to the query, if specified.
    /// </summary>
    /// <param name="input">The input for the pipeline stage.</param>
    protected abstract void AddOrderBy(IPipelineStageInput<ISelectQueryContext> input);

    /// <summary>
    /// Add any groups to the query, if specified.
    /// </summary>
    /// <param name="input">The input for the pipeline stage.</param>
    protected abstract void AddGroupBy(IPipelineStageInput<ISelectQueryContext> input);

    /// <summary>
    /// Add a limit to the query.
    /// </summary>
    /// <param name="input">The input for the pipeline stage.</param>
    protected abstract void AddLimit(IPipelineStageInput<ISelectQueryContext> input);

    private void AddWhere(IPipelineStageInput<ISelectQueryContext> input)
        => whereBuilder.AddWhere(input.Context.WhereClause, input.Builder, input.Parameters);

}