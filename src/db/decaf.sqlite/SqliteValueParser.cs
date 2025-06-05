using decaf.common.Utilities.Reflection;
using decaf.db.common.ANSISQL;
using decaf.db.common.Builders;

namespace decaf.sqlite;

public class SqliteValueParser(IReflectionHelper reflectionHelper, IConstants constants)
    : SqlValueParser(reflectionHelper, constants);