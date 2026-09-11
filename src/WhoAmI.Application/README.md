# WhoAmI.Application

[🏠 Home](../../README.md) / **Application**

---

## Introduction

The **WhoAmI.Application** project implements the application layer following the principles of [**Domain-Driven Design (DDD)**](https://www.domainlanguage.com/), [**Clean Architecture**](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html), [**SOLID**](http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod), and [**Command Query Responsibility Segregation (CQRS)**](https://martinfowler.com/bliki/CQRS.html).

The Application layer depends only on the Domain layer and a minimal set of framework libraries required for dependency injection. It has no dependency on Infrastructure, databases, external services, or presentation frameworks.

All use cases are organized by aggregate under the Features directory.

The Application layer is persistence-agnostic. It defines abstractions required by the use cases, while all database access is implemented by the Infrastructure layer.

---

## Responsibilities

The Application layer is responsible for the following tasks:

- Orchestrating use cases.
- Dispatching commands and queries.
- Validating requests.
- Managing transactions through the [**Unit of Work**](https://martinfowler.com/eaaCatalog/unitOfWork.html).
- Producing application results.
- Publishing execution information for observability.

The Application layer coordinates requests, delegates business behavior to the Domain, and collaborates with Infrastructure through abstractions.

> [!NOTE]
>
> The Application project references only `WhoAmI.Domain`, `Microsoft.Extensions.DependencyInjection`, and `Scrutor`.
> It intentionally contains no persistence logic, presentation logic, or infrastructure concerns.

---

## Features

### Commands

A command feature typically consists of:

- Command.
- Validator.
- Handler.
- Response (optional).

Example:
```
WhoAmI.Application
 └── Features
     └── Profiles
         ├── CreateProfile
         │   ├── CreateProfileCommand.cs
         │   ├── CreateProfileCommandHandler.cs
         │   ├── CreateProfileCommandValidator.cs
         │   └── CreateProfileResponse.cs
         └── UpdateEmail
             ├── UpdateEmailCommand.cs
             ├── UpdateEmailCommandHandler.cs
             └── UpdateEmailCommandValidator.cs
```

### Queries

A query feature typically consists of:

- Query.
- Validator (optional).
- Response.

Example:
```
WhoAmI.Application
 └── Features
     └── Profiles
         ├── GetProfileByEmail
         │   ├── GetProfileByEmailQuery.cs
         │   ├── GetProfileByEmailQueryValidator.cs
         │   └── GetProfileByEmailResponse.cs
         └── ListProfiles
             ├── ListProfilesQuery.cs
             └── ListProfilesResponse.cs
```

> [!IMPORTANT]
> 
> Query handlers are intentionally implemented in the Infrastructure layer because queries operate directly on the read model.
>
> This design keeps the Application layer independent of persistence technologies while allowing the read and write sides to evolve independently.

---

## Dispatchers

Commands and queries are executed through dedicated dispatchers:

- [ICommandDispatcher](./Abstractions/Dispatching/Commands/ICommandDispatcher.cs).
- [IQueryDispatcher](./Abstractions/Dispatching/Queries/IQueryDispatcher.cs).

The dispatchers resolve the appropriate handlers through dependency injection and apply the configured decorator pipeline before invoking the underlying handler.

---

## Command Pipeline

Commands represent operations that modify the application state.

Commands are executed through the following pipeline:

<p align="center">
  <img
    src="../../docs/application/application-command-pipeline.svg"
    alt="Application Command Pipeline"
  />
</p>

Commands are processed through a decorator pipeline before reaching their handlers. Each decorator has a single responsibility and addresses a specific cross-cutting concern, such as logging, validation, or transaction management.

The command pipeline is composed of the following decorators:

- [CommandExecutionDecorator&lt;TCommand&gt;](./Decorators/Commands/CommandExecutionDecorator.cs) and [CommandExecutionDecorator&lt;TCommand, TResult&gt;](./Decorators/Commands/CommandExecutionDecoratorOfT.cs), record execution information for logging and observability.
- [CommandValidationDecorator&lt;TCommand&gt;](./Decorators/Commands/CommandValidationDecorator.cs) and [CommandValidationDecorator&lt;TCommand, TResult&gt;](./Decorators/Commands/CommandValidationDecoratorOfT.cs), validate incoming commands before execution.
- [CommandUnitOfWorkDecorator&lt;TCommand&gt;](./Decorators/Commands/CommandUnitOfWorkDecorator.cs) and [CommandUnitOfWorkDecorator&lt;TCommand, TResult&gt;](./Decorators/Commands/CommandUnitOfWorkDecoratorOfT.cs), execute the handler inside a Unit of Work and commit the transaction when the operation succeeds.

Each decorator is available in two versions: one for commands that do not return a value and one for commands that return a result.

## Query Pipeline

Queries represent operations that retrieve information without modifying application state.

Queries are executed through the following pipeline:

<p align="center">
  <img
    src="../../docs/application/application-query-pipeline.svg"
    alt="Application Query Pipeline"
  />
</p>

Queries are processed through a decorator pipeline before reaching their handlers. Each decorator has a single responsibility and addresses a specific cross-cutting concern, such as logging or validation.

The query pipeline is composed of the following decorators:

- [QueryExecutionDecorator&lt;TQuery, TResult&gt;](./Decorators/Queries/QueryExecutionDecorator.cs).
- [QueryValidationDecorator&lt;TQuery, TResult&gt;](./Decorators/Queries/QueryValidationDecorator.cs).

Unlike the command pipeline, the query pipeline does not include transaction management because queries never modify application state.

---

## Validation

Commands and queries may define validators responsible for verifying application-level rules before a handler is executed.

Validation is performed by the corresponding validation decorators, ensuring that invalid requests never reach the underlying handlers.

When validation fails, the corresponding application result is returned without executing the underlying handler.

Application validation ensures that requests are well-formed and complete.

> [!IMPORTANT]
>
> Business invariants always remain the responsibility of the Domain layer.
> Application validators verify request correctness but never replace domain validation.

---

## Result Pattern

Application operations communicate expected business failures through the Result pattern instead of using exceptions for control flow.

Operations return either [Result](./Results/Result.cs) or [Result&lt;T&gt;](./Results/ResultOfT.cs). On failure, the result contains one or more error instances.

Each [Error](./Results/Error.cs) contains:

- Code.
- Message.
- Metadata (optional).

The optional `Metadata` dictionary allows additional context to be attached to an error without changing its structure.

This approach:
- Avoids exception-based control flow.
- Provides consistent error handling across different presentation layers.
- Allows presentation layers to map application results into user-facing responses.
- Preserves structured information for logging and diagnostics.

---

## Observability

The Application layer publishes execution information through abstractions without depending on any specific logging or monitoring framework.

### Execution Information

Execution information includes:

- Operation name.
- Execution time.
- Serialized request.
- Operation result.
- Business errors.
- Unhandled exceptions.

The execution information is represented by [ExecutionInfo](./Logging/ExecutionInfo.cs) and is produced through [ExecutionInfoFactory](./Logging/ExecutionInfoFactory.cs).

Presentation or Infrastructure projects may consume this information using logging providers, monitoring systems, or other observability tools without introducing dependencies into the Application layer.

### Safe Logging

The [SafeExecutionLogger](./Logging/SafeExecutionLogger.cs) ensures that failures occurring while publishing execution information never affect the execution of the application itself.

Exceptions thrown by the logging implementation are ignored after being written to the debug output.

---

## Related Documentation

- [Repository Guide](../../README.md) — Repository overview and getting started.
- [Architecture](../../docs/architecture/README.md) — High-level architecture, layer responsibilities, design decisions, and dependency structure.
- [WhoAmI.Domain](../WhoAmI.Domain/README.md) — Business model, entities, value objects, aggregates, and domain events.
- [WhoAmI.Infrastructure](../WhoAmI.Infrastructure/README.md) — Persistence, repositories, queries, EF Core configuration, and infrastructure services.
- [WhoAmI.Bootstrap](../WhoAmI.Bootstrap/README.md) — Dependency injection and application composition.
- [WhoAmI.CLI](../WhoAmI.CLI/README.md) — Command-line interface and available commands.

---

## References

- [Domain-Driven Design](https://www.domainlanguage.com/) — Eric Evans
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) — Robert C. Martin
- [SOLID Principles](http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod) — Robert C. Martin
- [CQRS](https://martinfowler.com/bliki/CQRS.html) — Martin Fowler
- [Unit of Work](https://martinfowler.com/eaaCatalog/unitOfWork.html) — Martin Fowler

---

Built with ❤️ on **Linux** using **JetBrains Rider**.

> *“I have told you these things, so that in me you may have peace. In this world you will have trouble. But take heart! I have overcome the world.”*
>
> — **John 16:33**
