# EduTrack API

EduTrack API is an ASP.NET Core Web API for managing students, courses, and enrollments.

This project was created as a learning project to practice backend development with a simple academic domain. The goal was to apply real-world concepts commonly used in .NET APIs, such as Repository Pattern, SQL migrations with DbUp, data access with Dapper, request validation with FluentValidation, pagination, centralized error handling, health checks, integration testing with Testcontainers, observability with Application Insights, and deployment to Azure App Service with Azure SQL Database.

## Tech Stack

- .NET 10 / ASP.NET Core Web API
- SQL Server / Azure SQL Database
- Dapper
- DbUp
- FluentValidation
- xUnit + Testcontainers
- Azure App Service
- Application Insights
- GitHub Actions

## Features

- Student CRUD
- Course creation and listing
- Student enrollment in courses
- Duplicate enrollment prevention
- Paginated student listing
- Request validation
- Centralized error handling
- Health check endpoint
- Database migrations at startup with DbUp
- Swagger/OpenAPI documentation

## API Overview

### Students

| Method | Endpoint | Description |
| --- | --- | --- |
| `POST` | `/api/students` | Create a student |
| `GET` | `/api/students` | List students with pagination |
| `GET` | `/api/students/{id}` | Get a student with enrolled courses |
| `PUT` | `/api/students/{id}` | Update a student |
| `DELETE` | `/api/students/{id}` | Delete a student |

### Courses

| Method | Endpoint | Description |
| --- | --- | --- |
| `POST` | `/api/courses` | Create a course |
| `GET` | `/api/courses` | List courses |
| `GET` | `/api/courses/{id}` | Get a course by ID |

### Enrollments

| Method | Endpoint | Description |
| --- | --- | --- |
| `POST` | `/api/enrollments` | Enroll a student in a course |

## Database

The API uses SQL Server as the relational database.

Database scripts are stored in:

```text
src/EduTrack.Api/Scripts/
```

DbUp runs the scripts during application startup. The current schema includes:

- `Student`
- `Course`
- `Enrollment`

The `Enrollment` table uses a composite key to prevent the same student from being enrolled in the same course more than once.

## Running Locally

### Testing with Postman

To make testing easier, a Postman collection is included in this repository. It contains all the pre-configured endpoints for Students, Courses, and Enrollments.

1. Select the file located at `docs/EduTrack_Postman_Collection.json`.
2. Make sure your local API is running, and you are ready to send requests!

### Prerequisites

- .NET 10 SDK
- SQL Server or Azure SQL Database
- Docker, required for integration tests

### Configure the Connection String

The API expects a connection string named `DefaultConnection`.

Using user secrets:

```powershell
cd src/EduTrack.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=EduTrack;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
```

### Run the API

```powershell
cd src
dotnet run --project EduTrack.Api
```

Swagger is available at:

```text
https://localhost:7042/swagger
```

Health check:

```text
https://localhost:7042/health
```

## Running Tests

```powershell
cd src
dotnet test
```

The integration tests use Testcontainers to start a SQL Server container and run repository tests against a real database.

## CI/CD and Azure

This repository includes GitHub Actions workflows for building, testing, publishing, and deploying the API to Azure App Service.

The application is designed to run with Azure SQL Database as the relational database. Application Insights can be enabled through Azure configuration to collect telemetry and help monitor the application.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE).
