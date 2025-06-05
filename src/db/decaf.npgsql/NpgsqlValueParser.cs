using decaf.common.Utilities.Reflection;
using decaf.db.common.ANSISQL;
using decaf.db.common.Builders;

namespace decaf.npgsql;

public class NpgsqlValueParser(IReflectionHelper reflectionHelper, IConstants constants)
    : SqlValueParser(reflectionHelper, constants);