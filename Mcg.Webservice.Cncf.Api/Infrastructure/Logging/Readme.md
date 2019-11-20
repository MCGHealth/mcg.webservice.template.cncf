# Mcg.Webservice.Cncf.Api.Infrastructure.Logging

## Files

| **File**                                                                      | **Description**                                                                                                 |
| ----------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------- |
| :page_facing_up: [HttpEventData.cs](./HttpEventData.cs)                       | Used by the RequestLoggingMiddleware to ensure that all reqired information about a request/response is logged. |
| :page_facing_up: [LoggingConfig.cs](./LoggingConfig.cs)                       | Initializes the logging infrastructure required by the application                                              |
| :page_facing_up: [JsonLogFormatter.cs](./JsonLogFormatter.cs)                 | Ensures a uniform formatting of log entries                                                                     |
| :page_facing_up: [MonitorAttribute.cs](./MonitorAttribute.cs)                 | Enables a method to be logged by using IL instructions within the decorated method                              |
| :page_facing_up: [RequestLoggingMiddleware.cs](./RequestLoggingMiddleware.cs) | Ensures that all incoming HTTP requests and responses are logged                                                |

## Notes

The `MonitorAttribute` should **NOT** be used on endpoints that are implemented within anything that derives from `Microsoft.AspNetCore.Mvc.ControllerBase`.  The logging for endpoints will be handled by the [RequestLoggingMiddleware](./RequestLoggingMiddleware.cs)
