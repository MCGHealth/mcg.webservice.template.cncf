# Mcg.Webservice.Cncf.Api C# Project

This is the core project; the project that will produce the deliverable to be deployed.

[Parent Solution](../Readme.md)

## Nuget Package Dependencies _(Not including default dependencies)_

| **Package**                                                                                    | **Min Version** |
| ---------------------------------------------------------------------------------------------- | --------------: |
| **[AspectInjector](https://www.nuget.org/packages/AspectInjector/)**                           |           2.2.8 |
| **[Jaeger](https://www.nuget.org/packages/Jaeger/)**                                           |           0.3.6 |
| **[Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/)**                         |          12.0.3 |
| **[OpenTracing](https://www.nuget.org/packages/OpenTracing/)**                                 |          0.12.1 |
| **[OpenTracing.Contrib.NetCore](https://www.nuget.org/packages/OpenTracing.Contrib.NetCore/)** |           0.6.2 |
| **[prometheus-net](https://www.nuget.org/packages/prometheus-net/)**                           |           3.4.0 |
| **[prometheus-net.AspNetCore](https://www.nuget.org/packages/prometheus-net.AspNetCore/)**     |           3.4.0 |
| **[Serilog.AspNetCore](https://www.nuget.org/packages/Serilog.AspNetCore/)**                   |           3.2.0 |
| **[Serilog.Sinks.Console](https://www.nuget.org/packages/Serilog.Sinks.Console/)**             |           3.1.1 |
| **[Serilog.Sinks.Elasticsearch](https://www.nuget.org/packages/Serilog.Sinks.Elasticsearch/)** |           8.0.1 |
| **[Serilog.Extensions.Hosting](https://www.nuget.org/packages/Serilog.Extensions.Hosting/)**   |           3.0.0 |
| **[Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/)**           |           5.0.0 |
|                                                                                                |                 |

## Project Structure

| **Project Items**                                | **Description**                                                                                                                             |
| ------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------- |
| **[Controllers](./Controllers/Readme.md)**       | Contains all WebApi controller implementations                                                                                              |
| **[DataAccess](./DataAccess/Readme.md)**         | Contains all data access implementation, whether it be a file or database or memory.                                                        |
| **[Messaging](./Messaging/Readme.md)**           | Contains all code artifacts related to publishing (producing) and subscribing to (consuming) asynchronous messaging (event and/or commands) |
| **[Models](./Models/Readme.md)**                 | Contains all POCO data objects related to the business logic implementation                                                                 |
| **[Services](./Services/Readme.md)**             | Contains all business logic related code artifacts                                                                                          |
| **[Infrastructure](./Infrastructure/Readme.md)** | Contains all logic related to cross cutting concerns, i.e., configuration, logging, tracing, metrics, APM, etc.                             |
| [Program.cs](./Program.cs)                       | The main entry point for the application                                                                                                    |
| [Startup.cs](./Startup.cs)                       | Contains the startup initialization logic requires to launch the application                                                                |
| [Dockerfile](./Dockerfile)                       | Defines the docker container that will be used to deploy the application                                                                    |
|                                                  |                                                                                                                                             |

## Configuration Values

Configuration is handled using the following environmental variables:

| **Env Var Name**       | **Acceptable Values**                                                                                                                   | **Description**                                           |
| ---------------------- | --------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------- |
| LOG_LEVEL              | debug, verbose, information, warning, error, fatal                                                                                      | Sets the logging level for the application                |
| JAEGER_AGENT_HOST      | Host FQDN _(preferred)_ or IPv4 address                                                                                                 | The hostname for communicating with agent via UDP         |
| JAEGER_SERVICE_NAME    | alpha-numeric value                                                                                                                     | The service name                                          |
| JAEGER_AGENT_PORT      | integer value _(default = 6831)_                                                                                                        | The port for communicating with agent via UDP             |
| JAEGER_SAMPLER_TYPE    | const, probablistic, ratelimiting, remote _(default = const)_                                                                           | The trace sampler type                                    |
| CORS_ALLOWED_URLS      |                                                                                                                                         | A comma delimited list of URI's that are allowed for CORS |
| ELASTIC_SEARCH_URI     |                                                                                                                                         | The URL for Elasticsearch for logs                        |
| ASPNETCORE_ENVIRONMENT | [Use multiple environments in ASP.NET core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-3.1) |                                                           |
|                        |                                                                                                                                         |                                                           |
