[![Actions Main](https://github.com/Pettor/template-web-api-dotnet/actions/workflows/main.yml/badge.svg)](https://github.com/Pettor/template-web-api-dotnet/actions/workflows/main.yml)

## Web API using .NET

### Requirements

- .NET 9.0
- Install PostgreSQL 15.
- Latest Docker

### Configuration

Copy the template files for `database.json` and `hangfire.json` in `Host.Configurations.Templates` directory to the `Host.Configurations` directory.

Configure the `ConnectionString` in both files to make sure they match the credentials for the local PostgreSQL installation.

### Run using Docker

To run the project using Docker run the following in the root dir:

```powershell
docker compose up
```

### Swagger

Running the backend from Solution will open up Swagger on:

```powershell
http://localhost:5000/swagger
```

Running the backend from Docker will open up Swagger on:

```powershell
https://localhost:5050/swagger
```

Authenticate using the following credentials:

```json
{
    "email":"admin@root.com",
    "password":"123Pa$$word!"
}
```

## License

This project is licensed with the [MIT license](LICENSE).
