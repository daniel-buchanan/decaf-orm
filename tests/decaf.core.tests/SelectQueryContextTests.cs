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
    public class SelectQueryContextTests
	{
		private readonly ISelectQueryContext context;

		public SelectQueryContextTests()
		{
			var aliasManager = AliasManager.Create();
            var hashProvider = new HashProvider();
			this.context = SelectQueryContext.Create(aliasManager, hashProvider);
		}

        [Fact]
        public void QueryTypeIsSelect()
        {
            // Act
            var queryType = this.context.Kind;

            // Assert
            queryType.Should().Be(QueryTypes.Select);
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
        [MemberData(nameof(ColumnTests))]
		public void AddColumnSucceeds(Column column)
        {
			// Act
			this.context.Select(column);

			// Assert
			this.context.Columns.Count.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(ColumnTests))]
        public void AddExistingColumnDoesNothing(Column column)
        {
            // Arrange
            this.context.Select(column);

            // Act
            this.context.Select(column);

            // Assert
            this.context.Columns.Count.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(TableTests))]
        public void SelectFromSucceeds(ITableTarget target)
        {
            // Act
            this.context.From(target);

            // Assert
            this.context.QueryTargets.Count.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(TableTests))]
        public void SelectFromSameDoesNothing(ITableTarget target)
        {
            // Arrange
            this.context.From(target);

            // Act
            this.context.From(target);

            // Assert
            this.context.QueryTargets.Count.Should().Be(1);
        }

        [Fact]
        public void AddGroupBySucceeds()
        {
            // Arrange
            var group = GroupBy.Create("name", TableTarget.Create("users", "u"));

            // Act
            this.context.GroupBy(group);

            // Assert
            this.context.GroupByClauses.Count.Should().Be(1);
        }

        [Fact]
        public void AddSameGroupByFails()
        {
            // Arrange
            var group = GroupBy.Create("name", TableTarget.Create("users", "u"));

            // Act
            this.context.GroupBy(group);
            this.context.GroupBy(group);

            // Assert
            this.context.GroupByClauses.Count.Should().Be(1);
        }

        [Fact]
        public void AddOrderBySucceeds()
        {
            // Arrange
            var order = OrderBy.Create("name", TableTarget.Create("users", "u"), SortOrder.Ascending);

            // Act
            this.context.OrderBy(order);

            // Assert
            this.context.OrderByClauses.Count.Should().Be(1);
        }

        [Fact]
        public void AddSameOrderByFails()
        {
            // Arrange
            var order = OrderBy.Create("name", TableTarget.Create("users", "u"), SortOrder.Ascending);

            // Act
            this.context.OrderBy(order);
            this.context.OrderBy(order);

            // Assert
            this.context.OrderByClauses.Count.Should().Be(1);
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

        [Theory]
        [MemberData(nameof(JoinTests))]
        public void AddJoinSucceeds(Join join)
        {
            // Act
            this.context.Join(join);

            // Assert
            this.context.Joins.Count.Should().Be(1);
        }

        [Fact]
        public void AddJoinWithSameTargetsAndAliasFails()
        {
            // Arrange
            var from = TableTarget.Create("users", "u");
            var to = TableTarget.Create("accounts", "a");
            var join = Join.Create(from, to, JoinType.Default, null);

            // Act
            this.context.Join(join);
            this.context.Join(join);

            // Assert
            this.context.Joins.Count.Should().Be(1);
        }

        [Fact]
        public void DisposeClearsJoins()
        {
            // Arrange
            var from = TableTarget.Create("users", "u");
            var to = TableTarget.Create("accounts", "a");
            var join = Join.Create(from, to, JoinType.Default, null);

            this.context.Join(join);

            // Act
            this.context.Dispose();

            // Assert
            this.context.Joins.Count.Should().Be(0);
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

        [Fact]
        public void DisposeClearsOrderBy()
        {
            // Arrange
            var order = OrderBy.Create("name", TableTarget.Create("users"), SortOrder.Ascending);
            this.context.OrderBy(order);

            // Act
            this.context.Dispose();

            // Assert
            this.context.OrderByClauses.Count.Should().Be(0);
        }

        [Fact]
        public void DisposeClearsGroupBy()
        {
            // Arrange
            var group = GroupBy.Create("name", TableTarget.Create("users"));
            this.context.GroupBy(group);

            // Act
            this.context.Dispose();

            // Assert
            this.context.GroupByClauses.Count.Should().Be(0);
        }

        public static IEnumerable<object[]> ColumnTests
        {
			get
            {
				yield return new[] { Column.Create("name", "users", "u") };
                yield return new[] { Column.Create("name", TableTarget.Create("users", "u")) };
                yield return new[] { Column.Create("name", TableTarget.Create("users", "u"), "username") };
            }
        }

        public static IEnumerable<object[]> TableTests
        {
            get
            {
                yield return new[] { TableTarget.Create("users", "u") };
                yield return new[] { TableTarget.Create("users", "public") };
            }
        }

        public static IEnumerable<object[]> JoinTests
        {
            get
            {
                yield return new[] { Join.Create(null, null, JoinType.Default, null) };
            }
        }
    }
}

