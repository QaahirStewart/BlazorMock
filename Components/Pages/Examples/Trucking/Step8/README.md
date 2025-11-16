# Step 8: Setup EF Core & DbContext

## Overview

This step teaches students how to set up Entity Framework Core in a Blazor application. Students learn to create a DbContext, configure the database connection, and run migrations.

## Files in This Folder

- `Example.razor` - Tutorial page (route: `/trucking-examples/step8`)
- `Example.razor.cs` - Code-behind with progress tracking
- `README.md` - This file

## Routes

- **Tutorial Page**: `/trucking-examples/step8`

## What Students Learn

1. Creating a DbContext class
2. Defining DbSet<T> properties for entities
3. Configuring connection strings in appsettings.json
4. Registering DbContext in Program.cs
5. Creating and running EF Core migrations

## Key Concepts

- **DbContext** - The main class for EF Core database operations
- **DbSet<T>** - Represents a table in the database
- **Migrations** - Version control for database schema
- **Connection Strings** - Database connection configuration

## Architecture

- **DbContext**: `AppDbContext` with DbSet properties
- **Database**: SQLite (or SQL Server)
- **DI Registration**: `AddDbContextFactory<AppDbContext>()` in Program.cs
- **Migrations**: Created via `dotnet ef migrations add`

## Prerequisites

- Step 7: EF Core Models (understanding of entity classes)

## Next Steps

- Step 9: CRUD Operations (creating, reading, updating, deleting data)

## Code Structure

### AppDbContext

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Truck> Trucks { get; set; }
    public DbSet<Route> Routes { get; set; }
}
```

### Program.cs Registration

```csharp
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=trucking.db"
  }
}
```

## Migration Commands

```bash
# Create a migration
dotnet ef migrations add InitialCreate

# Apply migrations to database
dotnet ef database update

# Remove last migration (if not applied)
dotnet ef migrations remove
```

## Styling Patterns

- Code examples showing file structure
- Terminal commands with proper formatting
- Step-by-step migration workflow
- Configuration file examples

## Common Issues & Solutions

### EF Core Tools Not Found

**Issue**: `dotnet ef` command not recognized  
**Solution**: Install EF Core tools globally: `dotnet tool install --global dotnet-ef`

### Migration Fails to Create

**Issue**: Error when running `dotnet ef migrations add`  
**Solution**: Ensure you have a `<PackageReference>` for `Microsoft.EntityFrameworkCore.Design` in your .csproj file.

### Database Connection Error

**Issue**: Cannot connect to database  
**Solution**: Verify the connection string is correct in appsettings.json and matches your database type (SQLite, SQL Server, etc.).

### DbContext Not Registered

**Issue**: Dependency injection error when injecting DbContext  
**Solution**: Verify `AddDbContextFactory<AppDbContext>()` is called in Program.cs before `builder.Build()`.

## Related Resources

- [EF Core DbContext Documentation](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/)
- [EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Connection Strings](https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-strings)
