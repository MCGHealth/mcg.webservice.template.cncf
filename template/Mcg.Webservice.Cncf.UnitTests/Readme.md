# Mcg.Webservice.Netcore.UnitTests

Implements the unit tests for the project [Mcg.Webservice.Cncf.Api](../Mcg.Webservice.Cncf.Api/Readme.md)

## Unit Test Types

* `*BehaviorTests`: These types of tests verify that the target of the test are decorated with the expected attributes, implement the expected interfaces, and/or inherit from an expected base class.  They do NOT test the functionality of the implemented or inherited behavior.
* `*FuntionalTests`: These types of tests verify the function of the implementation, i.e., that the implementation works as expected.
* If the test file does not have the word `Behavior` or `Function` in the name, then it contains typical unit function tests.

## Contents

| **Item**            | **Description**                                                                                                             |
| ------------------- | --------------------------------------------------------------------------------------------------------------------------- |
| ConfigurationTests  | Contains unit tests for the the [Mcg.Webservice.Cncf.Api.Configuration](../Mcg.Webservice.Cncf.Api/Configuration/readme.md) namespace |
| ControllerTests     | Contains unit tests for the [Mcg.Webservice.Cncf.Api.Controllers](../Mcg.Webservice.Cncf.Api/Controllers/readme.md) namepspace        |
| MessagingTests      | Contains unit tests for the[Mcg.Webservice.Cncf.Api.Messaging](../Mcg.Webservice.Cncf.Api/Messaging/readme.md) namespace              |
| ModelTests          | Contains unit tests for Mcg.[Mcg.Webservice.Cncf.Api.Models](../Mcg.Webservice.Cncf.Api/Controllers/readme.md) namespace              |
| ServicesTests       | Contains unit tests for any [Mcg.Webservice.Cncf.Api.Services](../Mcg.Webservice.Cncf.Api/Controllers/readme.md) namespace            |
| InfrastructureTests | Contains unit tests for any [Mcg.Webservice.Cncf.Api.Infrastructure](../Mcg.Webservice.Cncf.Api/Infrastructure/readme.md) namespace   |
|                     |                                                                                                                             |
