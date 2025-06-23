using decaf.common.Templates;
using decaf.db.common.Builders;

namespace decaf.sqlserver.Builders;

public class SelectBuilderPipeline : db.common.ANSISQL.SelectBuilderPipeline
{
    protected override bool LimitBeforeGroupBy => true;

    public SelectBuilderPipeline(
        DecafOptions options,
        IConstants constants,
        IParameterManager parameterManager,
        IQuotedIdentifierBuilder quotedIdentifierBuilder,
        IWhereClauseBuilder whereBuilder)
        : base(options, constants, parameterManager, quotedIdentifierBuilder, whereBuilder)
    {
    }
}