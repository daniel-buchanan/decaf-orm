using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using pdq.common;
using pdq.state;
using pdq.state.QueryTargets;
using Xunit;

namespace pdq.core_tests
{
	public class InsertQueryContextTests
	{
		private readonly IInsertQueryContext context;

		public InsertQueryContextTests()
		{
			var aliasManager = AliasManager.Create();
			this.context = InsertQueryContext.Create(aliasManager);
		}

        [Fact]
        public void QueryTypeIsInsert()
        {
            // Act
            var queryType = this.context.Kind;

            // Assert
            queryType.Should().Be(QueryTypes.Insert);
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
			this.context.Column(column);

			// Assert
			this.context.Columns.Count.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(ColumnTests))]
        public void AddExistingColumnDoesNothing(Column column)
        {
            // Arrange
            this.context.Column(column);

            // Act
            this.context.Column(column);

            // Assert
            this.context.Columns.Count.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(TableTests))]
        public void InsertIntoSucceeds(ITableTarget target)
        {
            // Act
            this.context.Into(target);

            // Assert
            this.context.QueryTargets.Count.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(TableTests))]
        public void InsertIntoSameDoesNothing(ITableTarget target)
        {
            // Arrange
            this.context.Into(target);

            // Act
            this.context.Into(target);

            // Assert
            this.context.QueryTargets.Count.Should().Be(1);
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
    }
}

