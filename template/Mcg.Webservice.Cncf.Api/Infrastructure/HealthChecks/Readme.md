# Mcg.Webservice.Cncf.Api.Infrastructure.HealthChecks

Contains the logic necessary to expand the **[Microsoft.AspNetCore.Diagnostics.HealthChecks](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.0)** to include checks for app specific dependencies.

## Files

| **File**                                                                          | **Description**                                                                                            |
| --------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------- |
| :page_facing_up: [Heartbeat.cs](./Heartbeat.cs)                                   | Implements the response description defined in the 03.04: Health Reporting and Alerts: ResponseDescription |
| :page_facing_up: [RestfulEndpointHealthCheck.cs](./RestfulEndpointHealthCheck.cs) | Creates a health check call to a MCG service that is required for this service to work                     |
| :page_facing_up: [HealthChecksExtensions.css](./HealthChecksExtensions.cs)        | Extension methods used to add the health check during startup                                              |
|                                                                                   |                                                                                                            |
