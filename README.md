# Products CRUD API

## Overview

An ASP.NET Core Web API that manages products using an in-memory collection. It implements input validation, custom business rules, centralized exception handling, standardized HTTP responses, and pagination.

## Technologies

- C# and ASP.NET Core Web API
- Data Annotations and `IValidatableObject`
- `IExceptionHandler` and Problem Details
- Swagger / OpenAPI

## Running the Project

1. Open the project in Visual Studio or VS Code.
2. Restore the project dependencies.
3. Run the application using `dotnet run`.
4. Open the Swagger URL shown in the terminal.

## Endpoints

| Method | Endpoint | Success | Error |
|---|---|---|---|
| GET | `/api/products` | 200 OK | 400 Bad Request |
| GET | `/api/products/{id}` | 200 OK | 404 Not Found |
| POST | `/api/products` | 201 Created + Location | 400, 409 |
| PUT | `/api/products/{id}` | 200 OK | 400, 404, 409 |
| DELETE | `/api/products/{id}` | 204 No Content | 404 |

Unexpected server errors return `500 Internal Server Error`.

## Validation and Error Handling

Request models use Data Annotations to validate required fields, string lengths, and numeric ranges. `IValidatableObject` checks that the sale price is less than the regular price. Duplicate SKUs produce a conflict exception. A centralized exception handler maps domain exceptions to Problem Details responses and logs exceptions on the server.

## Pagination

The list endpoint supports `page` and `pageSize` query parameters. Collection responses contain `data` and `meta` properties.

Example: `/api/products?page=1&pageSize=10`

## Limitations

The product collection is stored in memory. Changes are lost when the application restarts. A production deployment would require persistent storage and appropriate concurrency handling.
