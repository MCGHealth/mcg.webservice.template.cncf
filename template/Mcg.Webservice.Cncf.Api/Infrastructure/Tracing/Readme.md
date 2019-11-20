# Mcg.Webservice.Cncf.Api.Infrastructure.Tracing

## Files

| **File**                                                                      | **Description**                                                           |
| ----------------------------------------------------------------------------- | ------------------------------------------------------------------------- |
| :page_facing_up: [TraceAttribute.cs](./TraceAttribute.cs)                     | Handles distributed tracing for the decorated object.                     |
| :page_facing_up: [TracingServicesExtension.cs](./TracingServicesExtension.cs) | Used in the Startup.cs file to add tracing capability to the application. |
|                                                                               |                                                                           |

## Notes

* The `TraceAttribute` should **NOT** be used on endpoints that are implemented within anything that derives from `Microsoft.AspNetCore.Mvc.ControllerBase`.  The OpenTracing API handles that automatically.
* Private, protected, and internal methods within a controller CAN be decorated with `TraceAttribute` if needed.
