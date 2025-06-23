using decaf.common.Templates;
using decaf.db.common.Builders;

namespace decaf.npgsql.Builders;

public class DeleteBuilderPipeline : db.common.ANSISQL.DeleteBuilderPipeline
{
    public DeleteBuilderPipeline(
        DecafOptions options,
        IConstants constants,
        IParameterManager parameterManager,
        IWhereClauseBuilder whereBuilder,
        IQuotedIdentifierBuilder quotedIdentifierBuilder)
        : base(options, constants, parameterManager, whereBuilder, quotedIdentifierBuilder)
    {
            
    }
}