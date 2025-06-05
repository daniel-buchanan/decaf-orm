using decaf.common.Templates;
using decaf.db.common;
using decaf.db.common.Builders;

namespace decaf.npgsql.Builders;

public class DropTableBuilderPipeline(
    DecafOptions options,
    IConstants constants,
    IParameterManager parameterManager,
    IValueParser valueParser)
    : db.common.ANSISQL.DropTableBuilderPipeline(options, constants, parameterManager, valueParser);