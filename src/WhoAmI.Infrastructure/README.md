# WhoAmI.Infrastructure

[🏠 Home](../../README.md) / **Infrastructure**

---

## Introduction

The **WhoAmI.Infrastructure** project contains all infrastructure-specific implementations used by the application.

It provides concrete implementations for the abstractions defined by the Application layer while keeping technology-specific concerns isolated from the rest of the application.

The project follows the principles of **Clean Architecture**, **Domain-Driven Design (DDD)**, and **SOLID**, allowing infrastructure technologies to evolve independently without affecting the Domain or Application layers.

---

## Responsibilities

The Infrastructure layer is designed to be extensible. New persistence providers, query implementations, or persistence violation mappings can be introduced without affecting the Domain or Application layers.

The Infrastructure layer is responsible for:

- Configuring the database model.
- Implementing repositories.
- Persisting aggregates and executing other write operations against the database.
- Translating provider-specific persistence errors.
- Registering infrastructure services.
- Executing SQL queries.

> [!NOTE]
>
> The Infrastructure layer is **not** responsible for business rules. Business validation belongs to the Domain and Application layers.

---

## Data Access

### Persistence

#### Configurations

Infrastructure uses Entity Framework Core model configurations to define how aggregates are mapped to the database.

Each aggregate has an Entity Framework model configuration where the following are configured:
- Tables.
- Columns.
- Indexes.
- Conversions.
- Owned types.
- Database constraints.

These classes contain only persistence mapping and no business logic.

Database constraint names are centralized in the [DatabaseConstraintNames](./Data/Persistence/Configurations/Constraints/DatabaseConstraintNames.cs) class. This avoids repeating string literals throughout the persistence configuration.

> [!NOTE]
>
> These constants are used only when configuring the database schema for unique indexes or unique constraints.

---

#### Context

The [WhoAmIDbContext](./Data/Persistence/Context/WhoAmIDbContext.cs) class is the central entry point for Entity Framework Core persistence.

Besides managing entity persistence, it also coordinates the persistence error translation pipeline, ensuring provider-specific exceptions are converted into provider-independent application errors before leaving the Infrastructure layer.

Its responsibilities include:

- Exposing `DbSet` properties.
- Applying Entity Framework model configurations.
- Acting as the `IUnitOfWork` implementation.
- Coordinating persistence error translation.

---

#### Repositories

Repositories implement the repository interfaces defined by the Application layer, providing persistence for aggregate roots.

They encapsulate Entity Framework Core operations required to store and retrieve aggregates while hiding persistence-specific implementation details from the rest of the application.

Their responsibilities include:

- Persisting aggregate roots.
- Retrieving aggregate roots.
- Encapsulating Entity Framework Core operations.
- Providing an abstraction over the persistence mechanism.

---

### Queries

The query side is responsible for executing read operations against the database.

Unlike the persistence side, queries do not load or modify aggregates. Instead, they retrieve only the data required by each use case, allowing read operations to remain independent of the write model.

This separation follows the CQRS approach adopted by the project, enabling each side to evolve independently while keeping the Domain and Application layers unaware of persistence technology.

---

#### Connections

Query handlers obtain database connections through the [IDbConnectionFactory](./Data/Queries/Abstractions/IDbConnectionFactory.cs) abstraction.

The current implementation provides a SQLite connection factory, but the abstraction allows the underlying provider to be replaced without affecting query handlers.

This keeps connection creation centralized and prevents provider-specific code from being scattered throughout the query implementation.

---

#### Query Handlers

Each query handler is responsible for executing a single read operation.

Rather than retrieving aggregates, query handlers execute SQL directly against the database and return only the data required by the use case.

Organizing each query in its own directory groups all related artifacts together, improving discoverability and making each read operation self-contained.

A query typically consists of:

- Query handler.
- SQL statement.
- Row model (when required).

For example:

```text
Profiles/
└── GetProfileByEmail/
    ├── GetProfileByEmailQueryHandler.cs
    ├── GetProfileByEmailRow.cs
    └── GetProfileByEmailSql.cs
```

---

## CQRS Infrastructure Overview

The following diagram illustrates how the Infrastructure layer implements the Command (write) and Query (read) sides of the application and how both flows interact with the persistence layer.

<p align="center">
  <img
    src="../../docs/infrastructure/cqrs-infrastructure-overview.svg"
    alt="CQRS Infrastructure Overview"
  />
</p>

> [!NOTE]
>
> Although the diagram illustrates separate read and write databases, the current implementation uses a single SQLite database for both operations.
>
> The separation represents the CQRS architecture rather than the physical database layout. It allows the read and write sides to evolve independently and enables different persistence strategies, read models, or dedicated databases to be introduced in the future without affecting the Application or Domain layers.

---

## Persistence Error Handling

One of the responsibilities of the Infrastructure layer is translating provider-specific persistence errors into provider-independent application errors.

This prevents SQLite, SQL Server, PostgreSQL, or any other database-specific implementation details from leaking into the Application layer.

For example, SQLite may generate an exception such as `UNIQUE constraint failed: Profiles.email`.

---

### PersistenceErrorMapper

Coordinates the persistence error translation process.

It receives provider-specific exceptions, delegates the parsing and violation mapping to the appropriate components, and returns the corresponding application error.

[WhoAmIDbContext](./Data/Persistence/Context/WhoAmIDbContext.cs) then creates a failed [Result](../WhoAmI.Application/Results/Result.cs) containing the returned `Error` and returns it to the Application layer.

