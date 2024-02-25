using decaf.common.Utilities;
using decaf.db.common;
using decaf.db.common.Builders;
using decaf.state;

namespace decaf.tests.common.Mocks
{
    public class MockInsertPipeline : db.common.ANSISQL.InsertBuilderPipeline
    {
        public MockInsertPipeline(
            DecafOptions options,
            IHashProvider hashProvider,
            db.common.Builders.IWhereBuilder whereBuilder,
            IQuotedIdentifierBuilder quotedIdentifierBuilder,
            IValueParser valueParser,
            IBuilderPipeline<ISelectQueryContext> selectBuilder,
            IConstants constants) :
            base(options, hashProvider, whereBuilder, quotedIdentifierBuilder, valueParser, selectBuilder, constants)
        {
        }
    }
}