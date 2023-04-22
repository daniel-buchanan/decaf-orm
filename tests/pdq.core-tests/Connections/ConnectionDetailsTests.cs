using System;
using System.Threading.Tasks;
using FluentAssertions;
using pdq.common;
using pdq.common.Connections;
using pdq.tests.common.Mocks;
using Xunit;

namespace pdq.core_tests.Connections
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

