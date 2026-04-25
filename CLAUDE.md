# CLAUDE.md — AI Assistant Guide

This document provides guidance for AI assistants (Claude and others) working in this repository. It covers codebase structure, development workflows, and conventions to follow.

## Project Overview

This is a **production-ready, enterprise-grade .NET 10.0 Web API template** implementing Clean Architecture with multitenancy support. It is designed as a reusable starting point for scalable, maintainable web API services.

**Repository:** `pettor/template-web-api-dotnet`

---

## Architecture

The project follows **Clean Architecture** with four clearly separated layers:

```
src/
├── Core/
│   ├── Domain/           # Entities, value objects, domain events — no external deps
│   ├── Application/      # CQRS handlers, DTOs, validators, use cases
│   └── Shared/           # Shared contracts and interfaces
├── Infrastructure/       # EF Core, Auth, Caching, Mailing, Hangfire, etc.
├── Host/                 # ASP.NET Core entry point: controllers, middleware, config
└── Migrators/
    └── Migrators.PostgreSQL/  # EF Core migrations for PostgreSQL
tests/
└── Infrastructure.Test/  # xUnit unit tests
```

### Layer Dependency Rules

- **Domain** has zero external dependencies
- **Application** depends only on Domain and Shared
- **Infrastructure** depends on Application and Domain
- **Host** depends on Infrastructure (composition root)

Never add references that violate this direction (e.g., Domain referencing Application).

---

## Tech Stack

| Category | Technology |
|---|---|
| Framework | .NET 10.0, ASP.NET Core, C# 14 |
| ORM | Entity Framework Core 10 + Npgsql |
| Database | PostgreSQL 15+ |
| CQRS | MediatR 11 |
| Validation | FluentValidation 12 |
| Mapping | Mapster 10 |
| Auth | ASP.NET Core Identity, JWT Bearer, Azure AD (Microsoft.Identity.Web) |
| Multitenancy | Finbuckle.MultiTenant 10 |
| Background Jobs | Hangfire 1.8 + Hangfire.PostgreSql |
| Caching | Redis (StackExchangeRedis) |
| Real-time | SignalR |
| API Docs | NSwag 14 (OpenAPI/Swagger) |
| Logging | Serilog (Extensions 10) |
| Testing | xUnit v3 3.x, FluentAssertions 8, coverlet |
| Code Style | StyleCop.Analyzers, Roslynator, csharpier |
| Git Hooks | Husky.Net |

---

## Key Conventions

### Naming

- **Namespaces:** `Backend.{Layer}.{Feature}` (e.g., `Backend.Application.Identity.Tokens`)
- **Classes/Methods:** PascalCase
- **Private fields:** `_camelCase`
- **Global type alias:** `DefaultIdType` = `System.Guid` (defined in `Directory.Build.props`)

### CQRS Pattern

Every feature follows the same structure:

```
Application/{Feature}/
├── {Feature}Request.cs          # IRequest<TResponse>
├── {Feature}RequestHandler.cs   # IRequestHandler<TRequest, TResponse>
├── {Feature}RequestValidator.cs # FluentValidation AbstractValidator<TRequest>
└── Dtos/
    └── {Feature}Dto.cs
```

Dispatch via `ISender` (MediatR) injected in controllers through `BaseApiController`.

### Entity Design

Domain entities extend `AuditableEntity<TId>` and implement `IAggregateRoot`:

```csharp
public class Product : AuditableEntity<DefaultIdType>, IAggregateRoot
```

- Audit fields auto-populated: `CreatedBy`, `CreatedOn`, `LastModifiedBy`, `LastModifiedOn`
- Soft delete fields: `DeletedOn`, `DeletedBy` (via `ISoftDelete`)
- Use fluent update methods: `product.Update(...).ClearImagePath()`

### Controllers

All API controllers inherit `BaseApiController` (which provides `ISender`) and use `VersionNeutralApiController` for route/versioning attributes. Decorate with permission attributes:

```csharp
[MustHavePermission(ApiAction.View, ApiResource.Products)]
```

### Configuration Files

Runtime configuration lives in `src/Host/Configurations/`. Sensitive config files (`database.json`, `hangfire.json`) are gitignored. Use the `Templates/` subdirectory as a reference for required structure.

---

## Development Workflows

### Prerequisites

