// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using pdq;
using pdq.common;
using pdq.playground;
using pdq.playground.Mocks;

var services = new ServiceCollection();
services.AddPdq(o =>
{
    o.EnableTransientTracking();
    o.OverrideDefaultLogLevel(LogLevel.Debug);
    o.UseMockDatabase();
});
services.AddScoped<IConnectionDetails, MockConnectionDetails>();

var provider = services.BuildServiceProvider();

Console.WriteLine("Hello, World!");

var uow = provider.GetService<IUnitOfWork>();
using (var t = uow.Begin())
{
    using (var q = t.Query())
    {
        //q.Select()
        //    .From("bob", "b")
        //    .Column("name")
        //    .Column("email")
        //    .Where(b =>
        //    {
        //        b.ClauseHandling().DefaultToOr();

        //        b.Column("name").Is().EqualTo("hello");
        //        b.Column("email").Is().Like("my name");
        //    });

        q.Select()
            .From<Person>(x => x)
            .Join<Address>((p, a) => p.AddressId == a.Id)
            .Join<Person, Note>((p, n) => p.Id == n.PersonId)
            .Where((p, a, n) => p.LastName.Contains("smith"))
            .Select((p, a, n) => new Result
            {
                Name = p.FirstName,
                City = a.City,
                Note = n.Value
            });

    }
    
}

public class Result
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Note { get; set; }
}
