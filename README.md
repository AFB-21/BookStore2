# BookStore API

A simple BookStore REST API built with .NET 8 using Clean/CQRS patterns. Main technologies:

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- ASP.NET Core Identity (JWT authentication)
- MediatR (CQRS)
- AutoMapper
- FluentValidation
- Swashbuckle/Swagger for API docs

This README describes the project, configuration, and documents every HTTP endpoint exposed by the API.

## Projects

- `BookStore.Api` - Web API project (controllers, middleware, swagger). Entry point.
- `BookStore.Application` - Application layer (DTOs, MediatR requests/handlers, validators, mapping profiles).
- `BookStore.Infrastructure` - Infrastructure (EF Core `AppDbContext`, Identity, repository implementations, migrations).
- `BookStore.Domain` - Domain entities (e.g., `Book`, `Author`, `Category`).
- `BookStore.Service` - (if present) additional services/utilities.

## Configuration

Important settings (usually in `appsettings.json` / environment variables):

- Connection string key: `ConnectionStrings:SecondaryConnection` — SQL Server connection for `AppDbContext`.
- JWT settings under section `Jwt` (used by authentication middleware):
  - `Jwt:Key` (symmetric signing key; must be at least 32 chars)
  - `Jwt:Issuer`
  - `Jwt:Audience`

Swagger is enabled only in development environment by default. The swagger JSON is available at `/swagger/v1/swagger.json` and the UI at `/swagger` (when `ASPNETCORE_ENVIRONMENT=Development`).

Program.cs applies EF Core migrations at startup and seeds roles/users via `IdentityDataSeeder`.

## Running locally

1. Ensure SQL Server is reachable and `ConnectionStrings:SecondaryConnection` is set.
2. Configure `Jwt` settings in `appsettings.Development.json` or environment variables.
3. From solution root run (or open in IDE):

```bash
cd BookStore.Api
dotnet run
```

4. Open Swagger UI (development only): `https://localhost:{port}/swagger`

## Authentication

- Authentication uses JWT bearer tokens.
- To get a token, call `POST /api/auth/login` with `LoginDto` payload. The returned token should be used in `Authorization: Bearer <token>` header for protected endpoints. Note: many controller actions are currently public in the codebase; some endpoints are prepared for role-based policies.


---

# API Endpoints (detailed)

All endpoints are rooted at `/api`.

## Auth

### POST `/api/auth/login`
- Description: Authenticate user and return JWT.
- Request body: `LoginDto`
  - `Email` (string)
  - `Password` (string)
- Responses:
  - `200 OK` — `{ token: string }` (or richer `LoginResponse` in infrastructure)
  - `401 Unauthorized` — invalid credentials

Example request body:
```json
{ "email": "user@example.com", "password": "P@ssw0rd" }
```


## Books

Base route: `/api/books`

### POST `/api/books/create`
- Description: Create a new book
- Request body: `CreateBookDTO`
  - `Title` (string)
  - `Description` (string | optional)
  - `PublishedOn` (DateTime)
  - `Price` (decimal)
  - `AuthorId` (Guid)
  - `CategoryId` (Guid)
- Responses:
  - `201 Created` — returns created `BookDTO` with `Id` and other fields

### GET `/api/books/{id}`
- Description: Get a book by id
- Responses:
  - `200 OK` — `BookDTO`
  - `404 Not Found` — no book found

### GET `/api/books`
- Description: Get all books
- Responses:
  - `200 OK` — `List<BookDTO>`

### GET `/api/books/paginated?page={page}&pageSize={pageSize}`
- Description: Get paginated list of books
- Query parameters: `page` (int, default 1), `pageSize` (int, default 10)
- Responses:
  - `200 OK` — `List<BookDTO>` (page slice)

### PUT `/api/books/{id}`
- Description: Update a book
- Request body: `UpdateBookDTO` (same fields as `CreateBookDTO` except the Id)
- Responses:
  - `200 OK` — updated `BookDTO`

### DELETE `/api/books/{id}`
- Description: Delete a book
- Responses:
  - `200 OK` — `{ message: "BookDeleted" }`


DTO: `BookDTO` fields (returned)
- `Id` (Guid)
- `Title` (string)
- `Description` (string?)
- `PublishedOn` (DateTime)
- `Price` (decimal)
- `AuthorId` (Guid)
- `AuthorName` (string)
- `CategoryId` (Guid)
- `CategoryName` (string)


## Authors

Base route: `/api/authors`

### POST `/api/authors/create`
- Description: Create a new author
- Request body: `CreateAuthorDTO`
  - `Name` (string)
  - `Bio` (string | optional)
- Responses:
  - `201 Created` — created `AuthorDTO`

### GET `/api/authors/{id}`
- Description: Get an author by id
- Responses:
  - `200 OK` — `AuthorDTO`
  - `404 Not Found`

### GET `/api/authors`
- Description: Get all authors
- Responses:
  - `200 OK` — `List<AuthorDTO>`

### PUT `/api/authors/{id}`
- Description: Update an author
- Request body: `UpdateAuthorDTO`
  - `Name` (string)
  - `Bio` (string | optional)
- Responses:
  - `200 OK` — updated `AuthorDTO`

### DELETE `/api/authors/{id}`
- Description: Delete an author
- Responses:
  - `200 OK` — `{ message: "AuthorDeleted" }`

`AuthorDTO` contains:
- `Id` (Guid)
- `Name` (string)
- `Bio` (string?)
- `Books` (List<BookDTO>) — included by mapping


## Categories

Base route: `/api/categories`

### POST `/api/categories/create`
- Description: Create a new category
- Request body: `CreateCategoryDTO`
  - `Name` (string)
- Responses:
  - `201 Created` — created `CategoryDTO`

### GET `/api/categories/{id}`
- Description: Get a category by id
- Responses:
  - `200 OK` — `CategoryDTO`
  - `404 Not Found`

### GET `/api/categories`
- Description: Get all categories
- Responses:
  - `200 OK` — `List<CategoryDTO>`

### PUT `/api/categories/{id}`
- Description: Update a category
- Request body: `UpdateCategoryDTO`
  - `Name` (string)
- Responses:
  - `200 OK` — updated `CategoryDTO`

### DELETE `/api/categories/{id}`
- Description: Delete a category
- Responses:
  - `200 OK` — `{ message: "Category Deleted" }`

`CategoryDTO` fields:
- `Id` (Guid)
- `Name` (string)


---

## Data and Migrations

- The EF Core `AppDbContext` seeds sample categories, authors and books in `OnModelCreating`. Migrations exist in `BookStore.Infrastructure/Migrations` and `Program.cs` applies `db.Database.Migrate()` at startup. Review migrations before running in production.

## Validation

- FluentValidation is used for request validation. Validators are registered from the application assembly.

## Mapping

- AutoMapper `MappingProfile` maps between domain entities and DTOs, and configures nested fields (`AuthorName`, `CategoryName`) on `BookDTO`.

## Notes & Next steps

- Swagger UI is only enabled in Development environment; enable in other environments if needed in `Program.cs`.
- Consider returning typed `ActionResult<T>` and adding `[ProducesResponseType]` attributes to ensure full schema generation in Swagger.
- Secure endpoints (add `[Authorize]` attributes) and add role checks where appropriate.
- Add pagination metadata wrapper for paginated endpoints (total count, page, pageSize) if client needs it.

If you want, I can:
- Add example curl requests for each endpoint.
- Add a Postman collection or OpenAPI export.
- Update controllers to return typed `ActionResult<T>` and add `[ProducesResponseType]` attributes for better Swagger documentation.
