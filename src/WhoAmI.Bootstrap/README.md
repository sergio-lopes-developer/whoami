# WhoAmI.Bootstrap

[🏠 Home](../../README.md) / **Bootstrap**

---

## Introduction

The **WhoAmI.Bootstrap** project is responsible for composing and initializing the application.

It centralizes dependency injection and application startup, providing a single entry point for configuring and initializing the application.

This project acts as the **Composition Root**, keeping host applications (such as the CLI, Web API, or future UI projects) lightweight and focused on their own concerns.

---

## Responsibilities

The Bootstrap layer is responsible for the following tasks:

- Register application services.
- Register infrastructure services.
- Configure the application pipeline.
- Orchestrate application startup.
- Provide a single entry point for application composition.

---

## Composition Root

The Bootstrap project acts as the application's Composition Root by registering every required layer.

It composes the application by:

- Registering the Application layer.
- Registering the Infrastructure layer.
- Configuring the command and query pipelines.

> [!IMPORTANT]
>
> The Bootstrap project is the application's Composition Root.
>
> Presentation projects should configure the application exclusively through the Bootstrap layer instead of registering services directly. This keeps application composition centralized and consistent across different hosts.

---

## Application Initialization

During application startup, Bootstrap coordinates the database initialization process.

The initialization process consists of two steps:

1. Preparing the database environment.
2. Applying any pending Entity Framework Core migrations.

Bootstrap coordinates this process while delegating each responsibility to the appropriate layer. Database preparation is handled by the Infrastructure layer, and schema updates are applied through Entity Framework Core.

By centralizing startup initialization, every host application follows the same initialization workflow without duplicating startup logic.

---

## Related Documentation

- [Project Overview](../../README.md) — Introduction, demonstration, and roadmap.
- [Architecture](../../docs/architecture/README.md) — High-level architecture, layer responsibilities, design decisions, and dependency structure.
- [WhoAmI.Domain](../WhoAmI.Domain/README.md) — Business model, entities, value objects, aggregates, and domain events.
- [WhoAmI.Application](../WhoAmI.Application/README.md) — Application workflows, CQRS, validation, and the Result pattern.
- [WhoAmI.Infrastructure](../WhoAmI.Infrastructure/README.md) — Persistence, repositories, queries, EF Core configuration, and infrastructure services.
- [WhoAmI.CLI](../WhoAmI.CLI/README.md) — Command-line interface and available commands.

---

Built with ❤️ on **Linux** using **JetBrains Rider**.
 
> *“Taste and see that the Lord is good; blessed is the one who takes refuge in him.”*
> 
> — **Psalm 34:8**
