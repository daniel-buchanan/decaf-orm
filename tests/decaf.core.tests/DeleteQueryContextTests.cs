using System;
using System.Collections.Generic;
using System.Linq;
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
			this.context = DeleteQueryContext.Create(aliasManager, hashProvider);
		}

        [Fact]
        public void QueryTypeIsDelete()
        {
            // Act
            var queryType = this.context.Kind;

            // Assert
            queryType.Should().Be(QueryTypes.Delete);
        }

        [Fact]
        public void IdIsCreatedAndNotEmpty()
        {
            // Act
            var id = this.context.Id;

            // Assert
            id.Should().NotBe(Guid.Empty);
        }

        [Theory]
        [MemberData(nameof(TableTests))]
        public void DeleteFromSucceeds(ITableTarget target)
        {
            // Act
            this.context.From(target);

            // Assert
            this.context.QueryTargets.Count.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(TableTests))]
        public void DeleteFromSameDoesNothing(ITableTarget target)
        {
            // Arrange
            this.context.From(target);

            // Act
            this.context.From(target);

            // Assert
            this.context.QueryTargets.Count.Should().Be(1);
        }

        [Fact]
        public void WhereClauseSucceeds()
        {
            // Arrange
            var column = Column.Create("name", TableTarget.Create("users"));
            var clause = state.Conditionals.Column.Equals(column, 42);

            // Act
            this.context.Where(clause);

            // Assert
            this.context.WhereClause.Should().NotBeNull();
        }

        [Fact]
        public void DisposeClearsQueryTargets()
        {
            // Arrange
            var from = TableTarget.Create("users", "u");

            this.context.From(from);

            // Act
            this.context.Dispose();

            // Assert
            this.context.QueryTargets.Count.Should().Be(0);
        }

        [Fact]
        public void DisposeClearsWhereClause()
        {
            // Arrange
            var column = Column.Create("name", TableTarget.Create("users"));
            var clause = state.Conditionals.Column.Equals(column, 42);
            this.context.Where(clause);

            // Act
            this.context.Dispose();

            // Assert
            this.context.WhereClause.Should().BeNull();
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

