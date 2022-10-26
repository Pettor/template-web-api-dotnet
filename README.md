[![Actions Main](https://github.com/Pettor/template-web-api-dotnet/actions/workflows/main.yml/badge.svg)](https://github.com/Pettor/template-web-api-dotnet/actions/workflows/main.yml)

## Web API using .NET

### Requirements

- Install PostgreSQL 15.
- Latest Docker

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