The Application layer receives only the resulting application error and never depends on provider-specific exception details.

This process is implemented by [PersistenceErrorMapper](./Data/Persistence/ErrorHandling/PersistenceErrorMapper.cs).

---

### Parsing

Parsers are responsible for interpreting provider-specific persistence exceptions and translating them into provider-independent persistence violation codes.

Each database provider requires its own parser implementation because exception types and messages vary between providers.

For SQLite, the parser inspects the exception message and matches known constraint signatures.

For example, the exception message `UNIQUE constraint failed: Profiles.email` is translated into the persistence violation code `DuplicateProfileEmail`.

This translation isolates provider-specific knowledge within the Infrastructure layer, ensuring that no SQLite-specific details leak into the Application layer.

For SQLite, this responsibility is implemented by [SqliteConstraintViolationParser](./Data/Persistence/ErrorHandling/Parsers/Sqlite/SqliteConstraintViolationParser.cs).

---

### [PersistenceViolationCode](./Data/Persistence/ErrorHandling/PersistenceViolationCode.cs)

`PersistenceViolationCode` represents persistence violations independently of any specific database provider.

Examples include:

- `DuplicateProfileEmail`
- `DuplicateProfileGuid`

These codes provide a stable abstraction between provider-specific persistence exceptions and application errors. They allow the Infrastructure layer to recognize a persistence violation regardless of whether it originated from SQLite, SQL Server, PostgreSQL, or another database provider.

The codes intentionally do not represent:

- Database providers.
- Constraint names.
- SQL messages.
- Exception types.

Instead, they describe the semantic meaning of the persistence violation, allowing the Application layer to remain completely independent of persistence technology.

---

### Constraint Signatures

Constraint signatures define the mapping between provider-specific exception signatures and provider-independent persistence violation codes.

They centralize all database-specific knowledge in a single location, allowing parsers to translate persistence exceptions without scattering provider-specific strings throughout the codebase.

If support for another database provider is introduced, it can define its own parser and corresponding signature registry without affecting the existing implementation.

For SQLite, the signature registry is implemented by [SqliteConstraintSignatures](./Data/Persistence/ErrorHandling/Parsers/Sqlite/SqliteConstraintSignatures.cs).

---

### Violation Mappers

Violation mappers are responsible for translating persistence violation codes into application errors.

Each mapper extracts the domain-relevant metadata required to produce a rich, provider-independent application error.

For example, the mapper for `DuplicateProfileEmail` extracts the `Email` value from the tracked EF Core entity and uses it to construct the corresponding application error.

Each mapper has a single responsibility and handles exactly one persistence violation.

Adding support for a new persistence violation typically requires creating a new mapper without modifying existing ones, respecting the Open-Closed principle of [**SOLID**](http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod).

For example, the mapper for `DuplicateProfileEmail` is implemented by [DuplicateProfileEmailViolationMapper](./Data/Persistence/ErrorHandling/ViolationMappers/DuplicateProfileEmailViolationMapper.cs).

---

### [UnrecognizedConstraintException](./Data/Persistence/ErrorHandling/Exceptions/UnrecognizedConstraintException.cs)

Thrown when the parser receives an unexpected provider exception type or cannot match a provider-specific exception to a registered constraint signature.

Failing fast prevents silent data inconsistencies and makes unsupported database constraint violations immediately visible during development.

---

### Persistence Error Pipeline

The following diagram illustrates the persistence error handling pipeline step by step.

<p align="center">
  <img
    src="../../docs/infrastructure/persistence-error-pipeline.svg"
    alt="Infrastructure - Persistence Error Pipeline"
  />
</p>

---

## Dependency Injection

Infrastructure exposes a single entry point for registering all of its services through [ServiceCollectionExtensions](./DependencyInjection/ServiceCollectionExtensions.cs).

This includes the registration of:

- The Entity Framework Core `DbContext`.
- Repository implementations.
- Persistence error handling components.
- Query handlers.
- Database connection factories.

Applications only need to invoke a single extension method to configure the entire Infrastructure layer.

---

## Migrations

The `Migrations` folder contains the Entity Framework Core migrations that manage the evolution of the database schema.

Each migration represents a versioned set of schema changes, allowing the database structure to evolve alongside the persistence model while preserving existing data.

This keeps the database schema synchronized with the persistence model as the application evolves.

---

## Related Documentation

- [Repository Guide](../../README.md) — Repository overview and getting started.
- [Architecture](../../docs/architecture/README.md) — High-level architecture, layer responsibilities, design decisions, and dependency structure.
- [WhoAmI.Domain](../WhoAmI.Domain/README.md) — Business model, entities, value objects, aggregates, and domain events.
- [WhoAmI.Application](../WhoAmI.Application/README.md) — Application workflows, CQRS, validation, and the Result pattern.
- [WhoAmI.Bootstrap](../WhoAmI.Bootstrap/README.md) — Dependency injection and application composition.
- [WhoAmI.CLI](../WhoAmI.CLI/README.md) — Command-line interface and available commands.

---

## References

- [Domain-Driven Design](https://www.domainlanguage.com/) — Eric Evans
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) — Robert C. Martin
- [SOLID Principles](http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod) — Robert C. Martin

---

Built with ❤️ on **Linux** using **JetBrains Rider**.

> *“Cast all your anxiety on him because he cares for you.”*
>
> — **1 Peter 5:7**
