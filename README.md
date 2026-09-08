# WhoAmI

## Introduction

**WhoAmI** is a .NET portfolio project built around two primary goals: creating an interactive developer profile and showcasing software architecture through a real-world application.

---

## Why WhoAmI

The project is designed to introduce who I am as a software developer. It displays my profile information while also showcasing my programming and software architecture skills through a real-world application.

The name is inspired by the classic Linux `whoami` command, which prints the name of the current user. In the same spirit, this project introduces me through both its output and its source code.

---

## Goals

### 1. Interactive developer profile

A command-line application that serves as an interactive version of my developer profile. When executed, it displays information about me, my contact details, and links to my work. The application is also used as the banner for my LinkedIn profile.

### 2. Software architecture

A real-world .NET application built using [**Domain-Driven Design (DDD)**](https://www.domainlanguage.com/), [**Command Query Responsibility Segregation (CQRS)**](https://martinfowler.com/bliki/CQRS.html), and [**Clean Architecture**](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html). The project demonstrates architectural principles, maintainability, and testing practices through a practical, evolving application, with additional interfaces such as an API and a web application planned.

---

## Repository Guide

| Component             | Description                                                      |
|-----------------------|------------------------------------------------------------------|
| docs                  | Architecture and project documentation.                          |
| WhoAmI.Domain         | Domain model and business rules.                                 |
| WhoAmI.Application    | Application layer, CQRS, validation, and use cases.              |
| WhoAmI.Infrastructure | Persistence, repositories, queries, and infrastructure services. |
| WhoAmI.Bootstrap      | Composition root and dependency injection configuration.         |
| WhoAmI.CLI            | Command-line interface.                                          |

---

## Project Structure

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

The repository is organized into three main areas:

- **`docs/`** — Contains project documentation, including architecture, design decisions, diagrams, and development guides.
- **`src/`** — Contains the application source code. Each project has a single architectural responsibility.
- **`tests/`** — Contains unit and integration tests that verify the behavior of the project.

---

## Roadmap

### Architecture

- [x] Domain layer
- [x] Application layer
- [x] Infrastructure layer
- [x] Bootstrap

### Interfaces

- [x] Command-line interface
- [ ] REST API
- [ ] Web application
- [ ] Desktop application

### Business Features

- [x] Profile management
- [ ] Skills management

### Technical Improvements

- [ ] Pagination

---

## Documentation

- [Architecture](./docs/architecture/README.md)
  High-level architecture, layer responsibilities, design decisions, and dependency structure.

- [WhoAmI.Domain](./src/WhoAmI.Domain/README.md)
  Business model, entities, value objects, aggregates, and domain events.

- [WhoAmI.Application](./src/WhoAmI.Application/README.md)
  Application workflows, CQRS, validation, and the Result pattern.

- [WhoAmI.Infrastructure](./src/WhoAmI.Infrastructure/README.md)
  Persistence, repositories, queries, EF Core configuration, and infrastructure services.

- [WhoAmI.Bootstrap](./src/WhoAmI.Bootstrap/README.md)
  Dependency injection and application composition.

- [WhoAmI.CLI](./src/WhoAmI.CLI/README.md)
  Command-line interface and available commands.

---

## References

- [Domain-Driven Design](https://www.domainlanguage.com/) — Eric Evans
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) — Robert C. Martin
- [CQRS](https://martinfowler.com/bliki/CQRS.html) — Martin Fowler

---

## License

This project is licensed under the MIT License.

While you're welcome to use, modify, and learn from the code under the terms of the MIT License, I simply ask that you don't present substantial portions of it as your own work.

---

Built with ❤️ on **Linux** using **JetBrains Rider**.

> *“Be not overcome of evil, but overcome evil with good.”*
>
> — **Romans 12:21**
