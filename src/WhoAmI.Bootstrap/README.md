# WhoAmI.Bootstrap

The **WhoAmI.Bootstrap** project is responsible for composing the application.

It centralizes dependency injection, infrastructure registration, and application startup logic, providing a single entry point for configuring the system.

This project acts as the **Composition Root**, keeping host applications (such as the CLI, Web API, or future UI projects) lightweight and focused on their own concerns.

## Responsibilities

- Register application services.
- Register infrastructure services.
- Configure the application pipeline.
- Initialize the database during application startup.
- Provide a single entry point for application composition.

## Composition Root

The `AddBootstrap` extension method configures the application's service collection by registering every required layer.

```csharp
services.AddBootstrap(configuration, environment);
```

Internally, it composes the application by:

- Registering the Application layer.
- Registering the Infrastructure layer.
- Configuring the command and query pipelines.

## Database Initialization

The `InitializeDatabaseAsync` extension method applies any pending Entity Framework Core migrations during application startup.

```csharp
await serviceProvider.InitializeDatabaseAsync();
```

This ensures that the database schema is kept up to date before the application begins processing requests.

## Dependencies

- **WhoAmI.Application**
- **WhoAmI.Infrastructure**
- **Microsoft.Extensions.DependencyInjection**
- **Microsoft.Extensions.Hosting**
- **Microsoft.EntityFrameworkCore**

## Related Documentation

- [WhoAmI — Project Overview](../../README.md)
- [Architecture](../../docs/architecture/README.md)
- [WhoAmI.Application](../WhoAmI.Application/README.md)
- [WhoAmI.CLI](../WhoAmI.CLI/README.md)
- [WhoAmI.Domain](../WhoAmI.Domain/README.md)
- [WhoAmI.Infrastructure](../WhoAmI.Infrastructure/README.md)

---

Built with ❤️ on **Linux** using **JetBrains Rider**.
 
> “Taste and see that the Lord is good; blessed is the one who takes refuge in him.”
> 
> — Psalm 34:8
