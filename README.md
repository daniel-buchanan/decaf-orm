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

## Supported Query Types
PDQ supports the most common query types:  
- Select
- Delete
- Update
- Insert

If there is a specific variation (for example, PostgreSQL Upsert), then it will be added by the appropriate database implementation library.

Each of these supported query types is availble through the appropriate extension method on a query:
```csharp
query.Select();
query.Delete();
query.Update();
query.Insert();
```

In general for each supported query type, the Fluent API follows how you would write the query in SQL, or as you would read it in plain English.

***Note:*** That for all supported query types, both "typed" and "un-typed" methods are supported.

### Several examples of "un-typed" usage:  
```csharp
// select statement
query.Select()
  .From("user")
  .Where(b => 
    b.Column("id").IsNot().Null()
  );

// delete statement
query.Delete()
  .From("user")
  .Where(b => 
    b.Column("id").Is().EqualTo(42)
  );

// update statement
query.Update()
  .Table("user")
  .Set("email", "bob@bob.com")
  .Set("is_active", false)
  .Where(b => 
    b.Column("id").Is().EqualTo(42)
  );

// insert statement
query.Insert()
  .Into("user")
  .Columns(b => {
    email = b.Is<string>(),
    first_name = b.Is<string>(),
    last_name = b.Is<string>(),
    is_active = b.Is<bool>()
  })
  .Value(new {
    email = "bob@bob.com",
    first_name = "bob",
    last_name = "smith",
    is_active = false
  });
```

### Several examples of "typed" usage:  
```csharp
// select statement
query.Select()
  .From<User>(u => u)
  .Where(u => u.Id == 42);

// delete statement
query.Delete()
  .From<User>()
  .Where(u => u.Id == 42);

// update statement
query.Update()
  .Table<User>()
  .Set(u => u.Email, "bob@bob.com")
  .Set(u => u.IsActive, false)
  .Where(u => u.Id == 42);

// insert statement
query.Insert()
  .Into<User>()
  .Columns(u => new {
    u.Email,
    u.FirstName,
    u.LastName,
    u.IsActive
  })
  .Value(new User {
    Email = "bob@bob.com",
    FirstName = "bob",
    LastName = "smith",
    IsActive = false
  });
```

## Select
Select allows for a *Select* query to be built, using the Fluent API, and generally follows how you would write the query if you were doing so by handm with one primary exception.  
That is, you select the columns you want to return *after* you have added your `From`, `Join` and `Where` clauses.

