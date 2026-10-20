# Lab Log: Production-Grade ASP.NET Core Web API

## Part 0 Checkpoint: Baseline Behaviors
1. **Invalid POST accepted:** Sending a product with an empty name and negative price resulted in a `200 OK` or `201 Created` instead of being rejected.
2. **Missing ID GET behavior:** Requesting `/api/products/9999` returned an unhandled exception stack trace (500 error) or an empty 200 response.

## Part 1 Checkpoint: Validation Response (400 Bad Request)
json
{
  "type": "[https://tools.ietf.org/html/rfc9110#section-15.5.1](https://tools.ietf.org/html/rfc9110#section-15.5.1)",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": ["Product name is required."],
    "Price": ["Price must be greater than zero."]
  }
}
Part 2 Checkpoint: Custom Rule
Rule implemented: Cross-property check ensuring SalePrice is strictly less than Price, and SKU uniqueness verification via service logic.

Message returned: "Sale price must be less than the regular price."

Part 3 Checkpoint: Global Exception Handling
404 Problem Details Body:

JSON
{
  "type": "[https://tools.ietf.org/html/rfc9110#section-15.4.5](https://tools.ietf.org/html/rfc9110#section-15.4.5)",
  "title": "Resource Not Found",
  "status": 404,
  "detail": "Product with ID 9999 was not found.",
  "instance": "/api/products/9999"
}
Server Log Match: [Error] GlobalExceptionHandler: An unhandled exception occurred: Product with ID 9999 was not found.

Part 4 Checkpoint: Standardized Responses
201 Created Response: Returns headers Location: /api/products/4 with the created resource body.

404 Not Found: Standard Problem Details JSON (shown in Part 3).

Paged List Envelope:

JSON
{
  "data": [
    { "id": 1, "name": "Mechanical Keyboard", "sku": "KB001", "price": 1500 }
  ],
  "meta": {
    "page": 1,
    "pageSize": 10,
    "totalCount": 1,
    "totalPages": 1
  }
}

Reflection

Input validation prevents invalid product data from entering the application. Custom validation and domain exceptions enforce business rules and clearly identify missing resources or conflicts. Centralized exception handling converts exceptions into consistent HTTP responses while keeping internal details in server logs. Standard status codes, Problem Details, and pagination make the API easier for clients to use and troubleshoot. Together, these practices improve the reliability, security, and maintainability of the API.




### Review Questions & Answers

#### Lesson 1 — Input Validation
1. **Why is server-side validation mandatory even when the client already validates?**
   * Client-side validation can easily be bypassed using tools like Postman, curl, or modified client scripts. Server-side validation guarantees data integrity, security, and consistent business rules enforcement regardless of the client source.
2. **What does `[ApiController]` do automatically when ModelState is invalid, and which status code results?**
   * It automatically inspects `ModelState` and short-circuits execution to return a `400 Bad Request` containing a `ValidationProblemDetails` payload before the action method even executes.
3. **When would you use `IValidatableObject` instead of a Data Annotation attribute?**
   * You use `IValidatableObject` when validation logic requires comparing multiple properties within the model against each other (cross-property validation) or depends on context outside a single property.

#### Lesson 2 — Exception Handling
1. **Give two reasons centralized exception handling beats try/catch in every action.**
   * It eliminates massive code duplication across controllers and ensures a uniform error-handling policy globally without cluttering business logic.
2. **What does `IExceptionHandler.TryHandleAsync` return, and what does returning true signify?**
   * It returns a `ValueTask<bool>`. Returning `true` signifies that the exception was successfully handled, preventing the pipeline from falling back to default error handlers.
3. **Why must production error responses never include the stack trace, and where should it go instead?**
   * Stack traces reveal internal code structures, framework versions, and database schemas, exposing security vulnerabilities. They should instead be securely logged server-side via a structured logging framework (like Serilog or `ILogger`).

#### Lesson 3 — Standard API Responses
1. **Match each to a status code: a created resource, a successful delete, a missing resource, a duplicate key.**
   * Created resource: `201 Created`
   * Successful delete: `204 No Content`
   * Missing resource: `404 Not Found`
   * Duplicate key: `409 Conflict`
2. **What does the Problem Details standard (RFC 9457) standardize, and name three of its fields.**
   * It standardizes the machine-readable format of error responses in HTTP APIs. Key fields include: `type`, `title`, `status`, `detail`, and `instance`.
3. **State one benefit and one drawback of wrapping success responses in a data/meta envelope.**
   * *Benefit:* Easily carries pagination metadata, sorting, and filtering state alongside collection items.
   * *Drawback:* Breaks standard direct resource mapping for single items vs collections, requiring clients to unpack responses differently depending on the endpoint type.
