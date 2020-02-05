# Mcg.Webservice.Cncf Solution

## Prerequisites (Windows Only)

- Ensure **[Chocolatey](https://chocolatey.org/install)** is installed.

- Install **[GNU Make 4.2.1](https://chocolatey.org/packages/make)**:

  ```shell
   Windows PowerShell
   Copyright (C) Microsoft Corporation. All rights reserved.

   PS C:\> choco install make -y
  ```

- _(Optional)_ **[cmder](https://cmder.net/)** A very handy, convenient terminal for Windows.  I find it useful when working with the `dotnet` commandline.

  ```shell
   Windows PowerShell
   Copyright (C) Microsoft Corporation. All rights reserved.

   PS C:\> choco install cmder -y
  ```

## Prerequesites (All Platforms,)

- **[Dotnet Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)**: This is the SDK used to build the solution.
- **[Coverlet](https://github.com/tonerdo/coverlet?WT.mc_id=-blog-scottha#coverlet)**: This will calculate code coverage when using the `make test` command.

  ```shell
   Windows PowerShell
   Copyright (C) Microsoft Corporation. All rights reserved.

   PS C:\> dotnet tool install --global coverlet.console
  ```

- **[Docker Desktop](https://www.docker.com/products/docker-desktop)**: for building and running the solution's container.
- **[Visual Studio Code](https://visualstudio.microsoft.com/vs/)**: for editing of Readme files and Makefiles, though not absolutely necessary.
- (_Optional_) **[Visual Studio 2019](https://visualstudio.microsoft.com/vs/)**: If using Visual Studio, then you'll need 2019+ since it is required to open .NET Core 3.x-based solutions.

---

## Solution Structure

| **Solution Items**                                                                   | **Description**                                                                                                                 |
| ------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------- |
| **[Mcg.Webservice.Cncf.Api](./Mcg.Webservice.Cncf.Api/Readme.md)**                     | This is the core project; the project that will produce the deliverable to be deployed.                                         |
| **[Mcg.Webservice.Cncf.UnitTests](./Mcg.Webservice.Cncf.UnitTests/Readme.md)** | This project, as the name suggests, are the **_unit tests_** for the project.                                                   |
| [Makefile](./Makefile)                                                               | This file contains several commandline make command to make compiling, testing, and running the solution easier.                |
| [docker-compose.yml](./docker-compose.yml)                                           | Invoke `docker-compose up` in the solution root to launch the solution in a container along with the other supporting services. |
| [prometheus.yml](./prometheus.yml)                                                   | Configures prometheus to be able to access the container running the web service.                                               |
|                                                                                      |                                                                                                                                 |

## Make Commands

| **Command**    | **Description**                                                                                               |
| -------------- | ------------------------------------------------------------------------------------------------------------- |
| `make`         | The default action. Causes the project to be cleaned and re-build                                             |
| `make clean`   | Deletes the previous build                                                                                    |
| `make restore` | Restores any missing packages                                                                                 |
| `make build`   | Cleans, restores, builds, then launches the output binary.                                                    |
| `make run`     | Cleans, restores, and builds a binary.                                                                        |
| `make test`    | Cleans, restores, builds, and executes the unit tests.                                                        |
| `make publish` | Publishes a binary build for musl (Alpine) linux                                                              |
| `make docker`  | Creates a docker image and calls a docker-compose file to start up all supporting services and the container. |
|                |                                                                                                               |

 :warning: On Mac you may need to use **`gmake`** instead of **`make`**
