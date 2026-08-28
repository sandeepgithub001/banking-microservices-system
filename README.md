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

### Quick Start (PowerShell scripts)

Run both backend and frontend together:

```powershell
# Terminal 1 - starts all 5 backend services (each in its own window)
.\start-backend.ps1

# Terminal 2 - starts the Angular frontend (installs npm deps if needed)
.\start-frontend.ps1
```

Then open **http://localhost:4200** in your browser.

To stop everything:

```powershell
.\stop-all.ps1
```

### Manual Start (per service)

1. Start `MicroBank.ServiceRegistry` on port `8500` (acts as Consul; start first so others can register).
2. Start `MicroBank.ConfigService` on port `5000`.
3. Start `MicroBank.CustomerService` on port `6000`.
4. Start `MicroBank.AccountService` on port `6001`.
5. Start `MicroBank.ApiGateway` on port `7000` (frontend talks to this).
6. Start the Angular frontend:

```powershell
cd frontend
npm install   # first time only
npm start     # serves at http://localhost:4200
```

Each service's port is configured in its `Properties/launchSettings.json`, so plain `dotnet run` binds the correct port automatically.

### Service Ports

| Service | Port | Purpose |
|---|---|---|
| ServiceRegistry | 8500 | Consul-compatible service discovery |
| ConfigService | 5000 | Centralized configuration |
| CustomerService | 6000 | Customer CRUD |
| AccountService | 6001 | Accounts, deposits, withdrawals |
| ApiGateway | 7000 | Ocelot gateway (frontend entry point) |
| Angular frontend | 4200 | UI (calls gateway at 7000) |

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
