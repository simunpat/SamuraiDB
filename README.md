# Entity Framework Samurai API

This project is a .NET Web API that demonstrates the use of Entity Framework Core with a MySQL database, implementing a domain model around Samurais, their Weapons, Horses, and Battles.

## Project Structure

The project follows a clean architecture pattern with the following structure:

```
EntityFrameworkOpgave/
├── Controllers/
│   └── SamuraiController.cs      # API endpoints for Samurai operations
├── DAL/                          # Data Access Layer
│   ├── Data/
│   │   └── SamuraiDbContext.cs   # EF Core DbContext
│   ├── Models/
│   │   ├── Samurai.cs            # Domain models
│   │   ├── Battle.cs
│   │   ├── Horse.cs
│   │   ├── Weapon.cs
│   │   └── DTOs/                 # Data Transfer Objects
│   └── Repositories/
│       ├── IRepository.cs        # Generic repository interface
│       └── SamuraiRepository.cs  # Samurai-specific repository
└── Program.cs                    # Application configuration
```

## Domain Model

- **Samurai**: The main entity with properties like Name and Age
  - Has one Horse (one-to-one relationship)
  - Has many Weapons (one-to-many relationship)
  - Participates in many Battles (many-to-many relationship)
- **Horse**: Belongs to one Samurai
- **Weapon**: Belongs to one Samurai
- **Battle**: Can have multiple Samurais

## Prerequisites

- .NET 9.0 SDK
- MySQL Server
- An IDE (Visual Studio, VS Code, etc.)

## Configuration

1. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_mysql_connection_string_here"
  }
}
```

## Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Run the following commands:
```bash
dotnet restore
dotnet run
```
4. Access the Swagger UI at `https://localhost:[port]/swagger`

## API Endpoints

The API provides the following endpoints:

### Samurai Operations

- `GET /api/Samurai` - Get all samurais
- `GET /api/Samurai/{id}` - Get a specific samurai
- `POST /api/Samurai` - Create a new samurai
- `PUT /api/Samurai/{id}` - Update an existing samurai
- `DELETE /api/Samurai/{id}` - Delete a samurai

### Data Transfer Objects (DTOs)

The API uses DTOs for creating and updating Samurais:

#### CreateSamuraiDto
```csharp
{
    "name": "string",
    "age": int,
    "horse": {
        "name": "string",
        "breed": "string"
    },
    "weapons": [
        {
            "name": "string",
            "type": "string"
        }
    ],
    "battleIds": [int]
}
```

#### UpdateSamuraiDto
```csharp
{
    "name": "string",
    "age": int,
    "horse": {
        "id": int?,
        "name": "string",
        "breed": "string"
    },
    "weapons": [
        {
            "id": int?,
            "name": "string",
            "type": "string"
        }
    ],
    "battleIds": [int]
}
```

## Features

- Full CRUD operations for Samurai entities
- Relationship management (Horse, Weapons, Battles)
- Entity Framework Core with MySQL
- Repository pattern implementation
- Swagger UI for API documentation
- Error handling and logging
- JSON cycle handling for complex relationships

## Technologies Used

- ASP.NET Core 9.0
- Entity Framework Core
- MySQL
- Swagger/OpenAPI
- AutoMapper (for object mapping) 