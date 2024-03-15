using System;
using System.Linq;
using decaf.common.Utilities;
using decaf.db.common;
using decaf.db.common.Builders;
using decaf.state;
using decaf.common.Exceptions;
using decaf.common.Templates;
using decaf.Exceptions;

namespace decaf.sqlserver.Builders
{
	public class InsertBuilderPipeline : db.common.ANSISQL.InsertBuilderPipeline
	{
        public InsertBuilderPipeline(
            DecafOptions options,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IValueParser valueParser,
            IBuilderPipeline<ISelectQueryContext> selectBuilder,
            IConstants constants)
            : base(options, hashProvider, whereBuilder, quotedIdentifierBuilder, valueParser, selectBuilder, constants)
        {
        }
    }
}

