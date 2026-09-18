<div align="center">

# WhoAmI

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet) ![Platform](https://img.shields.io/badge/Tested-Linux-success?logo=linux) ![License](https://img.shields.io/badge/license-MIT-green) ![Portfolio](https://img.shields.io/badge/Portfolio-Developer-blue)

</div>

**WhoAmI** is a .NET portfolio project that serves two purposes: providing a command-line application for managing developer profiles and demonstrating modern software architecture through a real-world application.

Although the project is used as my personal portfolio, it is designed so that anyone can create, manage, and display their own developer profile.

---

## Why WhoAmI

While reviewing my LinkedIn profile, I noticed that my banner was just a generic circuit-board image. Rather than leaving that space purely decorative, I wanted to turn it into something meaningful—something that could introduce me as a software developer while also demonstrating what I can build.

That idea evolved into a command-line application capable of managing and displaying developer profiles. By using my own profile as the initial data, I could capture the terminal output and use it as my LinkedIn banner.

The name is inspired by the classic Linux `whoami` command, which prints the name of the current user of the operating system. In the same spirit, this project introduces developers through both its output and its source code: the CLI presents profile information, while the implementation demonstrates software architecture, engineering practices, and code quality.

Although the screenshots and examples throughout this repository use my own profile, the application itself is not tied to my information. Any developer can use it to create, manage, and display their own profile.

---

## Goals

### 1. Interactive developer profiles

A command-line application that allows developers to create, store, update, and display their own developer profiles.

The CLI provides a simple interface for managing profile information such as names, email addresses, and social links. My own profile is used as the primary example and serves as the banner for my LinkedIn profile.

### 2. Software architecture

A real-world .NET application built using [**Domain-Driven Design (DDD)**](https://www.domainlanguage.com/), [**Command Query Responsibility Segregation (CQRS)**](https://martinfowler.com/bliki/CQRS.html), and [**Clean Architecture**](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html).

The project demonstrates architectural principles, maintainability, testing practices, and separation of concerns through a practical, evolving application. While the current interface is a command-line application, additional interfaces such as a Web API and a web application are planned, allowing multiple clients to share the same application and domain layers.

---

## Repository Guide

```text
whoami/
├── docs/
├── src/
│   ├── WhoAmI.Application/
│   ├── WhoAmI.Bootstrap/
│   ├── WhoAmI.CLI/
│   ├── WhoAmI.Domain/
│   └── WhoAmI.Infrastructure/
└── tests/
    ├── WhoAmI.Application.Tests/
    ├── WhoAmI.CLI.Tests/
    ├── WhoAmI.Domain.Tests/
    ├── WhoAmI.Infrastructure.Tests/
    └── WhoAmI.Testing/
```

The repository is organized into three top-level directories: `docs/`, `src/`, and `tests/`.

- **`docs/`** — Project documentation, including architecture, design decisions, diagrams, and development guides.
- **`src/`** — Application source code. Each project has a single architectural responsibility following the principles of Clean Architecture.
  - **`src/WhoAmI.Domain/`** — Domain model, business rules, entities, value objects, and aggregates.
  - **`src/WhoAmI.Application/`** — CQRS, use cases, validation, command handlers, and application services.
  - **`src/WhoAmI.Infrastructure/`** — EF Core, repositories, query handlers, Dapper, database access, and infrastructure services.
  - **`src/WhoAmI.Bootstrap/`** — Composition root and dependency injection configuration.
  - **`src/WhoAmI.CLI/`** — Command-line interface and command implementations.
- **`tests/`** — Unit tests, integration tests, and shared testing utilities.

---

## Roadmap

- **Architecture**
  - ✅ Domain layer
  - ✅ Application layer
  - ✅ Infrastructure layer
  - ✅ Bootstrap
- **Interfaces**
  - ✅ Command-line interface
  - ⏳ REST API
  - ⏳ Web application
  - ⏳ Desktop application
- **Business Features**
  - ✅ Profile management
  - ⏳ Skills management
- **Enhancements**
  - ⏳ Pagination

> [!NOTE]
>
> **Legend:** ✅ Completed · ⏳ Planned

---

## Documentation

- [Architecture](./docs/architecture/README.md) — High-level architecture, layer responsibilities, design decisions, and dependency structure.
- [WhoAmI.Domain](./src/WhoAmI.Domain/README.md) — Business model, entities, value objects, aggregates, and domain events.
- [WhoAmI.Application](./src/WhoAmI.Application/README.md) — Application workflows, CQRS, validation, and the Result pattern.
- [WhoAmI.Infrastructure](./src/WhoAmI.Infrastructure/README.md) — Persistence, repositories, queries, EF Core configuration, and infrastructure services.
- [WhoAmI.Bootstrap](./src/WhoAmI.Bootstrap/README.md) — Dependency injection and application composition.
- [WhoAmI.CLI](./src/WhoAmI.CLI/README.md) — Command-line interface and available commands.

---

## References

- [Domain-Driven Design](https://www.domainlanguage.com/) — Eric Evans
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) — Robert C. Martin
- [CQRS](https://martinfowler.com/bliki/CQRS.html) — Martin Fowler

---

## License

This project is licensed under the MIT License.

If you find this project useful, you're welcome to use it as permitted by the license. If you build upon it, I'd appreciate proper attribution, although it is not required by the license.

---

Built with ❤️ on **Linux** using **JetBrains Rider**.

> *“Be not overcome of evil, but overcome evil with good.”*
>
> — **Romans 12:21**
