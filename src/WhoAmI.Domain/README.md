# WhoAmI.Domain

[🏠 Home](../../README.md) / **Domain**

---

## Introduction

The **WhoAmI.Domain** project contains the application's domain model.

It defines the entities, value objects, aggregates, domain events, exceptions, and business rules that make up that model.

It follows principles from [**Domain-Driven Design (DDD)**](https://www.domainlanguage.com/) and [**Clean Architecture**](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html), with the business model kept at the center of the system.

---

## Responsibilities

The Domain layer is responsible for:

- Modeling the application's core business concepts.
- Defining and enforcing business rules.
- Protecting aggregate consistency.
- Representing business concepts as entities and value objects.
- Expressing meaningful business events through domain events.
- Reporting business rule violations through domain exceptions.
- Remaining independent of application workflows, persistence, presentation, and infrastructure concerns.

> [!NOTE]
>
> The Domain layer answers the question:
>
> **What is a valid state of the WhoAmI domain, and how can that state change?**

---

## Domain Structure

The Domain project is organized into domain models and a set of shared building blocks:

```text
WhoAmI.Domain/
├── ...
└── Shared/
    ├── Base/
    ├── Constraints/
    ├── Exceptions/
    └── Guards/
```

Each root-level directory represents a domain model organized around one or more aggregates.

The `Shared` directory contains reusable abstractions and building blocks that can be shared across different parts of the domain.

Core building blocks include:

- Aggregates and aggregate roots.
- Entities.
- Value objects.
- Domain events.
- Domain exceptions.
- Domain constraints.
- Domain guards.
- Shared domain abstractions.

The Domain layer does **not** contain:

- Application use cases.
- Commands or queries.
- Command or query dispatching.
- Application pipelines.
- Persistence implementations.
- Entity Framework Core configurations.
- Logging.
- CLI or API concerns.
- Infrastructure services.

This separation keeps the business model independent of technical implementation details and reinforces the architectural boundaries of the application.

---

## Aggregates

An **aggregate** is a consistency boundary within the domain.

Each aggregate has an **aggregate root**, which is the primary entry point for interacting with the aggregate.

The aggregate root is responsible for maintaining the consistency of the aggregate and exposing the operations that can change its state.

---

## Entities

An **entity** is a domain object whose identity is significant regardless of its current state.

Two entities of the same type are equal when they have the same identifier.

Entities of different types are not considered equal solely because they happen to share the same identifier.

---

## Value Objects

Value objects represent domain concepts whose identity is defined by their values rather than by an identifier.

They encapsulate domain-specific behavior and ensure that only valid values become part of the domain model.

---

## Domain Events

Domain events represent facts about meaningful occurrences within the domain.

They are immutable records that describe those domain occurrences.

---

## Domain Exceptions

Domain exceptions represent violations of domain rules.

The Domain layer reports these violations without translating them into application- or presentation-specific representations.

Higher architectural layers are responsible for handling and translating domain exceptions appropriately.

---

## Related Documentation

- [Repository Guide](../../README.md) — Repository overview and getting started.
- [Architecture](../../docs/architecture/README.md) — High-level architecture, layer responsibilities, design decisions, and dependency structure.
- [WhoAmI.Application](../WhoAmI.Application/README.md) — Application workflows, CQRS, validation, and the Result pattern.
- [WhoAmI.Infrastructure](../WhoAmI.Infrastructure/README.md) — Persistence, repositories, queries, EF Core configuration, and infrastructure services.
- [WhoAmI.Bootstrap](../WhoAmI.Bootstrap/README.md) — Dependency injection and application composition.
- [WhoAmI.CLI](../WhoAmI.CLI/README.md) — Command-line interface and available commands.

---

## References

- [Domain-Driven Design](https://www.domainlanguage.com/) — Eric Evans
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) — Robert C. Martin

---

Built with ❤️ on **Linux** using **JetBrains Rider**.

> *“I can do all things through Christ who strengthens me.”*
>
> — **Philippians 4:13**
