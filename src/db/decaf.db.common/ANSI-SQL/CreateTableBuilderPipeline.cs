using System.Linq;
using decaf.common.Templates;
using decaf.db.common.Builders;
using decaf.state;
using decaf.state.Ddl.Definitions;

namespace decaf.db.common.ANSISQL;

public class CreateTableBuilderPipeline : db.common.Builders.CreateTableBuilderPipeline
{
    private const string QuoteFormat = "{0}{1}{0}";
    private readonly ITypeParser typeParser;
    private readonly IValueParser valueParser;
    public CreateTableBuilderPipeline(
        DecafOptions options, 
        IConstants constants, 
        IParameterManager parameterManager, 
        ITypeParser typeParser, 
        IValueParser valueParser) 
        : base(options, constants, parameterManager)
    {
        this.typeParser = typeParser;
        this.valueParser = valueParser;
    }

    protected override void AddCreateTable(IPipelineStageInput<ICreateTableQueryContext> input) 
        => input.Builder.Append("create table {0} as (", input.Context.Name);

    protected override void AddColumns(IPipelineStageInput<ICreateTableQueryContext> input)
    {
        var columns = input.Context.Columns.ToArray();
        for (var i = 0; i < columns.Length; i++)
        {
            var isLast = i == columns.Length - 1;
            var c = columns[i];
            var type = typeParser.Parse(c.Type);
            var name = valueParser.ValueNeedsQuoting(c.Name)
                ? string.Format("{0}{1}{0}", Constants.ColumnQuote, c.Name)
                : c.Name;
            var notNull = c.Nullable
                ? "NULL"
                : "NOT NULL";
            var comma = isLast
                ? string.Empty
                : ",";
            
            input.Builder.AppendLine("{0} as {1} {2}{3}", name, type, notNull, comma);
        }
    }

    protected override void AddIndicies(IPipelineStageInput<ICreateTableQueryContext> input)
    {
        var indexes = input.Context.Indexes.ToArray();
        foreach (var idx in indexes)
        {
            var columns = string.Join(",", idx.Columns.Select(QuoteValue));
            var name = valueParser.ValueNeedsQuoting(idx.Name)
                ? string.Format(QuoteFormat, Constants.ColumnQuote, idx.Name)
                : idx.Name;
            var table = valueParser.ValueNeedsQuoting(input.Context.Name)
                ? string.Format(QuoteFormat, Constants.ColumnQuote, input.Context.Name)
                : input.Context.Name;
            
            input.Builder.AppendLine("create index {0} on {1} ({2}){3}", name, table, columns, Constants.EndStatement);
        }
    }

    protected override void AddEndTable(IPipelineStageInput<ICreateTableQueryContext> input)
        => input.Builder.AppendLine(Constants.ClosingParen);

    protected override void AddPrimaryKey(IPipelineStageInput<ICreateTableQueryContext> input)
    {
        var pk = input.Context.PrimaryKey;
        if (pk is null) return;
        
        var table = valueParser.ValueNeedsQuoting(input.Context.Name)
            ? string.Format(QuoteFormat, Constants.ColumnQuote, input.Context.Name)
            : input.Context.Name;
        var name = valueParser.ValueNeedsQuoting(pk.Name)
            ? string.Format(QuoteFormat, Constants.ColumnQuote, pk.Name)
            : pk.Name;
        var columns = string.Join(",", pk.Columns.Select(QuoteValue));
        input.Builder.AppendLine("alter table {0}} constraint {1} primary key ({2}){3}", table, name, columns, Constants.EndStatement);
    }
    
    private string QuoteValue(IColumnDefinition columnDefinition) 
        => !valueParser.ValueNeedsQuoting(columnDefinition.Type) 
            ? columnDefinition.Name 
            : string.Format(QuoteFormat, Constants.ColumnQuote, columnDefinition.Name);
}