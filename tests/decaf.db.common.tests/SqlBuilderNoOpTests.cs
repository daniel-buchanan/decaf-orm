using System;
using decaf.db.common.Builders;
using FluentAssertions;
using Xunit;

namespace decaf.db.common.tests
{
	public class SqlBuilderNoOpTests
	{
		private readonly SqlBuilder sqlBuilder;

		public SqlBuilderNoOpTests()
		{
			sqlBuilder = SqlBuilder.CreateNoOp() as SqlBuilder;
		}

		[Fact]
		public void LineEndingMatchesEnvironment()
		{
			// Arrange

			// Act
			var ending = sqlBuilder.LineEnding;

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
                sqlBuilder.IncreaseIndent();

            sqlBuilder.PrependIndent();
            var sql = sqlBuilder.GetSql();

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
                sqlBuilder.IncreaseIndent();


            for (var i = 0; i < indentLevel; i++)
                sqlBuilder.DecreaseIndent();

            sqlBuilder.PrependIndent();
            var sql = sqlBuilder.GetSql();

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
            sqlBuilder.Append(prefix);
            sqlBuilder.Append(toAppend);
            var sql = sqlBuilder.GetSql();

            // Assert
            sql.Should().Be(string.Empty);
        }
    }
}

