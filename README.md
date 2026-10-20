<div align="center">

# WhoAmI

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet) [![CI](https://github.com/sergio-lopes-developer/whoami/actions/workflows/ci.yml/badge.svg)](https://github.com/sergio-lopes-developer/whoami/actions/workflows/ci.yml) ![Platform](https://img.shields.io/badge/platform-Linux%20%7C%20Windows%20%7C%20macOS-blue) ![License](https://img.shields.io/badge/license-MIT-green) ![Portfolio](https://img.shields.io/badge/Portfolio-Developer-blue)

</div>

**WhoAmI** is a .NET 10 application for managing developer profiles, built as a portfolio project to demonstrate modern software architecture.

<p align="center">
  <img
    src="docs/media/demo.gif"
    alt="WhoAmI CLI demonstration"
    width="900"
  >
</p>

---

## Features

- Manage developer profiles through a command-line interface.
- Built with [**Domain-Driven Design (DDD)**](https://www.domainlanguage.com/), [**Command Query Responsibility Segregation (CQRS)**](https://martinfowler.com/bliki/CQRS.html) and [**Clean Architecture**](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html).
- Cross-platform (.NET 10).
- Comprehensive unit and integration tests.
- Designed to support multiple clients through independent presentation layers.

---

## Project Roadmap

| Category        | Scope                                                                                 |
|-----------------|------------------------------------------------------------------------------------------|
| Architecture    | ✅ Domain layer<br>✅ Application layer<br>✅ Infrastructure layer<br>✅ Bootstrap       |
| Interfaces      | ✅ Command-line interface<br>⏳ REST API<br>⏳ Web application<br>⏳ Desktop application |
| Domain Features | ✅ Profile management<br>⏳ Skills management                                            |
| Enhancements    | ⏳ Pagination                                                                            |

> [!NOTE]
>
> **Legend:** ✅ Completed · ⏳ Planned

For detailed tracking of planned and completed work, see the GitHub Project:

[WhoAmI Development Roadmap](https://github.com/users/sergio-lopes-developer/projects/2)

Project development follows a transparent workflow using GitHub Issues, Milestones, Pull Requests, GitHub Projects, and GitHub Actions.

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
