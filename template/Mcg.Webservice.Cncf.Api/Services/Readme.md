# Mcg.Webservice.Cncf.Api.Services

Contains all business logic related code artifacts. Any logic related to operational readiness is located in [Infrastructure](../Infrastructure/Readme.md).

NOTE: The objects implemented within this namespace should be used via dependency injection functionality built into ASP.NET Core.

## Files

| **Item**                                                   | **Description**                                                                      |
| ---------------------------------------------------------- | ------------------------------------------------------------------------------------ |
| [IExampleBusinessService.cs](./IExampleBusinessService.cs) | A simple example of an interface defining business processing logic.                 |
| [ExampleBusinessService.cs](./ExampleBusinessService.cs)   | A example implementation of [IExampleBusinessService](./IExampleBusinessService.cs). |
|                                                            |                                                                                      |

## Wait...What the heck is this?

Yes, the approach in ExampleBusinessLogic.cs will seem very...unorthodox, especially to those who are not familiar with the programming lanuage Go. **[ValueTuples](https://blogs.msdn.microsoft.com/mazhou/2017/05/26/c-7-series-part-1-value-tuples/)** in .NET can mimic a feature in Go where functions can return multiple values. For example:

```go
// a Go function
function (mo * myType) Foo (ok bool, result interface{}, err error){
    // do some cool stuff in Go
}
```

C# Mimicry:

```csharp
// available since c# 7.0
public (bool ok, object result, error string) Foo()
{
    // do some cool stuff in c#
}
```

**Note: the use of a ValueTuple requires .Net Framework 4.7+ or .Net Standard _(a.k.a. .Net Core)_ 2.0+**

## OK, So what's your point?

In a typical .Net application inserting a duplicate key within a dictionary or database will result in a exception being thrown. This exception in turn is used to fashion the proper response to the caller of the API. In essense, it can be interpreted as using exceptions to control the flow of the business logic.

In contrast to using exceptions, the businss logic evaluates the current state of the database to determine if the model to be inserted conforms to the rules. If it doesn't, it returns a 'false' and a reason why the action will not be completed. The results of this are used to fashion the response to the caller.

Exceptions are not completely excluded from use, however. If a true system level exception is thrown, such as a timeout exception, or a socket exception, or some exception that is the result of a logic error, it will be thrown and handled as one normally would.

In essense, this example demonstrates creating a clear distinction between business errors and logic/system exceptions.

## But What About DataAnnotations?

DataAnnotations? Yeah, they are all well and good; a solid staple in the .NET world almost since its inception. However, they throw exceptions...well, usually they do. We all know that exception handling is costly. We're also taught early on in our careers not to use exceptions to control the flow of business logic. I often ponder why this is the way it is. Feel free to use those if you wish - it's an industry-wide accepted approach after all. But also be willing to try something different.
