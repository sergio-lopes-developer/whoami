# WhoAmI.CLI

[🏠 Home](../../README.md) / **CLI**

---

## Introduction

The **WhoAmI CLI** is the command-line interface for the WhoAmI project.

It provides a simple way to manage personal profiles from the terminal, allowing users to create, query, list, and update profile information without requiring a graphical interface.

---

## Design Goals

The CLI delegates all business logic to the application layer, making it a thin presentation layer responsible only for user interaction and output formatting.

The CLI is designed to be:

- Script-friendly
- Cross-platform
- Easy to extend
- Consistent

---

## Features

The CLI currently supports:

- Create profiles
- Retrieve profile information
- List all profiles
- Update profile names
- Update profile email addresses
- Update social links
- Display the application version

---

## Technology

| Component | Technology |
|-----------|------------|
| Framework | .NET 10 |
| CLI | Spectre.Console.Cli |
| Hosting | Microsoft.Extensions.Hosting |
| Logging | Serilog |
| Database | SQLite |
| Architecture | [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) |
| Pattern | [Command Query Responsibility Segregation (CQRS)](https://martinfowler.com/bliki/CQRS.html) |

---

## Project Structure

```
WhoAmI.CLI
├── Commands
│   ├── Core
│   └── Profiles
├── Configuration
├── DependencyInjection
│   └── Spectre
├── Enums
├── Hosting
├── Logging
├── Output
│   └── Renderers
├── Settings
│   └── Profiles
├── appsettings.json
└── Program.cs
```

---

## Documentation

- [Quick Start](../../docs/cli/quick-start.md)
- [Configuration](../../docs/cli/configuration.md)
- [Usage](../../docs/cli/usage.md)

---

## Related Documentation

- [Repository Guide](../../README.md)
  Repository overview and getting started.

- [Architecture](../../docs/architecture/README.md)
  High-level architecture, layer responsibilities, design decisions, and dependency structure.

- [WhoAmI.Domain](../WhoAmI.Domain/README.md)
  Business model, entities, value objects, aggregates, and domain events.

- [WhoAmI.Application](../WhoAmI.Application/README.md)
  Application workflows, CQRS, validation, and the Result pattern.

- [WhoAmI.Infrastructure](../WhoAmI.Infrastructure/README.md)
  Persistence, repositories, queries, EF Core configuration, and infrastructure services.

- [WhoAmI.Bootstrap](../WhoAmI.Bootstrap/README.md)
  Dependency injection and application composition.

---

## References

- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) — Robert C. Martin
- [CQRS](https://martinfowler.com/bliki/CQRS.html) — Martin Fowler

---

Built with ❤️ on **Linux** using **JetBrains Rider**.

> *“The Lord is my shepherd, I lack nothing.”*
>
> — **Psalm 23:1**
