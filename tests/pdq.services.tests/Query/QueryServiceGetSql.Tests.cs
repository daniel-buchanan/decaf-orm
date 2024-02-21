using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using pdq.common;
using pdq.state;
using pdq.tests.common.Mocks;
using pdq.tests.common.Models;
using pdq.db.common.ANSISQL;
using Xunit;

namespace pdq.services.tests.Query;

public class QueryServiceGetSql_Tests
{
    private readonly IService<Person, int> personService;

    public QueryServiceGetSql_Tests()
    {
        var services = new ServiceCollection();
        services.AddPdq(o =>
        {
            o.TrackUnitsOfWork();
            o.OverrideDefaultLogLevel(LogLevel.Debug);
            o.UseMockDatabase().WithMockConnectionDetails();
        });
        services.AddPdqService<Person, int>().AsScoped();

        var provider = services.BuildServiceProvider();
        this.personService = provider.GetService<IService<Person, int>>();
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