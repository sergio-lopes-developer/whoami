# Architecture

## Introduction

This document provides a high-level overview of the architecture used by the **WhoAmI** project.

Rather than explaining concepts such as [**Domain-Driven Design (DDD)**](https://www.domainlanguage.com/), [**Clean Architecture**](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html), or [**Command Query Responsibility Segregation (CQRS)**](https://martinfowler.com/bliki/CQRS.html), this document explains how those principles are applied throughout the project.

For implementation details of individual projects, see the related documentation at the end of this document.

---

## Architectural Goals

The architecture aims to:

- Keep business rules independent of frameworks and infrastructure.
- Separate application workflows from domain logic.
- Make infrastructure replaceable.
- Support multiple presentation layers.
- Encourage maintainability and testability.
- Keep dependencies pointing toward the core business model.

---

## Architectural Overview

The project is organized around the principles of Clean Architecture.

<p align="center">
    <img
        src="dependency-map.svg"
        alt="Dependency Map"
    />
</p>

All dependencies point toward the Domain.

The Domain project is the core of the application and contains the business model.

The Application project orchestrates use cases by coordinating domain objects.

The Infrastructure project implements technical concerns such as persistence and external integrations.

The Bootstrap project serves as the composition root responsible for assembling the application.

Presentation projects provide different ways to interact with the application while remaining free of business rules.

---

## Architectural Principles

The architecture emphasizes clear boundaries and explicit responsibilities over convenience.

Business rules belong exclusively to the Domain layer.

Application workflows belong to the Application layer.

Technical concerns are isolated within Infrastructure.

Presentation projects remain thin, translating user interactions into application requests without containing business logic.

These principles are enforced through the following dependency rules:

- Domain depends on nothing.
- Application depends only on Domain.
- Infrastructure depends on Application and Domain.
- Bootstrap depends on Application and Infrastructure.
- Presentation projects depend on Bootstrap.
- Dependencies always point toward the Domain.

As the project evolves, new features should be added by extending existing layers rather than changing their responsibilities.

This approach keeps the business model independent of frameworks and allows the application to evolve without compromising its architectural boundaries.

---

## Project Structure

```text
whoami/
├── docs/
├── src/
│   ├── WhoAmI.Application/
│   ├── WhoAmI.Bootstrap/
│   ├── WhoAmI.Domain/
│   └── WhoAmI.Infrastructure/
└── tests/
    ├── WhoAmI.Application.Tests/
    ├── WhoAmI.Domain.Tests/
    ├── WhoAmI.Infrastructure.Tests/
    └── WhoAmI.Testing/
```

The repository is organized into three main areas.

### docs

Contains architecture documentation, diagrams, and design decisions.

### src

Contains the application source code.

Each project has a single architectural responsibility.

### tests

Contains unit and integration tests that verify the behavior of the project.

---

## Layer Responsibilities

### Domain

The Domain layer represents the business itself.

It owns:

- business rules;
- aggregates;
- entities;
- value objects;
- domain events;
- domain exceptions.

The Domain never depends on higher layers.

---

### Application

The Application layer coordinates use cases.

It defines application workflows while remaining independent of persistence, presentation frameworks, and infrastructure implementations.

Business rules remain inside the Domain.

---

### Infrastructure

The Infrastructure layer provides technical implementations required by the Application.

Responsibilities typically include:

- persistence;
- repositories;
- Entity Framework Core configuration;
- Dapper query implementations;
- external services.

Infrastructure depends on the Application and Domain layers, never the opposite.

---

### Bootstrap

The Bootstrap layer acts as the application's composition root.

Its responsibility is to configure the application by:

- registering dependency injection services;
- wiring together Application and Infrastructure implementations;
- configuring external frameworks and libraries;
- providing a single composition root for presentation projects.

Presentation projects use the Bootstrap layer to initialize the application without containing dependency injection or infrastructure configuration themselves.

---

### Presentation

Presentation projects expose the application to users.

Current and planned presentation layers include:

- CLI
- REST API
- Web application

Presentation projects translate user input into application requests and present the results returned by the Application layer.

---

## Design Decisions

Several architectural decisions intentionally shape the project.

### Commands and Queries

Commands modify state.

Queries retrieve information.

Keeping them separate simplifies reasoning about application behavior.

---

### Domain-Centric Design

Business rules are always enforced by the Domain.

The Application coordinates use cases but does not own business behavior.

---

### Multiple Presentation Layers

The architecture allows different user interfaces to reuse the same application and domain logic.

This avoids duplication while keeping user interface concerns isolated.

---

## Evolution

The current implementation is centered on the Domain and Application layers, with additional projects being introduced incrementally.

Future work includes additional projects such as:

- Bootstrap
- Command-line interface
- REST API
- Web application
- Desktop application

The architecture has been designed so these additions can be introduced without changing the responsibilities of the existing layers.

---

## Related Documentation

- [WhoAmI — Project Overview](../../README.md)
- [WhoAmI.Application](../../src/WhoAmI.Application/README.md)
- [WhoAmI.Bootstrap](../../WhoAmI.Bootstrap/README.md)
- [WhoAmI.Domain](../../src/WhoAmI.Domain/README.md)
- [WhoAmI.Infrastructure](../../src/WhoAmI.Infrastructure/README.md)

---

## References

- [Domain-Driven Design](https://www.domainlanguage.com/) — Eric Evans
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) — Robert C. Martin
- [CQRS](https://martinfowler.com/bliki/CQRS.html) — Martin Fowler

---

Built with ❤️ on **Linux** using **JetBrains Rider**.

> "Trust in the LORD with all your heart; and lean not unto your own understanding."
>
> — Proverbs 3:5
