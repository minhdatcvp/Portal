# Portal Project

## Overview
This is a multi-project solution for the **Portal** application, which consists of the following main components:

- **Portal**: The main web application.
- **PortalCommon**: Contains shared models and configuration settings.
- **PortalRepository**: Handles data persistence with repositories for database and file operations.
- **PortalService**: Provides business logic and services.
- **PortalTest**: Contains unit tests for the application.

## Project Structure

```
PortalSolution
│-- Portal
│   ├── Controllers
│   ├── Models
│   ├── Views
│   ├── wwwroot (static files)
│   ├── DependencyInjection.cs
│   ├── Program.cs
│   ├── appsettings.json
│-- PortalCommon
│   ├── Model (Shared data models)
│   ├── Appsetting.cs
│-- PortalRepository
│   ├── Repository (DB and File repositories)
│-- PortalService
│   ├── Services (Business logic services)
│-- PortalTest
│   ├── ServiceTest (Unit tests)
```

## Prerequisites

- .NET SDK (Version 6.0 or later)
- Visual Studio (recommended) or any compatible IDE

## Setup and Run

### 1. Clone the Repository
```sh
git clone <repository-url>
cd Portal
```

### 2. Restore Dependencies
```sh
dotnet restore
```

### 3. Configure Database (if applicable)
- Update `appsettings.json` in **Portal** project with the correct database connection string.
```

### 4. Build the Solution
```sh
dotnet build
```

### 5. Run the Application
```sh
dotnet run --project Portal
```

### 6. Run Tests
```sh
dotnet test
```

## API Endpoints
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/home/index` | GET | Loads the home page |
| `/api/marketdata` | GET | Fetches market data |

