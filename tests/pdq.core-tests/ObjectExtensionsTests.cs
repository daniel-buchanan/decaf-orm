using System;
using Xunit;
using pdq.state;
using pdq.core_tests.Models;
using FluentAssertions;

namespace pdq.core_tests
{
	public class ObjectExtensionsTests
	{
		public ObjectExtensionsTests()
		{
		}

        [Fact]
        public void PropertyNotExistsReturnsNull()
        {
            // Arrange
            var p = new Person()
            {
                Email = "bob@bob.com"
            };

            // Act
            var prop = p.GetProperty("PropertyDoesntExist");

            // Assert
            prop.Should().BeNull();
        }

        [Fact]
		public void CanGetProperty()
		{
			// Arrange
			var p = new Person()
			{
				Email = "bob@bob.com"
			};

			// Act
			var prop = p.GetProperty("Email");

			// Assert
			prop.Should().NotBeNull();
		}

        [Fact]
        public void CanGetPropertyFromExpression()
        {
            // Arrange
            var p = new Person()
            {
                Email = "bob@bob.com"
            };

            // Act
            var prop = p.GetProperty(x => x.Email);

            // Assert
            prop.Should().NotBeNull();
        }

        [Fact]
        public void CanSetProperty()
        {
            // Arrange
            var p = new Person()
            {
                Email = "bob@bob.com"
            };

            // Act
            p.SetProperty("Email", "smith@bob.com");

            // Assert
            p.Email.Should().Be("smith@bob.com");
        }

        [Fact]
        public void CanSetPropertyWithExpression()
        {
            // Arrange
            var p = new Person()
            {
                Email = "bob@bob.com"
            };

            // Act
            p.SetProperty(x => x.Email, "smith@bob.com");

            // Assert
            p.Email.Should().Be("smith@bob.com");
        }

        [Fact]
        public void CanSetPropertyFrom()
        {
            // Arrange
            var p1 = new Person()
            {
                Email = "bob@bob.com"
            };
            var p2 = new Person()
            {
                Email = "bob@smith.com"
            };

            // Act
            p1.SetPropertyFrom("Email", p2);

            // Assert
            p1.Email.Should().Be("bob@smith.com");
        }
    }
}

