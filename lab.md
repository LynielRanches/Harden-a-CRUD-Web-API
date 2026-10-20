API Lab Log

Part 0 — Setup and Baseline

Invalid POST request: [Record the actual status code and behavior.]

GET /api/products/9999: [Record the actual status code and response.]

Observed problems: The originProducts al API does not validate product input and does not handle missing resources consistently.

Part 1 — Data Annotations

Test: POST a product with an empty name and negative price.

Expected status: 400 Bad Request.

Actual response: [Paste the ValidationProblemDetails response.]

Part 2 — Custom Validation and Exceptions

Custom rule: SalePrice must be less than Price.

Validation message: SalePrice must be less than Price.

Duplicate SKU rule: A duplicate SKU returns 409 Conflict.

Missing product: [Record the behavior before global exception handling.]

Part 3 — Global Exception Handling

Test: GET /api/products/9999.

Expected status: 404 Not Found.

Response body: [Paste the actual Problem Details response.]

Server log: [Paste the corresponding exception log line.]

Part 4 — Standard Responses

POST: [Paste actual 201 response and Location header.]

GET missing product: [Paste actual 404 response.]

Paginated list: [Paste actual data/meta response.]

DELETE: [Record the actual 204 response.]

Reflection

Input validation prevents invalid product data from entering the application. Custom validation and domain exceptions enforce business rules and clearly identify missing resources or conflicts. Centralized exception handling converts exceptions into consistent HTTP responses while keeping internal details in server logs. Standard status codes, Problem Details, and pagination make the API easier for clients to use and troubleshoot. Together, these practices improve the reliability, security, and maintainability of the API.
