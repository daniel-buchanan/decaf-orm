# decaf-orm
Decaf is a "lite" ORM (Object Relational Mapper), designed to fill the void between tools like [Dapper](https://github.com/DapperLib/Dapper) and using a full blown ORM like EntityFramework.

Unsurprisingly, Dapper forms the foundation of how decaf executes the queries that it builds against the database. This is because what decaf aims to excel at is formulating queries and giving the developer precise control over those queries *before* they are executed.

**So what is decaf?**.  
Decaf is so named because it's like coffee without the kick.
The aim has always been to provide a intuitive development experience that allows performant queries to be written, while not re-inventing the wheel for accessing the database and materialising objects.

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