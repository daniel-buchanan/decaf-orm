using System;
using FluentAssertions;
using pdq.db.common.Builders;
using Xunit;

namespace pdq.db.common.tests
{
	public class SqlBuilderNoOpTests
	{
		private readonly ISqlBuilder sqlBuilder;

		public SqlBuilderNoOpTests()
		{
			this.sqlBuilder = SqlBuilder.CreateNoOp();
		}

		[Fact]
		public void LineEndingMatchesEnvironment()
		{
			// Arrange

			// Act
			var ending = this.sqlBuilder.LineEnding;

			// Assert
			ending.Should().Be(Environment.NewLine);
		}

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void IndentMultiplesAreCorrectIncreasing(int indentLevel)
        {
			// Arrange

            // Act
            for (var i = 0; i < indentLevel; i++)
                this.sqlBuilder.IncreaseIndent();

            this.sqlBuilder.PrependIndent();
            var sql = this.sqlBuilder.GetSql();

            // Assert
            sql.Should().HaveLength(0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void IndentMultiplesAreCorrectDecreasing(int indentLevel)
        {
            // Arrange

            // Act
            for (var i = 0; i < indentLevel; i++)
                this.sqlBuilder.IncreaseIndent();


            for (var i = 0; i < indentLevel; i++)
                this.sqlBuilder.DecreaseIndent();

            this.sqlBuilder.PrependIndent();
            var sql = this.sqlBuilder.GetSql();

            // Assert
            sql.Should().HaveLength(0);
        }

        [Theory]
        [InlineData("world")]
        [InlineData("universe")]
        [InlineData("galaxy")]
        [InlineData("the brown fox jumps over the wall")]
        public void AppendSucceeds(string toAppend)
        {
            // Arrange
            var prefix = "hello ";

            // Act
            this.sqlBuilder.Append(prefix);
            this.sqlBuilder.Append(toAppend);
            var sql = this.sqlBuilder.GetSql();

            // Assert
            sql.Should().Be(string.Empty);
        }
    }
}

