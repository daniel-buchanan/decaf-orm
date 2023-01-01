using System;
using Xunit;
using pdq.state.Conditionals;
using pdq.state.QueryTargets;
using FluentAssertions;
using pdq.common.Utilities;
using pdq.common.Templates;

namespace pdq.db.common.tests
{
	public class ParameterManagerTests
	{
		private readonly IParameterManager parameterManager;

		public ParameterManagerTests()
		{
            var hashProvider = new HashProvider();
			this.parameterManager = new ParameterManager(hashProvider);
		}

		[Fact]
		public void AddParameterSucceeds()
		{
			// Arrange
			var source = TableTarget.Create("users", "u");
			var state = Column.Equals<int>(pdq.state.Column.Create("id", source), 42);

			// Act
			Action method = () => this.parameterManager.Add(state, 42);

			// Assert
			method.Should().NotThrow();
		}

        [Fact]
        public void AddSameParameterDoesNotResultInDuplicate()
        {
            // Arrange
            var source = TableTarget.Create("users", "u");
            var state = Column.Equals<int>(pdq.state.Column.Create("id", source), 42);

            // Act
            this.parameterManager.Add(state, 42);
            this.parameterManager.Add(state, 42);

			// Assert
			this.parameterManager.GetParameters().Should().HaveCount(1);
        }

        [Fact]
        public void AddParameterCreatesName()
        {
            // Arrange
            var source = TableTarget.Create("users", "u");
            var state = Column.Equals<int>(pdq.state.Column.Create("id", source), 42);

            // Act
            this.parameterManager.Add(state, 42);

            // Assert
            this.parameterManager.GetParameters().Should().Satisfy(p => p.Name == "@p1");
        }

        [Fact]
        public void AddMultipleParametersCreatesIncreasingName()
        {
            // Arrange
            var source = TableTarget.Create("users", "u");
            var state1 = Column.Equals<int>(pdq.state.Column.Create("id", source), 42);
            var state2 = Column.Equals<int>(pdq.state.Column.Create("member_id", source), 63);

            // Act
            this.parameterManager.Add(state1, 42);
            this.parameterManager.Add(state2, 63);

            // Assert
            this.parameterManager.GetParameters()
                .Should().Satisfy(
                    p => p.Name == "@p1",
                    p => p.Name == "@p2");
        }

        [Fact]
        public void AddGeneratesCorrectHash()
        {
            // Arrange
            var source = TableTarget.Create("users", "u");
            var state1 = Column.Equals<int>(pdq.state.Column.Create("id", source), 42);
            var state2 = Column.Equals<int>(pdq.state.Column.Create("id", source), 64);

            // Act
            var result1 = this.parameterManager.Add(state1, 42);
            this.parameterManager.Clear();
            var result2 = this.parameterManager.Add(state2, 64);

            // Assert
            result1.Hash.Should().BeEquivalentTo(result2.Hash);
        }

        [Fact]
        public void ClearRemovesAllState()
        {
            // Arrange
            var source = TableTarget.Create("users", "u");
            var state1 = Column.Equals<int>(pdq.state.Column.Create("id", source), 42);
            var state2 = Column.Equals<int>(pdq.state.Column.Create("member_id", source), 63);
            this.parameterManager.Add(state1, 42);
            this.parameterManager.Add(state2, 63);

            // Act
            this.parameterManager.Clear();

            // Assert
            this.parameterManager.GetParameters().Should().HaveCount(0);
            this.parameterManager.GetParameterValues().Should().HaveCount(0);
        }
    }
}

