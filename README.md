# Currency Rates Dashboard

A real-time currency rates dashboard built with .NET 8, Vue.js, and SQL Server, following Clean Architecture and DDD principles.

## Features

- Hourly currency rate updates from FX Rates API
- Filterable and sortable data grid
- CQRS pattern implementation
- Clean Architecture with Domain-Driven Design

## Prerequisites

- .NET 8 SDK
- Node.js (v18+)
- Docker and Docker Compose
- Visual Studio Code or Visual Studio 2022

## Project Structure

```
src/
├── Core/                          # Core layers
│   ├── Domain/                    # Domain entities, interfaces
│   └── Application/               # Application services, CQRS
├── Infrastructure/                # Infrastructure implementations
│   ├── Infrastructure/            # EF Core, repositories
│   └── Jobs/                      # Hangfire jobs
├── Hosts/                         # Application hosts
│   ├── Api/                       # WebAPI
│   └── Worker/                    # Worker Service
└── Presentation/                  # Frontend
    └── Vue/                       # Vue.js application
```

## Getting Started

1. Start SQL Server:
   ```bash
   docker-compose up -d
   ```

2. Apply database migrations:
   ```bash
   cd src/Hosts/Hahn.CurrencyRates.Api
   dotnet ef database update
   ```

3. Start the Worker Service:
   ```bash
   cd src/Hosts/Hahn.CurrencyRates.Worker
   dotnet run
   ```

4. Start the WebAPI:
   ```bash
   cd src/Hosts/Hahn.CurrencyRates.Api
   dotnet run
   ```

5. Start the Vue.js frontend:
   ```bash
   cd src/Presentation/Hahn.CurrencyRates.Vue
   npm install
   npm run dev
   ```

The application will be available at:
- Frontend: http://localhost:5173
- WebAPI: http://localhost:5000
- SQL Server: localhost,1433

## Configuration

### API Settings
Update the following files with your settings:

1. Worker Service (`src/Hosts/Hahn.CurrencyRates.Worker/appsettings.json`):
   - Database connection string
   - FX Rates API key
   - Hangfire settings

2. WebAPI (`src/Hosts/Hahn.CurrencyRates.Api/appsettings.json`):
   - Database connection string
   - CORS settings

## Architecture

The solution follows Clean Architecture principles:

1. Domain Layer:
   - Contains business entities
   - Defines repository interfaces
   - No dependencies on other layers

2. Application Layer:
   - Implements CQRS pattern
   - Contains business logic
   - Depends only on Domain layer

3. Infrastructure Layer:
   - Implements repositories
   - Handles data access
   - Contains external service integrations

4. Presentation Layer:
   - Vue.js frontend
   - Uses PrimeVue components
   - TypeScript implementation

## Development

### Adding Migrations
```bash
cd src/Infrastructure/Hahn.CurrencyRates.Infrastructure
dotnet ef migrations add [MigrationName] --startup-project ../../Hosts/Hahn.CurrencyRates.Api/Hahn.CurrencyRates.Api.csproj
```

### Running Tests
```bash
dotnet test
