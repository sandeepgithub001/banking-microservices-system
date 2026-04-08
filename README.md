# MicroBank

MicroBank is a microservices-based banking application built using Angular, ASP.NET Core, Entity Framework Core, and SQL Server.

## Architecture Overview

- **Microservices architecture**: Services are separated by business capability.
- **Customer Management Service**: Handles customer CRUD operations and coordinates account cleanup on deletion.
- **Account Management Service**: Manages account lifecycle, deposit/withdraw, transaction history, and validates customers.
- **API Gateway**: Provides a single entry point for frontend requests and routes traffic to service backends.
- **Service Discovery**: Uses Consul for auto-registration and dynamic resolution of service instances.
- **Centralized Configuration**: Config Service exposes environment-based settings for connection strings and service URLs.
- **Database-per-service**: Each microservice maintains its own SQL Server database.

## Solution Structure

- `src/MicroBank.CustomerService`
- `src/MicroBank.AccountService`
- `src/MicroBank.ApiGateway`
- `src/MicroBank.ServiceRegistry`
- `src/MicroBank.ConfigService`
- `frontend` (Angular starter app)

## Running MicroBank

1. Start Consul locally on `http://localhost:8500`.
2. Start `MicroBank.ConfigService`.
3. Start `MicroBank.ServiceRegistry`.
4. Start `MicroBank.CustomerService` on port `6000`.
5. Start `MicroBank.AccountService` on port `6001`.
6. Start `MicroBank.ApiGateway` on port `7000`.
7. Start the Angular frontend after installing npm dependencies.

## Sample API Requests

- `POST http://localhost:7000/customers` to create a customer.
- `GET http://localhost:7000/customers` to list customers.
- `GET http://localhost:7000/customers/{id}` to get customer details.
- `POST http://localhost:7000/accounts/deposit` to deposit funds.
- `POST http://localhost:7000/accounts/withdraw` to withdraw funds.
- `GET http://localhost:7000/accounts/{accountId}` to retrieve an account.
- `DELETE http://localhost:7000/accounts/{accountId}` to delete an account.

## Notes

- SQL Server connection strings are configured in each service's `appsettings.json`.
- Customer deletion triggers a call to Account Service to remove customer accounts.
- Account Service validates customers by calling Customer Service via `HttpClientFactory`.
- Global exception middleware returns consistent error payloads.
