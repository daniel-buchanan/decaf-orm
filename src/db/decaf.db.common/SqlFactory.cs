using System.Collections.Generic;
using decaf.common;
using decaf.common.Templates;
using decaf.db.common.Builders;
using decaf.db.common.Exceptions;
using decaf.state;

namespace decaf.db.common;

public class SqlFactory(
    IBuilderPipeline<ISelectQueryContext> selectBuilder,
    IBuilderPipeline<IDeleteQueryContext> deleteBuilder,
    IBuilderPipeline<IInsertQueryContext> insertBuilder,
    IBuilderPipeline<IUpdateQueryContext> updateBuilder,
    IBuilderPipeline<ICreateTableQueryContext> createTableBuilder = null,
    IBuilderPipeline<IDropTableQueryContext> dropTableBuilder = null)
    : state.SqlFactory
{
    /// <inheritdoc/>
    protected override IDictionary<string, object> ParseParameters(IInsertQueryContext context, bool includePrefix)
        => ParseParameters(insertBuilder, context, includePrefix, KnownFeatures.Insert);

    /// <inheritdoc/>
    protected override IDictionary<string, object> ParseParameters(IDeleteQueryContext context, bool includePrefix)
        => ParseParameters(deleteBuilder, context, includePrefix, KnownFeatures.Delete);

    /// <inheritdoc/>
    protected override IDictionary<string, object> ParseParameters(IUpdateQueryContext context, bool includePrefix)
        => ParseParameters(updateBuilder, context, includePrefix, KnownFeatures.Update);

    /// <inheritdoc/>
    protected override IDictionary<string, object> ParseParameters(ISelectQueryContext context, bool includePrefix)
        => ParseParameters(selectBuilder, context, includePrefix, KnownFeatures.Select);

    /// <inheritdoc/>
    protected override SqlTemplate ParseQuery(IInsertQueryContext context)
        => ParseQuery(insertBuilder, context, KnownFeatures.Insert);

    /// <inheritdoc/>
    protected override SqlTemplate ParseQuery(IDeleteQueryContext context)
        => ParseQuery(deleteBuilder, context, KnownFeatures.Delete);

    /// <inheritdoc/>
    protected override SqlTemplate ParseQuery(IUpdateQueryContext context)
        => ParseQuery(updateBuilder, context, KnownFeatures.Update);

    /// <inheritdoc/>
    protected override SqlTemplate ParseQuery(ISelectQueryContext context)
        => ParseQuery(selectBuilder, context, KnownFeatures.Select);

    /// <inheritdoc/>
    protected override SqlTemplate ParseQuery(ICreateTableQueryContext context)
        => ParseQuery(createTableBuilder, context, KnownFeatures.CreateTable);
        
    /// <inheritdoc />
    protected override SqlTemplate ParseQuery(IDropTableQueryContext context)
        => ParseQuery(dropTableBuilder, context, KnownFeatures.DropTable);

    private static IDictionary<string, object> ParseParameters<TInput, TBuilder>(TBuilder builder, TInput input, bool includePrefix, string feature)
        where TBuilder: IBuilderPipeline<TInput> 
        where TInput : IQueryContext
    {
        if (builder is null) throw new FeatureNotImplementedException(GetFeatureNotImplementedExceptionMessage<TBuilder>(feature));
        return builder.GetParameterValues(input, includePrefix);
    }
        
    private static SqlTemplate ParseQuery<TInput, TBuilder>(TBuilder builder, TInput input, string feature)
        where TBuilder: IBuilderPipeline<TInput> 
        where TInput : IQueryContext
    {
        if (builder is null) throw new FeatureNotImplementedException(GetFeatureNotImplementedExceptionMessage<TBuilder>(feature));
        return builder.Execute(input);
    }

    private static string GetFeatureNotImplementedExceptionMessage<TBuilder>(string feature)
        => $"Feature {feature} is not implemented in {typeof(TBuilder).Assembly.FullName}";
}