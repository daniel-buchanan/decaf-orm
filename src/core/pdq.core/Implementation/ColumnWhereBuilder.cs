namespace pdq.Implementation
{
    public class ColumnWhereBuilder : IColumnWhereBuilder
    {
        private readonly IWhereBuilder builder;
        private readonly state.Column column;

        private ColumnWhereBuilder(
            IWhereBuilder builder,
            state.Column column)
        {
            this.builder = builder;
            this.column = column;
        }

        internal static IColumnWhereBuilder Create(
            IWhereBuilder builder,
            state.Column column)
            => new ColumnWhereBuilder(builder, column);

        /// <inheritdoc />
        public IColumnValueBuilder Is() => ColumnValueBuilder.Create(builder, column, false);

        /// <inheritdoc />
        public IColumnValueBuilder IsNot() => ColumnValueBuilder.Create(builder, column, true);
    }
}

