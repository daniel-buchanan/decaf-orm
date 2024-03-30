using decaf.common;
using decaf.tests.common.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace decaf.core_tests;

public class UnitOfWorkTests : CoreTestBase
{
    [Fact]
    public void UnitOfWorkFluentSuccess()
    {
        // Arrange
        var decaf = provider.GetService<IDecaf>();
        var unit = decaf.BuildUnit();
        var hasException = false;
        User result = null;

        // Act
        unit.OnException(_ => hasException = true)
            .OnSuccess(() => hasException = false)
            .Query(q =>
            {
                result = q.Select()
                    .From<User>(u => u)
                    .Where(u => u.Id == 1)
                    .SelectAll<User>(u => u)
                    .SingleOrDefault();
            })
            .PersistChanges();

        // Assert
        hasException.Should().BeFalse();
    }
}