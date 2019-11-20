# Mcg.Webservice.Cncf.Api.Configuration

Contains all logic to manage the external configuration.

 | **Namespace Items**                      | **Description**                                                            |
 | ---------------------------------------- | -------------------------------------------------------------------------- |
 | **[IAppSettings.cs](./IAppSettings.cs)** | Describes the minimal configuration settings that the application requires |
 | **[AppSettings.cs](./AppSettings.cs)**   | Implements the [IAppSettings](./IAppSettings.cs) interface                 |
 |                                          |                                                                            |

## Usage

The goal of this class is to remove the need to add code for new configuration value that may be added during the course of development.  Any environmental variable added with the prefix `APP_` will be automatically recognized by the built in Asp.net core configuration functionality.  For example, the application requires the env var `LOG_LEVEL`.  In the [Dockerfile](../Dockerfile) the env var is defined as:

```ruby
    ENV APP_LOG_LEVEL=information
```

The configuration value can then be accessed via a singleton implementation of the interface `IAppSettings`.

```csharp
    public class Example
    {
        private IAppSettings Settings {get;}
        public Example(IAppSettings settings)
        {
            this.Settings = settings;

            var logLevel = settings["LOG_LEVEL"];
        }
    }
```

This approach permits the easy addition or modification of env vars without the need for writing new code or having the test the new code.

## Some Thoughs

* Please delete any unused configuration values from:

  * [Dockerfile](../../Dockerfile)
  * [docker-compose.yaml](../../../docker-compose.yaml)
  * [launchSettings.json](../../Properties/launchSettings.json)
