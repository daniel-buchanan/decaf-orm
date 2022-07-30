# pdq
PDQ is a "lite" ORM (Object Relational Mapper), designed to fill the void between tools like [Dapper](https://github.com/DapperLib/Dapper) and using a full blown ORM like EntityFramework.

Unsurprisingly, Dapper forms the foundation of how pdq executes the queries that it builds against the database. This is because what pdq aims to excel at is formulating queries and giving the developer precise control over those queries *before* they are executed.

**So what is PDQ?**.  
PDQ stands for **P**retty **D**arn **Q**uick.  
The aim has always been to provide a intuitive development experience that allows performant queries to be written, while not re-inventing the wheel for accessing the database and materialising objects.

### Status:
**Build**  
[![Build](https://github.com/daniel-buchanan/pdq/actions/workflows/sonar.yml/badge.svg)](https://github.com/daniel-buchanan/pdq/actions/workflows/sonar.yml)
[![CodeQL](https://github.com/daniel-buchanan/pdq/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/daniel-buchanan/pdq/actions/workflows/codeql-analysis.yml)  
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

## Sections
1. [History](#history)
2. [Concepts](#concepts)
3. [Examples](#examples)
4. [Reference](#reference)

# History
[Return to Top](#pdq)  

PDQ originally came about because during the course of work we needed something that enabled us to write complex, performant queries, but at the time nothing existed (at least that myself or my team could find).  

Hence we started on the journey of writing our own.  

This proved to be a long but worthwhile one, but now the time has come for a ground-up-re-write; technology has moved on (for the better) and the original concepts behind the API; while useful, don't provide a particlarly good development experience.  

For this re-birth if you will, the attempt is to make use of a fully fluent API and provide as much of an intuitive experience as possible.

# Concepts
[Return to Top](#pdq)  

There are several core concepts within PDQ.  
1. Everything should use a Fluent API  
   ```csharp
   query.Select().From("users", "u").Where(b => b.Column("email", "u").IsNot().Null());
   ```
2. The API should be intuitive to use and follow common SQL language, or english language patterns.  
   More specifically, statements should either:  
   *a)* Make sense from a SQL statement as you would write it  
   *b)* Read as understandable English
   
# Examples
[Return to Top](#pdq)  


## Configuration
In order to use PDQ, you simply have to reference it and ensure that it is registered with your Dependency Injection provider.  

The following options are available to be configured:
| Item | Description |
| ---- | ----------- |
| Default Log Level | This allows you to configure the default log level/verbosity of what PDQ itself prints to the logs. |
| Default Clause Handling | This allows you to configure the default "Where" clause handling, i.e. the default way that clauses are bundled together when using an `IWhereBuilder`. |
| Transient Tracking | This allows you to enable or disable tacking of "Transients", which are effectively the unit by which pdq executes queries. |

Extensions methods are provided for the built in .Net Dependency Injection framework, which work as follows:  
1. No Configuration, just use defaults  
   ```csharp
   services.AddPdq();
   ```  
2. Configuration passed in:
   ```csharp
   var options = new PdqOptions();
   options.OverrideDefaultLogLevel(LogLevel.Debug);
   services.AddPdq(options);
   ```
3. Configured using a provided Action:
   ```csharp
   services.AddPdq(o => {
     o.OverrideDefaultLogLevel(LogLevel.Debug);
   });
   ```

To configure a given database provider, works in a very similar way; insofar as that an extension method is provided on the `PdqOptions` which allows for the configuration.  
For example, with PostgreSQL:  
```csharp
services.AddPdq(o => {
  o.OverrideDefaultLogLevel(LogLevel.Debug);
  o.UseNpgsql(connectionString);
});
```

## Getting a Query
[Return to Top](#pdq)  

PDQ's default configuration provides an `IUnitOfWOrk` service which is added to the service provider by default.  
Once you have a `ITransient` created from the `IUnitOfWork`, you can begin to query it. Each query is managed by itself and executed seperately.  

> It is worth noting that it is the `ITransient` instance that controls the commit/rollback of the transaction which is associated with any queries created from it.

Once this is injected into your service, handler or other class it can be used in the following ways.

**Disposable:**
```csharp
using(var transient = this.uow.Begin())
{
  using(var query = transient.Query())
  {
    ...
  }
}
```

**Non-Disposable:**
```csharp
var transient = this.uow.Begin();
var query = t.Query();
```

## Creating a Simple Query
[Return to Top](#pdq)  

There are two main ways that PDQ can be used:  
1. Typed; and
2. Un-typed

Each has their merits and will be covered in brief below, however for more details please see the [Reference](#reference).  

**Un-Typed:**  
This is intended primarily for use where you want to write SQL, but you don't want it hard-coded into your application, and/or store-procedures
or functions aren't your thing. This flexible API allows you to write the statement in a fluent manner while retaining readability.  
```csharp
var result = query
  .Select()
  .From("users", "u")
  .Where(b => b.Column("id", "u").Is().EqualTo(42))
  .Select(b => new {
    Id = b.Is<int>("id", "u"),
    Email = b.Is<string>("email", "u")
  })
  .FirstOrDefault();
```

**Typed:**  
This is intended for use where you already have objects that represent your database tables, noting that they will either need to be a direct
mapping to the tables themselves; or be modified using the various attributes that PDQ provides to modify property and class names when parsed.  
```csharp
var result = query
  .Select()
  .From<User>(u => u)
  .Where(u => u.Id == 42)
  .Select(u => new {
    Id = u.Id,
    Email = u.Email
  })
  .FirstOrDefault();
```

# Reference
[Return to Top](#pdq)  

## Untyped

## Typed

## Execution

