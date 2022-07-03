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
            .From<Person>()
            .Join<Address>((p, a) => p.AddressId == a.Id)
            .Join<Person, Note>((p, n) => p.Id == n.PersonId)
            .Columns((p, a, n) => new
            {
                Name = p.FirstName,
                City = a.City,
                Note = n.Value
            })
            .Where((p, a, n) => p.LastName.Contains("smith"));

    }
    
}
