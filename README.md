# Abstra - ASP.NET Core REST API

A layered REST API application for managing Countries, States, and Cities with a three-level hierarchy (Country → State → City), built with ASP.NET Core 8 following IDesign architecture principles.

## Architecture

The solution follows IDesign's layered architecture pattern:

- **Abstra.Api (Host)** - Controllers, Middleware, Configuration
- **Abstra.Application (Application)** - Business Logic, Services, Handlers
- **Abstra.Repository (Repository)** - Data Access, EF Core, Repositories
- **Abstra.Domain** - Entities, DTOs, Contracts
- **Abstra.Tests** - Unit, Integration, Acceptance Tests

## Features

- ✅ RESTful API with CRUD operations for Countries, States, and Cities
- ✅ JWT Bearer Token Authentication
- ✅ Logging Middleware for request/response tracking
- ✅ Error Handling Middleware for standardized error responses
- ✅ Entity Framework Core with SQL Server LocalDB
- ✅ Comprehensive test coverage (Unit, Integration, Acceptance)
- ✅ Swagger/OpenAPI documentation

## Prerequisites

- .NET 8 SDK
- SQL Server LocalDB (or SQL Server)
- Node.js 18+ and npm (for frontend)

## Getting Started

1. **Restore packages and build:**
   ```bash
   dotnet restore
   dotnet build
   ```

2. **Update database:**
   ```bash
   dotnet ef database update --project Abstra.Repository --startup-project Abstra.Api
   ```

3. **Run the application:**
   ```bash
   cd Abstra.Api
   dotnet run
   ```

4. **Access Swagger UI:**
   - Navigate to `https://localhost:5001/swagger` (or the port shown in console)

## Running the Frontend

1. **Navigate to the frontend directory:**
   ```bash
   cd Abstra.Frontend
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Start the development server:**
   ```bash
   npm run dev
   ```

4. **Access the application:**
   - The frontend will be available at `http://localhost:5173`
   - Make sure the API is running on `https://localhost:7246` (or update the API URL in `src/utils/constants.ts`)

**Note:** The frontend is configured to connect to the API at `https://localhost:7246/api` by default. If your API runs on a different port, update the `API_BASE_URL` constant in `Abstra.Frontend/src/utils/constants.ts` file.

## API Endpoints

### Authentication
- `POST /api/auth/login` - Login and get JWT token

**Default Credentials (for demo):**
- Username: `admin`
- Password: `admin123`

**Note:** For demo purposes, any non-empty username/password will work. In production, implement proper user authentication and password hashing.

### Countries
- `GET /api/countries` - Get all countries (requires authentication)
- `GET /api/countries/{id}` - Get country by ID
- `POST /api/countries` - Create country
- `PUT /api/countries/{id}` - Update country
- `DELETE /api/countries/{id}` - Delete country

### States
- `GET /api/states` - Get all states
- `GET /api/states/{id}` - Get state by ID
- `GET /api/states/country/{countryId}` - Get states by country
- `POST /api/states` - Create state
- `PUT /api/states/{id}` - Update state
- `DELETE /api/states/{id}` - Delete state

### Cities
- `GET /api/cities` - Get all cities
- `GET /api/cities/{id}` - Get city by ID
- `GET /api/cities/state/{stateId}` - Get cities by state
- `POST /api/cities` - Create city
- `PUT /api/cities/{id}` - Update city
- `DELETE /api/cities/{id}` - Delete city

**Note:** Cities belong to States, which belong to Countries. The hierarchy is: Country → State → City.

## Testing

Run all tests:
```bash
dotnet test
```

Run specific test categories:
```bash
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"
```

## Configuration

JWT settings and database connection string can be configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AbstraDb;Trusted_Connection=true;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!",
    "Issuer": "AbstraApi",
    "Audience": "AbstraApi",
    "ExpirationMinutes": "1440"
  }
}
```

## Project Structure

```
Abstra/
├── Abstra.Api/              # Web API project
│   ├── Controllers/         # API controllers
│   ├── Middleware/          # Custom middleware
│   └── Program.cs           # Application startup
├── Abstra.Application/       # Application layer
│   ├── Services/            # Business services
│   └── Handlers/            # MediatR handlers
├── Abstra.Repository/       # Data access layer
│   ├── Data/                # DbContext
│   └── Repositories/        # Repository implementations
├── Abstra.Domain/           # Domain layer
│   ├── Entities/            # Domain entities
│   ├── DTOs/                # Data transfer objects
│   ├── Contracts/           # Interfaces
│   ├── Commands/            # CQRS Commands
│   └── Queries/             # CQRS Queries
├── Abstra.Frontend/         # React frontend application
│   ├── src/
│   │   ├── components/      # React components
│   │   ├── pages/          # Page components
│   │   ├── services/       # API services
│   │   └── contexts/       # Context providers
│   └── package.json
└── Abstra.Tests/            # Test project
    ├── Unit/                # Unit tests
    ├── Integration/         # Integration tests
    └── Acceptance/          # Acceptance tests
```

## Technologies Used

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server LocalDB
- JWT Authentication
- MediatR (CQRS pattern)
- xUnit, Moq, FluentAssertions
- Swagger/OpenAPI

### Frontend
- React 18 with TypeScript
- Vite
- Material-UI (MUI)
- React Router v6
- Axios
- Context API

## Data Model

The application uses a three-level hierarchy:

- **Country**: Top-level entity (e.g., "United States", "Brazil")
- **State**: Belongs to a Country (e.g., "New York", "São Paulo")
- **City**: Belongs to a State (e.g., "New York City", "São Paulo")

Each entity has:
- Unique identifier (Id)
- Name
- Code (for Countries and States)
- Created timestamp
- Navigation properties for relationships

## Notes

- Authentication is simplified for demo purposes. In production, implement proper user authentication and password hashing.
- The database is automatically created and migrations are applied on first run.
- All endpoints require JWT authentication except `/api/auth/login`.
- CORS is configured to allow requests from `http://localhost:5173` (frontend development server).
