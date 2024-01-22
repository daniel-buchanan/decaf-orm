# pdq.logging.serilog
This package provides additional logging support for Serilog if that is your logger of choice.

The main way that this package is intended to be used is to provide a **Serilog** implementation for the `ILoggerProxy` interface.  
This is used to provide logging services to *pdq* itself. 

To use, simply add:
```csharp
.UseSerilog()
```
To the *pdq* options builder. A full example is included below.

```csharp
services.AddPdq(o =>
    {
        o.TrackUnitsOfWork()
            .OverrideDefaultLogLevel(LogLevel.Debug)
            .UseSerilog();
    });
```

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

## Documentation
Documentation can be found here: [README](https://github.com/daniel-buchanan/pdq/blob/main/README.md).