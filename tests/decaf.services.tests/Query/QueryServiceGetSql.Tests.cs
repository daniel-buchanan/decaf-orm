using decaf.tests.common.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using decaf.tests.common.Mocks;
using Xunit;

namespace decaf.services.tests.Query;

public class QueryServiceGetSql_Tests
{
    private readonly IService<Person, int> personService;

    public QueryServiceGetSql_Tests()
    {
        var services = new ServiceCollection();
        services.AddDecaf(o =>
        {
            o.TrackUnitsOfWork();
            o.OverrideDefaultLogLevel(LogLevel.Debug);
            o.UseMockDatabase().WithMockConnectionDetails();
        });
        services.AddDecafService<Person, int>().AsScoped();

        var provider = services.BuildServiceProvider();
        personService = provider.GetService<IService<Person, int>>();
    }

    [Fact]
    public void SelectProvidesQuery()
    {
        // Arrange

        // Act
        var result = personService.All();

        // Assert
        personService.LastExecutedSql.Should().NotBeNullOrWhiteSpace();
    }
}