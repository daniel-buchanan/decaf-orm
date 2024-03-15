using System;
using System.Threading.Tasks;
using decaf.common.Connections;
using decaf.tests.common.Mocks;
using FluentAssertions;
using decaf.common;
using Xunit;

namespace decaf.core_tests.Connections
{
    public class ConnectionDetailsTests
    {
        private readonly IConnectionDetails connectionDetails;

        public ConnectionDetailsTests()
        {
            this.connectionDetails = new MockConnectionDetails();
        }

        [Fact]
        public void GetConnectionStringSucceeds()
        {
            // Act
            Action method = () => this.connectionDetails.GetConnectionString();

            // Assert
            method.Should().NotThrow<Exception>();
        }

        [Fact]
        public async Task GetConnectionStringAsyncSucceeds()
        {
            // Act
            Func<Task<string>> method = () => this.connectionDetails.GetConnectionStringAsync();

            // Assert
            await method.Should().NotThrowAsync<Exception>();
        }
    }
}

