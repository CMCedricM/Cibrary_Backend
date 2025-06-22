# Cibrary_Backend

## Summary

This is the backend service for the Cibrary App. It provides RESTFUL APi's protected by Auth0 to manage users, a book catalog, check-in and check-out books, and also comes bundled in with a PostgreSQL database, which can be run with docker-compose. This requires .NET 9 with an Auth0 api setup, along with PostgreSQL (All bundled in the docker file).

:memo: This backend is to be used in conjuction with the Cibrary_Frontend Project, which is to be launched on a different web host.

## Features

- :lock: Auth0 Protected endpoints
- Add, updated, delete, and search books
- Search with ISBN and with Title
- Quantity information

## To-Do

- Publish Postman Collection
- Add Better Setup Instructions

## Tech Stack

- **Backend Framework**: .Net 9 (ASP.NET Core Web API)
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Containerization**: Docker
- **Frontend Framework**: Next.js, Vercel

## Setup

### Required Variables

```bash
DATABASE_URL=
DATABASE_URL_DOTNET=HOST=db;Username=<YourUserName>;Password=<YourPassword>;Database=<YourDatabaseName>>
POSTGRES_PASSWORD=<YourPostgreSQLPassword>

AUTH0_DOMAIN=
AUTH0_JWT_AUDIENCE=
AUTH0_CLIENT_ID=
AUTH0_CLIENT_SECRET=
AUTH0_MANAGMENT_AUDIENCE=
```

### Instructions

1. Clone the repository
2. Setup an Auth0 API and App
3. Provide necessary Variables
4. Navigate to the root of the project
5. Run `dotnet restore`
6. Run `docker-compose up`
