using decaf.common.Templates;
using decaf.db.common;
using decaf.db.common.Builders;
using decaf.state;
using decaf.state.Ddl.Definitions;

namespace decaf.sqlite.Builders;

public class CreateTableBuilderPipeline : db.common.ANSISQL.CreateTableBuilderPipeline
{
    private readonly ITypeParser typeParser;
    
    public CreateTableBuilderPipeline(
        DecafOptions options, 
        IConstants constants, 
        IParameterManager parameterManager,
        ITypeParser typeParser,
        IValueParser valueParser) : 
        base(options, constants, parameterManager, typeParser, valueParser) 
        => this.typeParser = typeParser;

    protected override void AddCreateTable(IPipelineStageInput<ICreateTableQueryContext> input)
        => input.Builder.AppendLine("create table {0} {1}", input.Context.Name, Constants.OpeningParen);

    protected override string FormatColumn(IColumnDefinition column)
    {
        var type = typeParser.Parse(column.Type);
        var name = ValueParser.QuoteIfNecessary(column.Name);
        var notNull = column.Nullable
            ? "null"
            : "not null";
        return $"{name} {type} {notNull}";
    }
    // protected override void AddColumns(IPipelineStageInput<ICreateTableQueryContext> input)
    // {
    //     input.Builder.IncreaseIndent();
    //     var items = input.Context.Columns.ToArray();
    //     var primaryKey = input.Context.PrimaryKey;
    //     IColumnDefinition? pk = null;
    //     if (primaryKey?.Columns.Count() == 1)
    //         pk = primaryKey.Columns.First();
    //     
    //     var lastItemIndex = items.Length - 1;
    //     for (var i = 0; i < items.Length; i++)
    //     {
    //         var delimiter = string.Empty;
    //         var type = typeParser.Parse(items[i].Type);
    //         if (items[i].Type == typeof(int) &&
    //             items[i].Name == pk?.Name)
    //             type += " primary key";
    //         
    //         if (i < lastItemIndex) delimiter = Constants.Seperator;
    //         input.Builder.AppendLine("{0} {1} {2}{3}", 
    //             items[i].Name, 
    //             type, 
    //             items[i].Nullable 
    //                 ? "null" 
    //                 : "not null",
    //             delimiter);
    //     }
    //     input.Builder.DecreaseIndent();
    // }

    // protected override void AddIndicies(IPipelineStageInput<ICreateTableQueryContext> input)
    // {
    //     
    // }

    // protected override void AddEndTable(IPipelineStageInput<ICreateTableQueryContext> input) 
    //     => input.Builder.AppendLine("{0}{1}", Constants.ClosingParen, Constants.EndStatement);

    // protected override void AddPrimaryKey(IPipelineStageInput<ICreateTableQueryContext> input)
    // {
    //     
    // }
}