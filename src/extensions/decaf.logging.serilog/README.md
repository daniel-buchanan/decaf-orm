# decaf-orm.logging.serilog
This package provides additional logging support for Serilog if that is your logger of choice.

The main way that this package is intended to be used is to provide a **Serilog** implementation for the `ILoggerProxy` interface.  
This is used to provide logging services to *decaf* itself. 

To use, simply add:
```csharp
.UseSerilog()
```
To the *decaf* options builder. A full example is included below.

```csharp
services.AddDecaf(o =>
    {
        o.TrackUnitsOfWork()
            .OverrideDefaultLogLevel(LogLevel.Debug)
            .UseSerilog();
    });
```

## Serilog Configuration
Serilog will need to have already been configured in your DI pipeline, see the Serilog docs for more information.  
Docs: https://serilog.net  
Nuget: https://www.nuget.org/packages/Serilog  

### Status:
**Build**  
[![Build](https://github.com/daniel-buchanan/decaf-orm/actions/workflows/sonar.yml/badge.svg)](https://github.com/daniel-buchanan/decaf-orm/actions/workflows/sonar.yml)  
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