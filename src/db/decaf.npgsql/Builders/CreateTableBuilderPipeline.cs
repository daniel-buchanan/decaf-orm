using decaf.common.Templates;
using decaf.db.common;
using decaf.db.common.Builders;

namespace decaf.npgsql.Builders;

public class CreateTableBuilderPipeline(
    DecafOptions options, 
    IConstants constants, 
    IParameterManager parameterManager, 
    ITypeParser typeParser, 
    IValueParser valueParser) : 
    db.common.ANSISQL.CreateTableBuilderPipeline(options, constants, parameterManager, typeParser, valueParser) { }