### Sections
1. [Supported Methods on Properties](#supported-methods-on-properties)
2. [Joins](#joins)
3. [Selecting Results](#selecting-results)

### Supported methods on Properties
PDQ generally supports methods which translate well to their SQL counterparts, *e.g.* `Contains() == Like` and `ToUpper() == upper()` and so on.  

The other difference is if you want to check if the column value is *in* a set of values, the parameter in the query is the constant list, making use of the `Contains()` method.  
*e.g.* `.Where(u => ids.Contains(u.Id))`  
This is only supported in the context of a **Where** clause, otherwise you will get exceptions, or no columns selected.  
It is also worth noting, that a **Where** clause is also used as the conditional for a **Join** so it also works there.  
*e.g.* 
```csharp
query.Select()
  .From<User>(u => u)
  .Join<Audit>((u, a) => ids.Contains(u.Id) && u.Id == a.UserId)
```

The following methods are supported on Properties and/or columns:
```csharp
string.Contains(string)
string.ToLower()
string.ToUpper()
string.Substring(int, int?)
DateTime.DatePart(DatePart)
DateTime.Year()
DateTime.Month()
DateTime.Day()
DateTime.Hour()
DateTime.Minute()
DateTime.Second()
DateTime.Millisecond()
DateTime.Epoch()
```

Note that they are *only* supported in the context of either a **Select** or a **Where Clause**, for example:  
```csharp
query.Select()
  .From<User>()
  .Where(u => u.I == 42)
  .Select(u => new {
    Name = u.FirstName.ToLower(),
    Timestamp = u.CreatedAt.Year()
  });
```
The result of which would be:  
```sql
select
  lower(u.first_name) as "Name",
  date_part(year, u.created_at) as "Timestamp"
from user as u
where 
(u.id == @p1);
```
Or, if you are using the "typed" version;  
```csharp
query.Select()
  .From<User>()
  .Where(u => u.Id == 42 || u.FirstName.Contains("bob"))
  .Select(u => new {
    Id = u.Id,
    Name = u.FirstName.ToLower(),
    Email = u.Email.ToUpper()
  });
```
The result would be:  
```sql
select
  u.id as "Id",
  lower(u.first_name) as "Name",
  upper(u.email) as "Email"
from users as u
where 
(
  (u.id == @p1) or
  (u.first_name like @p2)
);
```

### Joins
Joins are handled slightly differently depending on whether you are using the "typed" or "un-typed" versions of the **Select**.

The primary differences are as follows:  
| Feature | Typed | Un-Typed |
| ------- | ----- | -------- |
| One Table to another | This is always done from the previous table that was in a `From` statment, or a `Join` and always done using an expression. The result allows you to perform typed where and select clauses across all joined tables. | This can be done at any point until the where clause. However *you* are responsible for ensuring that all aliases used are correct, otherwise you are likely to get invalid SQL at the other end. |
| To a Query | This is always done from the previous table that was in a `From` statement, or a `Join` statement and always done using an expression. However, you have to specify the type of the result, this is to ensure that selecting results works as you would expect. | This can be done at any point until the where clause. However, as with joining from one table to another, you are responsible for ensuring that all aliases used are correct, otherwise you are likely to get invalid SQL at the other end. |

#### Typed Syntax
The typed syntax for joins, is the same regardless of how deep your joins are, although it is worth noting that at present ***only*** four levels of joins are supported.  
There is one additional feature available when using the "typed" syntax, which is that you can additionally specify the "from" table as well using a type parameter.
```csharp
// one level
query.Select()
  .From<User>(u => u)
  .Join<Audit>((u, a) => u.Id == a.UserId);

// two levels
query.Select()
  .From<User>(u => u)
  .Join<Audit>((u, a) => u.Id == a.UserId)
  .Join<AuditEvent>((u, a, ae) => a.Id == ae.AuditId);

// setting the "from" table
query.Select()
  .From<User>(u => u)
  .Join<Audit>((u, a) => u.Id == a.UserId)
  .Join<AuditEvent>((u, a, ae) => a.Id == ae.AuditId)
  .Join<User, Company>((u, c) => u.Id == c.PrimaryUserId);
```
There are a couple of things worth noting:  
1. Each subsequent join will mean that the return will effectively be:  
   `ISelectFromTyped<T1, T2, TN>`
2. The above also means that if you do a subsequent join, or add a where clause it will take the number of paramters that represent the numbeer of tables you have joined.

#### Un-Typed Syntax
The un-typed syntax for joins is as follows:  
```csharp
query.Select()
  .From("users", "u")
  .Join()
    .From("users", "u")
    .To("audit", "a")
    .On(b => {
      b.Column("id", "u").Is().EqualTo().Column("user_id", "a")
    })
  ...
```
The following things should be noted:  
1. The `From`, `To`, `On` syntax is fixed, and parts of it cannot be skipped
2. The builder provided to the `On` method, is an `IWhereBuilder` so follows the exact same conventions used for that, so if you are already familiar it works the same way.  
   The main difference is that there are the following methods, which are more useful in this case:  
   ```csharp
   EqualTo()
   LessThan()
   LessThanOrEqualTo()
   GreaterThan()
   GreaterThanOrEqualTo()
   ```
   You will note that these, instead of taking a paramter allow you to chain a `.Column()` method on to complete the `a.column = b.column` idea.

### Selecting Results
Selecting Results from a query works in a very similar way when it is "typed" or "un-typed", the primary difference is that the "un-typed" version does not have the type inference to allow you to specify the properties.

#### Typed Syntax
The typed syntax for selecting results will differ depending on how many tables you have joined, but effectively there are two variants:  
1. Dynamic:  
   ```csharp
   .Select(u => new {
     u.Id,
     u.Email
   });
   ```
2. Concrete:
   ```csharp
   .Select(u => new Result {
     UserId = u.Id,
     EmailAddress = u.Email
   });
   ```

#### Un-Typed Syntax
The un-typed syntax for selecting results is the same regardless of whether you have joins etc, as you are still using the `builder.Is<>()` method to define the column you want to select.  
The following methods are available on the builder:  
```csharp
object Is(string name);
object Is(string name, string alias);
T Is<T>(string name);
T Is<T>(string name, string alias);
```
The typed methods are particularly useful when you are returning a concrete result.

There are two kinds of selects for the un-typed version, in the same vein as for the typed version.
1. Dynamic:  
   ```csharp
   .Select(b => new {
     Id = b.Is<int>("id", "u"),
     Email = b.Is<string>("email", "u")
   });
   ```
2. Concrete:
   ```csharp
   .Select<Result>(b => new Result {
     Id = b.Is<int>("id", "u"),
     Email = b.Is<string>("email", "u")
   });
   ```

## Delete

## Update

## Insert

## Execution

