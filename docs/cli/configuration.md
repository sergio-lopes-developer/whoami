# Configuration

[🏠 Home](../../README.md) / [CLI](../../src/WhoAmI.CLI/README.md) / **Configuration**

---

## Connection String

The CLI reads the database connection from `appsettings.json`.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=whoami.db;"
  }
}
```

For local development you can override the connection string using `appsettings.Development.json`.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection":
      "Data Source=/absolute/path/to/whoami.db;"
  }
}
```

---

## Database

Before using the CLI, the database must exist.

The database schema is created from the Entity Framework Core migrations contained in the **WhoAmI.Infrastructure** project.

After creating the database, the CLI automatically initializes the application during startup.

---

## Logging

The CLI uses **Serilog**.

Log files are written to:

```
logs/
    log-yyyyMMdd.json
    log-yyyyMMdd.txt
```

Features include:

- Structured logging
- Daily rolling log files
- Sensitive data masking
- Execution logging
- Exception logging

---

<div align="center">

| &nbsp;&nbsp;&nbsp; ⬅️ Previous Page &nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Next Page ➡️ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; |
| :----------------------------------------------------: | :------------------------------------------------------------------------: |
| [Quick Start](./quick-start.md)                        | [Usage](./usage.md)                                                        |

</div>

