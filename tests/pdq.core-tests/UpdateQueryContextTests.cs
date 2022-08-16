using System;
using FluentAssertions;
using pdq.common;
using pdq.state;
using pdq.state.QueryTargets;
using Xunit;

namespace pdq.core_tests
{
    public class UpdateQueryContextTests
    {
        private readonly IUpdateQueryContext context;

        public UpdateQueryContextTests()
        {
            var aliasManager = AliasManager.Create();
            this.context = UpdateQueryContext.Create(aliasManager);
        }

        [Fact]
        public void QueryTypeIsUpdate()
        {
            // Act
            var queryType = this.context.Kind;

            // Assert
            queryType.Should().Be(QueryTypes.Update);
        }

        [Fact]
        public void IdIsCreatedAndNotEmpty()
        {
            // Act
            var id = this.context.Id;

            // Assert
            id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void UpdateSetsTableCorrectly()
        {
            // Arrange
            var target = TableTarget.Create("users", "u");

            // Act
            this.context.Update(target);

            // Assert
            this.context.Table.Should().NotBeNull();
            this.context.Table.Should().Be(target);
        }

        [Fact]
        public void FromSetsSourceCorrectly()
        {
            // Arrange
            var target = SelectQueryTarget.Create(null, "a");

            // Act
            this.context.From(target);

            // Assert
            this.context.Source.Should().NotBeNull();
            this.context.Source.Should().Be(target);
        }

        [Fact]
        public void SetSucceeds()
        {
            // Arrange
            var column = Column.Create("email", "users", "a");
            var value = state.ValueSources.Update.StaticValueSource.Create(column, typeof(string), "bob@bob.com");

            // Act
            this.context.Set(value);

            // Assert
            this.context.Updates.Should().HaveCount(1);
        }

        [Fact]
        public void OutputSucceeds()
        {
            // Arrange
            var column = Column.Create("email", "users", "a");
            var output = Output.Create(column, OutputSources.Updated);

            // Act
            this.context.Output(output);

            // Assert
            this.context.Outputs.Should().HaveCount(1);
        }

        [Fact]
        public void WhereClauseSucceeds()
        {
            // Arrange
            var column = Column.Create("id", TableTarget.Create("users", "u"));
            var clause = state.Conditionals.Column.Equals(column, 42);

            // Act
            this.context.Where(clause);

            // Assert
            this.context.WhereClause.Should().NotBeNull();
        }
    }
}

