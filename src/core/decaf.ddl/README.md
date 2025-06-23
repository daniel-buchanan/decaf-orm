# decaf-orm.ddl
This package provides support for DDL natively within decaf.
> **Note:** the implementation of the DDL is dependent on the actual implementation library, e.g. decaf.npgsql.

There are several variants of these, that allow for multiple composite keys on entities.
This is supplied through the `IEntity` interface, which has four variants:
1. `IEntity` - no primary key defined
2. `IEntity<TKey>` - a single primary key of the type `TKey`
3. `IEntity<TKeyOne, TKeyTwo>` - a composite primay key of the type `<TKeyOne, TKeyTwo>`
4. `IEntity<TKeyOne, TKeyTwo, TKeyThree>` - a composite primary key of the type `<TKeyOne, TKeyTwo, TKeyThree>`

Each of these exist for the `Command`, `Query` and `Service` variants.

Each `IEntity` has a `KeyMetadata` property, which represents the primary key information for that object.  
This is either just an `IKeyMetadata` instance, or it is a `ICompositeKey` corresponding to the primary key as defined.

To use services, your entities should inherit from one of the `Entity` classes as they provide the base structures necessary for the services to operate.  
You will need to provide the key metadata at this point.  
An example of a simple entity is below:
```csharp
public class Person : Entity<int>
{
    public Person() : base(nameof(Id)) { }

    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

Note that the key name is passed into the base constructor to provide the key metadata.

To add a `Query`, `Command` or `Service` to your DI container, use the following as a template:
```csharp
services.AddDecafService<Person>().AsScoped();
```

You can also use `AsSingleton()` or `AsTransient()` depending on your requirements.
The above method registers the following:
- `IQuery<Person>`
- `ICommand<Person>`
- `IService<Person>`

Alternatively, if you are using the `IUnitOfWork` patter, you may use the following extension methods:
```csharp
uow.GetQuery<Person>();
uow.GetCommand<Person>();
uow.GetService<Person>();
```

### Status:
**Build**  
[![Build](https://github.com/daniel-buchanan/decaf-orm/actions/workflows/sonar.yml/badge.svg)](https://github.com/daniel-buchanan/decaf-orm/actions/workflows/sonar.yml)
[![CodeQL](https://github.com/daniel-buchanan/decaf-orm/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/daniel-buchanan/decaf-orm/actions/workflows/codeql-analysis.yml)  
**Sonar Cloud**  
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_pdq&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_pdq)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_pdq&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_pdq)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_pdq&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_pdq)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_pdq&metric=coverage)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_pdq)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_pdq&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_pdq)  
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_pdq&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_pdq)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_pdq&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_pdq)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_pdq&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_pdq)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_pdq&metric=bugs)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_pdq)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_pdq&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_pdq)

## Documentation
Documentation can be found here: [README](https://github.com/daniel-buchanan/decaf-orm/blob/main/README.md).