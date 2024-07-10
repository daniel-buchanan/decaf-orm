using System;
using System.Collections.Generic;
using decaf.common;
using decaf.common.Utilities;
using decaf.state;
using decaf.state.QueryTargets;
using FluentAssertions;
using Xunit;

namespace decaf.core_tests
{
    public class DeleteQueryContextTests
	{
		private readonly IDeleteQueryContext context;

		public DeleteQueryContextTests()
		{
			var aliasManager = AliasManager.Create();
            var hashProvider = new HashProvider();
			context = DeleteQueryContext.Create(aliasManager, hashProvider);
		}

        [Fact]
        public void QueryTypeIsDelete()
        {
            // Act
            var queryType = context.Kind;

            // Assert
            queryType.Should().Be(QueryTypes.Delete);
        }

        [Fact]
        public void IdIsCreatedAndNotEmpty()
        {
            // Act
            var id = context.Id;

            // Assert
            id.Should().NotBe(Guid.Empty);
        }

        [Theory]
        [MemberData(nameof(TableTests))]
        public void DeleteFromSucceeds(ITableTarget target)
        {
            // Act
            context.From(target);

            // Assert
            context.QueryTargets.Count.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(TableTests))]
        public void DeleteFromSameDoesNothing(ITableTarget target)
        {
            // Arrange
            context.From(target);

            // Act
            context.From(target);

            // Assert
            context.QueryTargets.Count.Should().Be(1);
        }

        [Fact]
        public void WhereClauseSucceeds()
        {
            // Arrange
            var column = Column.Create("name", TableTarget.Create("users"));
            var clause = state.Conditionals.Column.Equals(column, 42);

            // Act
            context.Where(clause);

            // Assert
            context.WhereClause.Should().NotBeNull();
        }

        [Fact]
        public void DisposeClearsQueryTargets()
        {
            // Arrange
            var from = TableTarget.Create("users", "u");

            context.From(from);

            // Act
            context.Dispose();

            // Assert
            context.QueryTargets.Count.Should().Be(0);
        }

        [Fact]
        public void DisposeClearsWhereClause()
        {
            // Arrange
            var column = Column.Create("name", TableTarget.Create("users"));
            var clause = state.Conditionals.Column.Equals(column, 42);
            context.Where(clause);

            // Act
            context.Dispose();

            // Assert
            context.WhereClause.Should().BeNull();
        }

        public static IEnumerable<object[]> TableTests
        {
            get
            {
                yield return new[] { TableTarget.Create("users", "u") };
                yield return new[] { TableTarget.Create("users", "public") };
            }
        }
    }
}

