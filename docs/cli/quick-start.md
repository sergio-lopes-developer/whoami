# Quick Start

[🏠 Home](../../README.md) / [CLI](../../src/WhoAmI.CLI/README.md) / **Quick Start**

---

## Introduction

This guide will help you configure and run the **WhoAmI CLI** for the first time.

## Prerequisites

Before starting, ensure you have the following installed:

* .NET 10 SDK
* SQLite
* Git

You also need a local copy of the complete **WhoAmI** solution.

---

## 1. Clone the Repository

```bash
git clone <repository-url>
cd whoami
```

---

## 2. Configure the Database Connection

The CLI reads its database connection from `appsettings.json`.

For development, create an `appsettings.Development.json` file in the **WhoAmI.CLI** project.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection":
      "Data Source=/absolute/path/to/whoami.db;"
  }
}
```

Replace the path with the location where your SQLite database should be stored.

---

## 3. Create the Database

The database schema is managed by Entity Framework Core migrations located in the **WhoAmI.Infrastructure** project.

Run the project's migration process to create the SQLite database before using the CLI.

> **Note**
>
> The CLI automatically initializes the database during startup, but it expects the migration history and schema to already exist.

---

## 4. Build the Solution

From the repository root:

```bash
dotnet build
```

---

## 5. Run the CLI

Navigate to the CLI project:

```bash
cd src/WhoAmI.CLI
```

Run the application:

```bash
dotnet run -- version
```

You should see output similar to:

```text
WhoAmI CLI - v0.1.0
```

---

## 6. Create Your First Profile

```bash
dotnet run -- profile create \
    -f John \
    -l Doe \
    -e john@example.com \
    -n https://linkedin.com/in/johndoe \
    -g https://github.com/johndoe
```

Expected output:

```text
Profile successfully created.
```

---

## 7. List Profiles

```bash
dotnet run -- profile list
```

Example:

```text
┌───┬──────────┬────────────────────┐
│ # │ Name     │ Email              │
├───┼──────────┼────────────────────┤
│ 1 │ John Doe │ john@example.com   │
└───┴──────────┴────────────────────┘
```

---

## 8. Display a Profile

```bash
dotnet run -- profile get \
    -e john@example.com
```

To display all available information:

```bash
dotnet run -- profile get \
    -e john@example.com \
    --verbose
```

---

## 9. Update a Profile

Update the email address:

```bash
dotnet run -- profile update email \
    -i <PROFILE_ID> \
    -e john.doe@example.com
```

Update the name:

```bash
dotnet run -- profile update name \
    -i <PROFILE_ID> \
    -f Jonathan \
    -l Doe
```

Update social links:

```bash
dotnet run -- profile update social-links \
    -i <PROFILE_ID> \
    -n https://linkedin.com/in/jonathandoe \
    -g https://github.com/jonathandoe
```

---

## Log Files

Execution logs are written automatically to the `logs` directory.

```
logs/
├── log-yyyyMMdd.json
└── log-yyyyMMdd.txt
```

---

## Next Steps

Once the CLI is running successfully, you may want to explore:

* The complete command reference in the [usage](usage.md).
* The application architecture documentation.
* The Domain, Application, Infrastructure, and Bootstrap projects to understand how requests flow through the system.

---

<div align="center">

| &nbsp;&nbsp;&nbsp; ⬅️ Previous Page &nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Next Page ➡️ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; |
| :----------------------------------------------------: | :------------------------------------------------------------------------: |
| [CLI Documentation](../../src/WhoAmI.CLI/README.md)    | [Configuration](./configuration.md)                                       |

</div>

