﻿using decaf.common.Templates;
using decaf.state;

namespace decaf.db.common.Builders;

public abstract class DeleteBuilderPipeline : BuilderPipeline<IDeleteQueryContext>
{
    private readonly IWhereClauseBuilder whereBuilder;

    protected DeleteBuilderPipeline(
        DecafOptions options,
        IConstants constants,
        IParameterManager parameterManager,
        IWhereClauseBuilder whereBuilder)
        : base(options, constants, parameterManager)
    {
        this.whereBuilder = whereBuilder;

        Add(AddDelete, providesParameters: false);
        Add(AddTables, providesParameters: false);
        Add(AddWhere, providesParameters: true);
        Add(AddOutput, providesParameters: false);
    }

    private static void AddDelete(IPipelineStageInput<IDeleteQueryContext> input)
        => input.Builder.AppendLine("{0} from", Builders.Constants.Delete);

    protected abstract void AddTables(IPipelineStageInput<IDeleteQueryContext> input);

    protected abstract void AddOutput(IPipelineStageInput<IDeleteQueryContext> input);

    private void AddWhere(IPipelineStageInput<IDeleteQueryContext> input)
        => whereBuilder.AddWhere(input.Context.WhereClause, input.Builder, input.Parameters);
}