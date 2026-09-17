# Configuration

[🏠 Home](../../README.md) / [CLI](../../src/WhoAmI.CLI/README.md) / **Configuration**

---

## Introduction

This guide explains how the **WhoAmI CLI** loads its configuration and how to customize the database connection and logging behavior.

All configuration files are located in the `src/WhoAmI.CLI` project.

---

## Configuration Sources

The CLI uses the standard .NET configuration system.

Configuration is loaded in the following order:

- `appsettings.json`
- `appsettings.Development.json` (when running in the Development environment)

Settings loaded later override values loaded earlier.

The active environment is determined by the standard `DOTNET_ENVIRONMENT` environment variable.

---

## Database Connection

When the environment is `Development`, `appsettings.Development.json` overrides settings from `appsettings.json`.

If no environment-specific configuration file is present, the CLI uses the settings from `appsettings.json`.

Example `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=whoami.db;"
  }
}
```

This relative path specifies that the SQLite database file is stored in the application's current working directory.

Example `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection":
      "Data Source=/absolute/path/to/whoami.db;"
  }
}
```

---

## Database Initialization

The CLI uses Entity Framework Core migrations to manage the database schema.

When the application starts, it automatically ensures that the SQLite database exists and applies any pending Entity Framework Core migrations.

For first-time setup, simply run the CLI as described in the [Quick Start](./quick-start.md) guide.

---

## Logging

The CLI uses **Serilog** to write structured log files.

By default, log files are written to the `logs` directory in the application's working directory.

The generated log files follow this structure:

```text
logs/
    log-yyyyMMdd.json
    log-yyyyMMdd.txt
```

The CLI writes both human-readable text logs and structured JSON logs to support troubleshooting and automated log analysis.

Logging features include:

- Structured JSON logs
- Human-readable text logs
- Daily rolling log files
- Sensitive data masking
- Execution logging
- Exception logging

By default, the CLI logs application events at the `Information` level. Entity Framework Core infrastructure logging is reduced to minimize log noise while still reporting database errors.

Sensitive information is automatically masked before being written to the log files, helping prevent the accidental exposure of confidential data.

---

<div align="center">

| &nbsp;&nbsp;&nbsp; ⬅️ Previous Page &nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Next Page ➡️ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; |
| :----------------------------------------------------: | :------------------------------------------------------------------------: |
| [Quick Start](./quick-start.md) | [Usage](./usage.md) |

</div>

