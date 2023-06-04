using System;
using System.Linq;
using pdq.common.Exceptions;
using pdq.common.Templates;
using pdq.common.Utilities;
using pdq.db.common;
using pdq.db.common.Builders;
using pdq.Exceptions;
using pdq.state;

namespace pdq.sqlserver.Builders
{
	public class InsertBuilderPipeline : db.common.ANSISQL.InsertBuilderPipeline
	{
        public InsertBuilderPipeline(
            PdqOptions options,
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

