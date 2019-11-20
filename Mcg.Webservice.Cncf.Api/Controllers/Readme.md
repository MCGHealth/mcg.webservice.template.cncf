# Mcg.Webservice.Cncf.Api.Controllers

Contains all WebApi controller implementations.

The [ExampleController.cs](./ExampleController.cs) demonstrates the ideal way to creating a WebApi controller at MCG.

* All WebAPI controllers should conform to the [Richardson Maturity Model: Level 2](https://restfulapi.net/richardson-maturity-model/#level-two).
* The WebAPI controller should inherit from [ControllerBase](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase?view=aspnetcore-3.0).
* Any dependencies should be - if practical - injected via dependency injection
* All actions should explicitly define the possible response types using the [ProducesResponseTypeAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.producesresponsetypeattribute?view=aspnetcore-3.0)
* The controller should be decorated with the attribute [ProducesAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.producesattribute?view=aspnetcore-3.0).
* The controller should be decorated with the attribute [ConsumesAttribute](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.consumesattribute?view=aspnetcore-3.0).
* Endpoints that use the verbs `POST`, `PUT`, `PATCH`, or `DELETE` should be asynchronous whenever practical.
* GET endpoints should be synchrounous and return an `HTTP 200` for a successful call.
* Asynchronous `POST` endpoints should return an `HTTP 202` (Accepted) for a successful call.
* Synchronous `POST` endpoints should return an `HTTP 201` (Created) for a successful call.
* Synchronous and Asynchronous `PUT`, `PATCH`, and `DELETE` should return an `HTTP 204` (No Content) for a successful call.
* A WebAPI controller should work on/expose a only single resource.

## Files

 | **Namespace Items**                            | **Description**                                                                                                |
 | ---------------------------------------------- | -------------------------------------------------------------------------------------------------------------- |
 | [ExampleController.cs](./ExampleController.cs) | An example of an api controller that receives a IExampleBusinessService instance through dependency injection. |
 |                                                |