- .NET 10.0 SDK
- PostgreSQL 15+ (or Docker)
- Redis (or Docker)
- PowerShell (for migration script)

### Initial Setup

```bash
# 1. Copy configuration templates
cp src/Host/Configurations/Templates/database.json src/Host/Configurations/database.json
cp src/Host/Configurations/Templates/hangfire.json src/Host/Configurations/hangfire.json

# 2. Update connection strings in both files

# 3. Apply migrations
dotnet ef database update

# 4. Run the API
dotnet run --project src/Host
```

### Running with Docker

```bash
docker compose up
```

API available at `https://localhost:5050` (HTTPS) and `http://localhost:5060` (HTTP). See `README_DOCKER.md` for HTTPS certificate setup.

**Default credentials:**
- Email: `admin@root.com`
- Password: `123Pa$$word!`

### Build & Test

```bash
dotnet restore
dotnet build
dotnet test
```

### Lint / Format

csharpier is enforced via pre-commit hook (Husky.Net) and CI:

```bash
# Check formatting
dotnet csharpier check .

# Auto-format
dotnet csharpier format .
```

The pre-commit hook runs csharpier automatically on staged `.cs` files. Install hooks after cloning:

```bash
dotnet husky install
```

### Database Migrations

Use the PowerShell script to generate migrations (it temporarily swaps config, runs `dotnet ef`, then restores):

```powershell
# From repo root
.\scripts\add-update-db-migrations.ps1 -commitMessage "AddProductTable"
```

Migrations are placed in `src/Migrators/Migrators.PostgreSQL/Migrations/Application/`.

Manual alternative:

```bash
cd src/Host
dotnet ef migrations add <MigrationName> \
  --project ../Migrators/Migrators.PostgreSQL/ \
  --context ApplicationDbContext \
  -o Migrations/Application
```

---

## CI/CD

GitHub Actions workflow (`.github/workflows/main.yml`) runs on push/PR to `main`:

1. `dotnet restore`
2. `dotnet csharpier check .` (lint)
3. `dotnet build --no-restore`
4. `dotnet test --no-build --verbosity normal`

All four steps must pass. Formatting errors block the build.

---

## Testing

Tests live in `tests/Infrastructure.Test/` using xUnit with dependency injection support.

```bash
dotnet test
dotnet test --collect:"XPlat Code Coverage"  # with coverage
```

Test configuration is in `tests/Infrastructure.Test/appsettings.json`.

Current test coverage focuses on:
- `Multitenancy/` — Connection string securing
- `Caching/` — Distributed and local cache services

When adding features, add corresponding tests in the relevant subdirectory following the existing fixture pattern.

---

## Code Quality Rules

Enforced globally via `Directory.Build.props`:

- **Target framework:** `net10.0`
- **Language version:** C# 14
- **Nullable reference types:** enabled
- **Implicit usings:** enabled
- **Documentation file:** generated (XML doc comments encouraged on public APIs)
- **Analyzers:** StyleCop.Analyzers + Roslynator.Analyzers on every project

Key StyleCop rules (see `stylecop.json` and `dotnet.ruleset`):
- `using` directives go outside namespace declarations
- System namespaces ordered first
- No trailing newline at EOF

---

## Project-Level MSBuild Notes

`Directory.Build.props` applies to all projects automatically. Do not repeat these settings in individual `.csproj` files:
- `TargetFramework`
- `LangVersion`
- `Nullable`
- `ImplicitUsings`
- Analyzer package references

The global alias `DefaultIdType = System.Guid` is available everywhere without an import.

---

## Dependency Updates

Renovate (`renovate.json`) manages dependency updates automatically:
- Runs every weekend
- Auto-merges NuGet package updates
- Uses semantic commit messages

Do not manually pin package versions unless there is a specific regression to avoid.

---

## What Not To Do

- Do not add business logic to the **Host** layer — it belongs in **Application** or **Domain**
- Do not bypass the CQRS pattern by calling repositories directly from controllers
- Do not add direct `DbContext` usage in **Application** — use the repository/specification pattern
- Do not commit `database.json` or `hangfire.json` — they contain credentials
- Do not skip csharpier formatting — CI will reject unformatted code
- Do not add `TargetFramework` or analyzer references to individual `.csproj` files (inherited from `Directory.Build.props`)
