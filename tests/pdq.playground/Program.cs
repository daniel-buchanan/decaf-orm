// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using pdq;
using pdq.common;

var services = new ServiceCollection();
services.AddPdq(o =>
{
    o.EnableTransientTracking();
    o.OverrideDefaultLogLevel(LogLevel.Debug);
});

var provider = services.BuildServiceProvider();

Console.WriteLine("Hello, World!");

var uow = provider.GetService<IUnitOfWork>();
using (var t = uow.Begin())
{
    var q = t.Query();
    q.Select()
        .From("bob", "b")
        .Column("name")
        .Column("email")
        .Where(b =>
        {
            b.ClauseHandling().DefaultToOr();
            
            b.Column("name").Is().EqualTo("hello");
            b.Column("email").Is().Like("my name");
        });
}
