# Repository Guidelines

## Project Structure & Module Organization

All application code lives under `src/`; the solution is `src/eshop-microservices.slnx`.

- `Catalog.API` and `Basket.API` organize Carter endpoints and MediatR handlers by feature, such as `Products/CreateProduct`.
- `Discount.Grpc` contains the discount service, protobuf contracts in `Protos/`, and SQLite persistence.
- `Ordering.API`, `Ordering.Application`, `Ordering.Domain`, and `Ordering.Infrastructure` separate endpoints, use cases, domain models, and persistence. Keep domain logic independent of infrastructure.
- `BuildingBlocks` provides shared CQRS, validation, and exception handling; `BuildingBlocks.Messaging` contains MassTransit integration and events.
- `YarpApiGateway` routes requests to services. Docker Compose files live in `src/`. There are currently no dedicated test or frontend asset directories.

## Build, Test, and Development Commands

Use the .NET 10 SDK and Docker with Linux containers. Run these commands from the repository root:

- `dotnet build src/Catalog.API/Catalog.API.csproj`: restore and build Catalog and its dependencies.
- `dotnet run --project src/Catalog.API --launch-profile http`: run Catalog at `http://localhost:5000`, with its database available.
- `docker compose -f src/docker-compose.yml -f src/docker-compose.override.yml up --build -d`: build and start the stack.
- `docker compose -f src/docker-compose.yml -f src/docker-compose.override.yml logs -f`: inspect stack logs.

The solution includes a Visual Studio Docker Compose project; standalone project builds avoid requiring that tooling. Compose HTTPS settings require local development certificates and use Windows `${APPDATA}` mounts.

## Coding Style & Naming Conventions

Follow existing C# style: four-space indentation, file-scoped namespaces, PascalCase types and methods, and camelCase parameters and locals. Prefix interfaces with `I`. Use descriptive `Command`, `Query`, `Handler`, and `Endpoint` names. Preserve nullable reference types and implicit usings. No repository formatter or `.editorconfig` is configured.

## Testing Guidelines

No test framework or coverage threshold is configured. For new tests, prefer service-specific projects under `tests/`, named `<Service>.Tests`, and methods named `Method_Scenario_ExpectedResult`. Run them with `dotnet test <test-project.csproj>`. Validate affected endpoints, validation failures, and persistence or messaging flows when relevant; record manual checks until automated coverage exists.

## Commit & Pull Request Guidelines

History uses short action-oriented subjects, such as `Configure API endpoints for Orders`; no strict prefix convention is evident. Keep commits focused. PRs should describe behavior changes, affected services, verification, and configuration or migration requirements, and link related issues when available.

## Security & Configuration

Use environment variables or .NET user secrets for sensitive configuration. Treat committed Compose credentials as local development defaults. Review EF Core migrations with model changes, and account for Development startup database initialization when testing.
