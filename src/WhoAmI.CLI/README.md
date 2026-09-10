# WhoAmI.CLI

[🏠 Home](../../README.md) / **CLI**

---

## Introduction

The **WhoAmI CLI** is the command-line interface for the WhoAmI project.

It provides a terminal-based interface for creating, retrieving, listing, and updating developer profiles without requiring a graphical user interface.

The CLI is designed to be:

- Script-friendly.
- Cross-platform.
- Easy to extend.
- Consistent.

---

## Responsibilities

The CLI is responsible for the following tasks:

- Parsing command-line arguments.
- Mapping user input to application commands and queries.
- Formatting output for the terminal.
- Returning appropriate exit codes.
- Delegating business logic to the application layer.

The CLI receives user input, dispatches commands and queries through the application layer, and renders the results in a terminal-friendly format.

> [!NOTE]
>
> The CLI intentionally contains no business rules. All business logic resides in the application and domain layers.

---

## Features

The CLI provides commands for:

- **Profile management**
  - Create, retrieve, list, and update developer profiles.
- **Version information**
  - Display the application version.

---

## User Guide

- [Quick Start](../../docs/cli/quick-start.md)
- [Configuration](../../docs/cli/configuration.md)
- [Usage](../../docs/cli/usage.md)

---

## Related Documentation

- [Repository Guide](../../README.md) — Repository overview and getting started.
- [Architecture](../../docs/architecture/README.md) — High-level architecture, layer responsibilities, design decisions, and dependency structure.
- [WhoAmI.Domain](../WhoAmI.Domain/README.md) — Business model, entities, value objects, aggregates, and domain events.
- [WhoAmI.Application](../WhoAmI.Application/README.md) — Application workflows, CQRS, validation, and the Result pattern.
- [WhoAmI.Infrastructure](../WhoAmI.Infrastructure/README.md) — Persistence, repositories, queries, EF Core configuration, and infrastructure services.
- [WhoAmI.Bootstrap](../WhoAmI.Bootstrap/README.md) — Dependency injection and application composition.

---

## References

- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) — Robert C. Martin
- [CQRS](https://martinfowler.com/bliki/CQRS.html) — Martin Fowler

---

Built with ❤️ on **Linux** using **JetBrains Rider**.

> *“The Lord is my shepherd, I lack nothing.”*
>
> — **Psalm 23:1**
