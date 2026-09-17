# Quick Start

[🏠 Home](../../README.md) / [CLI](../../src/WhoAmI.CLI/README.md) / **Quick Start**

---

## Introduction

This guide explains how to set up and run the **WhoAmI CLI** for the first time.

---

## Prerequisites

Before starting, ensure you have the following installed:

- .NET 10 SDK.
- Git.

---

## 1. Clone the Repository

Open a terminal and clone the repository:

```shell
git clone https://github.com/sergio-lopes-developer/whoami.git
cd whoami
```

---

## 2. Configure the Database Connection

For development, copy:

```text
src/WhoAmI.CLI/appsettings.Development.example.json
```

to:

```text
src/WhoAmI.CLI/appsettings.Development.json
```

Then update the connection string to specify the location of the SQLite database file.

For example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection":
      "Data Source=/absolute/path/to/whoami.db;"
  }
}
```

The development configuration overrides the default connection string without modifying the repository's default configuration.

For more information, see the [Configuration](./configuration.md) guide.

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

If the database does not exist, the CLI automatically creates it and applies any pending Entity Framework Core migrations.

If the database is empty, you should see:

```text
No profiles were found.
```

If the command completes successfully, your **WhoAmI CLI** installation is ready to use.

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

