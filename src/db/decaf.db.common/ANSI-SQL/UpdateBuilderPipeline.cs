using System;
using System.Linq;
using decaf.common.Templates;
using decaf.db.common.Builders;
using decaf.state;
using decaf.state.ValueSources.Update;

namespace decaf.db.common.ANSISQL;

public class UpdateBuilderPipeline : db.common.Builders.UpdateBuilderPipeline
{
    protected readonly IWhereClauseBuilder WhereBuilder;
    protected readonly IQuotedIdentifierBuilder QuotedIdentifierBuilder;
    protected readonly IBuilderPipeline<ISelectQueryContext> SelectBuilder;

    protected UpdateBuilderPipeline(
        DecafOptions options,
        IConstants constants,
        IParameterManager parameterManager,
        IWhereClauseBuilder whereBuilder,
        IQuotedIdentifierBuilder quotedIdentifierBuilder,
        IBuilderPipeline<ISelectQueryContext> selectBuilder)
        : base(options, constants, parameterManager)
    {
        WhereBuilder = whereBuilder;
        QuotedIdentifierBuilder = quotedIdentifierBuilder;
        SelectBuilder = selectBuilder;
    }

    private void AppendItems<T>(
        ISqlBuilder sqlBuilder,
        T[] items,
        Action<ISqlBuilder, T> processMethod,
        bool appendNewLine = false)
    {
        var lastItemIndex = items.Length - 1;
        for (var i = 0; i < items.Length; i++)
        {
            var delimiter = string.Empty;
            if (i < lastItemIndex) delimiter = Constants.Seperator;
            processMethod(sqlBuilder, items[i]);
            sqlBuilder.Append(delimiter);

            if(appendNewLine) sqlBuilder.AppendLine();
        }
    }

    protected override void AddOutput(IPipelineStageInput<IUpdateQueryContext> input)
    {
        if (!input.Context.Outputs.Any())
            return;

        input.Builder.AppendLine(Constants.Returning);
        input.Builder.IncreaseIndent();
        var outputs = input.Context.Outputs.ToArray();

        AppendItems(input.Builder, outputs, (_, o) =>
        {
            input.Builder.PrependIndent();
            QuotedIdentifierBuilder.AddOutput(o, input.Builder);
        });

        input.Builder.DecreaseIndent();
        input.Builder.AppendLine();
    }

    protected override void AddTable(IPipelineStageInput<IUpdateQueryContext> input)
    {
        input.Builder.IncreaseIndent();

        input.Builder.PrependIndent();
        QuotedIdentifierBuilder.AddFromTable(input.Context.Table.Name, input.Builder);
        input.Builder.AppendLine();

        input.Builder.DecreaseIndent();
    }

    protected override void AddValues(IPipelineStageInput<IUpdateQueryContext> input)
    {
        input.Builder.AppendLine("set");

        var first = input.Context.Updates?.FirstOrDefault();
        if (first == null || input.Context.Updates?.Any() == false)
            return;

        input.Builder.IncreaseIndent();
        if(first is StaticValueSource)
        {
            var items = input.Context.Updates.Select(i => i as StaticValueSource).ToArray();
            AppendItems(input.Builder, items, (b, i) => {
                var p = input.Parameters.Add(i.Column, i.Value);
                b.PrependIndent();
                b.Append("{0} = {1}", i.Column.Name, p.Name);
            }, true);
        }
        else if (first is QueryValueSource)
        {
            var items = input.Context.Updates.Select(i => i as QueryValueSource).ToArray();
            AppendItems(input.Builder, items, (b, i) => {
                b.PrependIndent();
                QuotedIdentifierBuilder.AddColumn(i.DestinationColumn, b);
                b.Append(" = x.");
                QuotedIdentifierBuilder.AddColumn(i.SourceColumn, b);
            }, true);
        }
        input.Builder.DecreaseIndent();

        if (first is QueryValueSource)
        {
            input.Builder.AppendLine(Constants.From);
            input.Builder.AppendLine(Constants.OpeningParen);
            input.Builder.IncreaseIndent();
            AddValuesFromQuery(input.Context.Source as ISelectQueryTarget, input);
            input.Builder.DecreaseIndent();
            QuotedIdentifierBuilder.AddClosingFromQuery("x", input.Builder);
        }
    }

    private void AddValuesFromQuery(ISelectQueryTarget source, IPipelineStageInput<IUpdateQueryContext> input)
        => SelectBuilder.Execute(source.Context, input);

    protected override void AddWhere(IPipelineStageInput<IUpdateQueryContext> input)
        => WhereBuilder.AddWhere(input.Context.WhereClause, input.Builder, input.Parameters);
}