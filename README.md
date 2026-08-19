# WhoAmI

WhoAmI is a portfolio project with two main goals.

## Goals

### 1. Interactive developer profile

A command-line application that serves as an interactive version of my developer profile. When executed, it displays information about me, my contact details, and links to my work. The application is also used as the banner for my LinkedIn profile.

### 2. Software architecture

A real-world .NET application built using [**Domain-Driven Design (DDD)**](https://www.domainlanguage.com/), [**Command Query Responsibility Segregation (CQRS)**](https://martinfowler.com/bliki/CQRS.html), and [**Clean Architecture**](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html). The project demonstrates architectural principles, maintainability, and testing through a practical, evolving application, with additional interfaces such as an API and a web application planned.

## Why WhoAmI

The project is designed to introduce who I am as a software developer. It displays my profile information while also showcasing my programming and software architecture skills through a real-world application.

The name is inspired by the classic Linux `whoami` command, which prints the name of the current user. In the same spirit, this project introduces me through both its output and its source code.

## Repository Guide

| Project | Description |
|---------|-------------|
| docs | Architecture and project documentation. |
| WhoAmI.Domain | Domain model and business rules. |
| WhoAmI.Application | Application layer, CQRS, validation, and use cases. |

---

## Project Structure

```
whoami/
├── docs/
├── src/
│   ├── WhoAmI.Application/
│   └── WhoAmI.Domain/
└── tests/
    ├── WhoAmI.Application.Tests/
    ├── WhoAmI.Domain.Tests/
    └── WhoAmI.Testing/
```

---

## Documentation

- [Architecture](docs/architecture/README.md)
- [WhoAmI.Application](src/WhoAmI.Application/README.md)
- [WhoAmI.Domain](src/WhoAmI.Domain/README.md)
- [WhoAmI.Infrastructure](src/WhoAmI.Infrastructure/README.md)

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

Built with ❤️ on Linux using JetBrains Rider.

> "Be not overcome of evil, but overcome evil with good."
>
> — Romans 12:21
