# Quick Start

[🏠 Home](../../README.md) / [CLI](../../src/WhoAmI.CLI/README.md) / **Quick Start**

---

## Introduction

This guide explains how to set up and run the **WhoAmI CLI** for the first time.

---

## Prerequisites

Before getting started, ensure the following tools are installed:

- [.NET 10 SDK](https://dotnet.microsoft.com/download) — Download and installation instructions for Linux, macOS, and Windows.
- [Git](https://git-scm.com/downloads) — Download and installation instructions for Linux, macOS, and Windows.

Verify the installation by running:

```bash
dotnet --version
git --version
```

Each command should print the installed version.

The repository includes a [global.json](../../global.json) file that defines the .NET SDK version used by the project.

---

## 1. Clone the Repository

Open a terminal and clone the repository:

```shell
git clone https://github.com/sergio-lopes-developer/whoami.git
cd whoami
```

---

## 2. Database Setup

The CLI uses SQLite with the default connection string configured in:

```text
src/WhoAmI.CLI/appsettings.json
```

The default configuration creates the database file automatically in the application's working directory.

To customize the database location or use environment-specific configuration, see the [Configuration](./configuration.md) guide.

---

## 3. Build the Solution

Restore the project dependencies:

```shell
dotnet restore
```

Build the solution:

```shell
dotnet build
```

---

## 4. Verify Installation

Run the following command:

```shell
dotnet run --project src/WhoAmI.CLI -- profile list
```

If the database does not exist, the CLI creates it automatically and applies any pending migrations.

If the database is empty, you should see:

```text
No profiles were found.
```

If the command completes successfully, your **WhoAmI CLI** setup is complete.

---

## Next Steps

After verifying the installation, you may want to:

- Learn how to [configure the CLI](./configuration.md).
- Explore the [command reference](usage.md).
- Learn about the [project's architecture](../architecture/README.md).

---

<div align="center">

| &nbsp;&nbsp;&nbsp; ⬅️ Previous Page &nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Next Page ➡️ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; |
|:------------------------------------------------------:|:--------------------------------------------------------------------------:|
| [CLI](../../src/WhoAmI.CLI/README.md) | [Configuration](./configuration.md) |

</div>
