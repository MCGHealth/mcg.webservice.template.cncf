# Mcg.Webservice.Cncf.Api.Models

Contains all POCO data objects related only to the business logic functionality.  POCO's related to cross-cutting concerns are found in [Sysops](../Sysops/Readme.md).

## Notes

:heavy_exclamation_mark: No business logic should be implemented in this directory, aside from simple overrides such as `ToString()`, or interfaces such as `IEquatable<T>` or `IComparable<T>`.

:heavy_exclamation_mark: All models should be easily serialized / deserialized using JSON.

| **Item**                             | **Description**                                      |
| ------------------------------------ | ---------------------------------------------------- |
| [ExampleModel.cs](./ExampleModel.cs) | An example implementation of a type used as a model. |
|                                      |                                                      |
