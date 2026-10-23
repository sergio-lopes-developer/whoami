# WhoAmI.CLI

[🏠 Home](../../README.md) / **CLI**

---

## Introduction

**WhoAmI.CLI** is the command-line interface for the WhoAmI application. It provides a cross-platform, script-friendly interface for managing developer profiles while delegating all business logic to the application layer.

---

## Responsibilities

The CLI is responsible for the following tasks:

- Parsing command-line arguments.
- Mapping user input to application commands and queries.
- Formatting output for the terminal.
- Returning appropriate exit codes.
- Delegating business logic to the application layer.

> [!NOTE]
>
> The CLI intentionally contains no business rules. All business logic resides in the application and domain layers.

---

## CLI Documentation

- [Quick Start](../../docs/cli/quick-start.md)
- [Configuration](../../docs/cli/configuration.md)
- [Usage](../../docs/cli/usage.md)

---

## Related Documentation

- [Project Overview](../../README.md) — Introduces the project, demonstrates its functionality, and outlines its roadmap.
- [Architecture](../../docs/architecture/README.md) — Explains the application's architecture, layer responsibilities, design decisions, and dependency structure.
- [WhoAmI.Domain](../WhoAmI.Domain/README.md) — Defines the business model, entities, value objects, and domain events.
- [WhoAmI.Application](../WhoAmI.Application/README.md) — Implements application workflows, CQRS, validation, and the Result pattern.
- [WhoAmI.Infrastructure](../WhoAmI.Infrastructure/README.md) — Provides persistence, repositories, queries, and infrastructure services.
- [WhoAmI.Bootstrap](../WhoAmI.Bootstrap/README.md) — Configures dependency injection and application composition.

---

Built with ❤️ on **Linux** using **JetBrains Rider**.

> *“The Lord is my shepherd, I lack nothing.”*
>
> — **Psalm 23:1**
