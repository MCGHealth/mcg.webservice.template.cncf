# Mcg.Webservice.Template.Cncf

We at MCG are currently undergoing a focused effort to make our products and services cloud native. At this time our primary development language is C#.NET. Fortunately .NET Core’s support for cloud development is fairly robust; it supports containerization using Linux containers, and has a wide variety of APIs and libraries to support many of today's top open-source technologies.

When writing microservices using WebAPI and related technologies, one thing all services have in common are the following:

- They need to be containerized;
- They need to have robust, structured, leveled logging;
- They need to be instrumented, i.e., performance counters;
- A health check, or heartbeat, endpoint needs to be available for monitoring;
- And - in my opinion - distributed tracing is an absolute must!

Setting up this “infrastructure code”, or boilerplate, can be almost as time consuming as implementing the logic that solves the business problem itself! Not only that, getting that boilerplate implemented across all services consistently can be a real challenge, especially when you may have several scrum teams working on different projects at one time. Consistency in implementing these cross-cutting concerns is equally as important for your DevOps and SysOps teams.

## Some Prerequisites

### All Platforms

- Your development machine will need (at least at this time) both .NET Core [SDK 2.1.607](https://dotnet.microsoft.com/download/dotnet-core/2.1) and [SDK 3.0.101](https://dotnet.microsoft.com/download/dotnet-core/3.0).

- [Docker Desktop](https://www.docker.com/products/docker-desktop) for building and running the solution locally in a container.

- [Coverlet](https://github.com/tonerdo/coverlet?WT.mc_id=-blog-scottha#coverlet) for calculating code coverage when running the command `make test`

```shell
dotnet tool install --global coverlet.console
```

### Windows Only

- Ensure [Chocolatey](https://chocolatey.org/install) is installed. Chocolatey is a package manager for Windows and performs a similar function for it as Brew does for OSX, or APT or yum does for their respective Linux distros.
- After installing Chocolatey, install GNU Make:

```shell
choco install make
```

### Optional

- (OSX, Windows)[Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
- (OSX, Windows, Linux)[Visual Studio Code](https://code.visualstudio.com/)

---

## Installing the template

1. Clone the repository into a location of your choice:

   ```shell
   git clone https://github.com/jeremyj01/mcg.webservice.template.cncf.git
   ```

2. Navigate to the template directory:

   ```shell
   cd mcg.webservice.template.cncf/template
   ```

3. Run the following command:

   ```shell
   dotnet new -i .
   ```

4. The template should now be installed as "MCG's ASP.NET Core Web API". The output of the install command will look similar to below:

   ```shell
   Welcome to .NET Core!
   ---------------------
   Learn more about .NET Core: https://aka.ms/dotnet-docs
   Use 'dotnet --help' to see available commands or visit: https://aka.ms/dotnet-cli-docs

   Telemetry
   ---------
   The .NET Core tools collect usage data in order to help us improve your experience. The data is anonymous and doesn't include command-line arguments. The data is collected by Microsoft and shared with the community. You can opt-out of telemetry by setting the DOTNET_CLI_TELEMETRY_OPTOUT environment variable to '1' or 'true' using your favorite shell.

   Read more about .NET Core CLI Tools telemetry: https://aka.ms/dotnet-cli-telemetry

   ASP.NET Core
   ------------
   Successfully installed the ASP.NET Core HTTPS Development Certificate.
   To trust the certificate run 'dotnet dev-certs https --trust' (Windows and macOS only). For establishing trust on other platforms refer to the platform specific documentation.
   For more information on configuring HTTPS see https://go.microsoft.com/fwlink/?linkid=848054.
   Getting ready...
   Usage: new [options]

   Options:
     -h, --help          Displays help for this command.
     -l, --list          Lists templates containing the specified name. If no name is specified, lists all templates.
     -n, --name          The name for the output being created. If no name is specified, the name of the current directory is used.
     -o, --output        Location to place the generated output.
     -i, --install       Installs a source or a template pack.
     -u, --uninstall     Uninstalls a source or a template pack.
     --nuget-source      Specifies a NuGet source to use during install.
     --type              Filters templates based on available types. Predefined values are "project", "item" or "other".
     --force             Forces content to be generated even if it would change existing files.
     -lang, --language   Filters templates based on language and specifies the language of the template to create.
   ```

Templates Short Name Language Tags

---

Console Application console [C#], F#, VB Common/Console
Class library classlib [C#], F#, VB Common/Library
Unit Test Project mstest [C#], F#, VB Test/MSTest
NUnit 3 Test Project nunit [C#], F#, VB Test/NUnit
NUnit 3 Test Item nunit-test [C#], F#, VB Test/NUnit
xUnit Test Project xunit [C#], F#, VB Test/xUnit
Razor Page page [C#] Web/ASP.NET
MVC ViewImports viewimports [C#] Web/ASP.NET
MVC ViewStart viewstart [C#] Web/ASP.NET
ASP.NET Core Empty web [C#], F# Web/Empty
ASP.NET Core Web App (Model-View-Controller) mvc [C#], F# Web/MVC
ASP.NET Core Web App razor [C#] Web/MVC/Razor Pages
ASP.NET Core with Angular angular [C#] Web/MVC/SPA
ASP.NET Core with React.js react [C#] Web/MVC/SPA
ASP.NET Core with React.js and Redux reactredux [C#] Web/MVC/SPA
Razor Class Library razorclasslib [C#] Web/Razor/Library/Razor Class Library
ASP.NET Core Web API webapi [C#], F# Web/WebAPI
MCG ASP.NET Core Web API mcgcncf [C#] WebAPI
global.json file globaljson Config
NuGet Config nugetconfig Config
Web Config webconfig Config
Solution File sln Solution

Examples:
dotnet new mvc --auth Individual
dotnet new mcgwebsvc
dotnet new --help

````

5. If you need to reset your templates back to default, you can run this command:

```shell
dotnet new --debug:reinit
````

---

## Creating a new solution

The template is a dotnet template, not a Visual Studio template. Therefore you create a new solution from the commandline. To create a new solution with the template use the following command:

```shell
dotnet new mcgwebsvc -o [desired solution root dir name] -n [solution name]
```

For example, the following command will create a new soltion named "Acme.Example" and place it in the "Acme.Example" directory:

```shell
dotnet new mcgcncf -o Acme.Example -n Acme.Example
```

The directory structure will look like this:

```shell
<your dir>\Acme.Example
├───Acme.Example.Api
│   ├───Connected Services
│   │   └───Application Insights
│   ├───Controllers
│   ├───DataAccess
│   ├───Infrastructure
│   │   ├───Configuration
│   │   ├───DependencyTracking
│   │   ├───HealthChecks
│   │   ├───Logging
│   │   └───Metrics
│   ├───Messaging
│   │   ├───Publishers
│   │   └───Subscribers
│   ├───Models
│   ├───Properties
│   └───Services
└───Acme.Example.UnitTests
    ├───ControllerTests
    ├───InfrastructureTests
    │   ├───ConfigurationTests
    │   ├───HealthCheckTests
    │   └───MetricsTests
    ├───MessagingTests
    ├───ModelTests
```

---

## Run the solution

From the terminal run the command `make run`. You should see the following output:

```shell
make run
dotnet build Acme.Example.sln -c Debug --force --nologo
  Restore completed in 380.78 ms for /Users/.../Acme.Example.UnitTests/Acme.Example.UnitTests.csproj.
  Restore completed in 380.78 ms for /Users/.../Acme.Example.Api/Acme.Example.Api.csproj.
  Acme.Example.Api -> /Users/.../Acme.Example.Api/bin/Debug/netcoreapp3.0/Acme.Example.Api.dll
  Acme.Example.UnitTests -> /Users/.../Acme.Example.UnitTests/bin/Debug/netcoreapp3.0/Acme.Example.UnitTests.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:01.33
dotnet run -p Acme.Example.Api/Acme.Example.Api.csproj
[13:20:29 INF] Initialized Tracer(ServiceName=Mcg.Webservice, Version=CSharp-0.3.6.0, Reporter=RemoteReporter(Sender=UdpSender(UdpTransport=ThriftUdpClientTransport(Client=127.0.0.1:6831))), Sampler=ConstSampler(True), IPv4=167772263, Tags=[jaeger.version, CSharp-0.3.6.0], [hostname, Slartibartfast.local], [ip, 10.0.0.103], ZipkinSharedRpcSpan=False, ExpandExceptionLogs=False, UseTraceId128Bit=False)
[13:20:30 INF] Now listening on: http://localhost:5000
[13:20:30 INF] Application started. Press Ctrl+C to shut down.
[13:20:30 INF] Hosting environment: Production
[13:20:30 INF] Content root path: /Users/.../Acme.Example.Api
```

Open a browser of your choice and navigate to [http://localhost:5000](http://localhost:5000)
