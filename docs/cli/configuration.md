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

The CLI uses the `DefaultConnection` connection string to configure the SQLite database.

By default, the connection is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=whoami.db;"
  }
}
```

This relative path creates the SQLite database file in the application's working directory if it does not already exist.

When running in the `Development` environment, values from `appsettings.Development.json` override values from `appsettings.json`.

To customize development settings, create this file by copying:

```text
src/WhoAmI.CLI/appsettings.Development.example.json
```

to:

```text
src/WhoAmI.CLI/appsettings.Development.json
```

Then update the settings according to your local environment.

```json
{
  "ConnectionStrings": {
    "DefaultConnection":
      "Data Source=/absolute/path/to/whoami.db;"
  }
}
```

Using an environment-specific configuration file allows developers to customize local settings without modifying the repository's default configuration.

The example file provides a template for local development settings and must not contain machine-specific configuration.

---

## Database Initialization

The CLI uses Entity Framework Core migrations to manage the database schema.

When the application starts, it automatically:

- Creates the SQLite database if it does not exist.
- Applies any pending migrations.

For first-time setup, simply run the CLI as described in the [Quick Start](./quick-start.md) guide.

---

## Logging

The CLI uses **Serilog** to write structured log files.

By default, log files are written to the `logs` directory in the application's working directory.

Generated log files follow this naming pattern:

```text
logs/
    log-yyyyMMdd.json
    log-yyyyMMdd.txt
```

The CLI writes both human-readable text logs and structured JSON logs to support troubleshooting and automated log analysis.

Logging features include:

- Structured JSON logs.
- Human-readable text logs.
- Daily rolling log files.
- Sensitive data masking.
- Execution logging.
- Exception logging.

By default, the CLI logs application events at the `Information` level.

Entity Framework Core infrastructure logging is reduced to minimize log noise while still reporting database errors.

Sensitive information is automatically masked before being written to the log files, helping prevent accidental exposure of confidential data.

---

<div align="center">

| &nbsp;&nbsp;&nbsp; ⬅️ Previous Page &nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Next Page ➡️ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; |
| :----------------------------------------------------: | :------------------------------------------------------------------------: |
| [Quick Start](./quick-start.md) | [Usage](./usage.md) |

</div>
