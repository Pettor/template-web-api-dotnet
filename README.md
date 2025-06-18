[![Actions Main](https://github.com/Pettor/template-web-api-dotnet/actions/workflows/main.yml/badge.svg)](https://github.com/Pettor/template-web-api-dotnet/actions/workflows/main.yml)

# Clean Architecture .NET Web API Template

A production-ready, enterprise-grade .NET 9.0 Web API template built with Clean Architecture principles and comprehensive multitenancy support. This template provides a solid foundation for building scalable, maintainable web APIs with modern development practices and industry-standard patterns.

## ✨ Key Features

**🏗️ Architecture & Design**
- Clean Architecture implementation with clear separation of concerns
- Domain-Driven Design (DDD) patterns
- CQRS with MediatR for command/query separation
- Repository pattern with specifications
- Dependency injection throughout all layers

**🏢 Multi-tenancy Support**
- Built-in multitenancy using Finbuckle.MultiTenant
- Tenant isolation at database and application levels
- Flexible tenant resolution strategies

**🔐 Authentication & Authorization**
- JWT Bearer authentication
- Azure AD integration support
- Role-based and permission-based authorization
- Identity management with ASP.NET Core Identity

**📊 Database & Persistence**
- Entity Framework Core with PostgreSQL support
- Code-first migrations with dedicated migrator projects
- Database seeding and initialization
- Audit trails and soft delete functionality

**🔧 Background Processing**
- Hangfire integration for background jobs
- Job scheduling and monitoring dashboard
- Persistent job storage with PostgreSQL

**📚 API Documentation**
- Swagger/OpenAPI documentation with NSwag
- Comprehensive API versioning
- Interactive API explorer

**🧪 Testing & Quality**
- Unit test infrastructure setup
- Code style enforcement with StyleCop
- EditorConfig for consistent formatting
- GitHub Actions CI/CD pipeline

**⚡ Performance & Scalability**
- Redis caching support
- SignalR for real-time communication
- Health checks implementation
- Docker containerization ready

**📧 Additional Features**
- Email services with MailKit
- Excel export functionality with ClosedXML
- Comprehensive logging with Serilog
- Localization support

## 🚀 Quick Start

### Prerequisites
- .NET 9.0 SDK
- PostgreSQL 15+
- Docker (optional)

### Setup
1. Clone the repository
2. Copy configuration templates from `Host.Configurations.Templates` to `Host.Configurations`
3. Update database connection strings in `database.json` and `hangfire.json`
4. Run migrations: `dotnet ef database update`
5. Start the application: `dotnet run` or use Docker: `docker compose up`

### Default Credentials
```json
{
    "email": "admin@root.com",
    "password": "123Pa$$word!"
}
```

## 📁 Project Structure

```
├── src/
│   ├── Core/
│   │   ├── Application/          # Application layer (CQRS, DTOs, Services)
│   │   ├── Domain/              # Domain entities and business logic
│   │   └── Shared/              # Shared contracts and utilities
│   ├── Infrastructure/          # Infrastructure concerns (EF, Auth, etc.)
│   ├── Host/                   # Web API host and controllers
│   └── Migrators/              # Database migration projects
├── tests/                      # Test projects
└── docker-compose.yml          # Docker configuration
```

## 🛠️ Technology Stack

- **.NET 8.0** - Latest LTS framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM and data access
- **PostgreSQL** - Primary database
- **Redis** - Caching and session storage
- **Hangfire** - Background job processing
- **MediatR** - CQRS implementation
- **Mapster** - Object mapping
- **FluentValidation** - Input validation
- **Serilog** - Structured logging
- **JWT** - Authentication tokens
- **Swagger/NSwag** - API documentation

## 📋 API Endpoints

The template includes sample CRUD operations for:
- **Identity Management** - Users, roles, and permissions
- **Product Catalog** - Products and brands with full CRUD
- **Tenant Management** - Multi-tenant operations

## 🐳 Docker Support

Full Docker support with:
- Multi-stage Dockerfile for optimized builds
- Docker Compose with PostgreSQL and Redis services
- Environment-based configuration
- Health checks and logging

## 🔄 CI/CD

GitHub Actions workflow included for:
- Automated testing
- Code quality checks
- Docker image building
- Deployment automation

## 📄 License

This project is licensed under the [MIT License](LICENSE) - see the LICENSE file for details.

## 🤝 Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

---
