# Step 9: CRUD Operations

## Overview

This step teaches students how to perform Create, Read, Update, and Delete (CRUD) operations using Entity Framework Core in a Blazor application. Students learn async database operations and best practices.

## Files in This Folder

- `Example.razor` - Tutorial page (route: `/trucking-examples/step9`)
- `Example.razor.cs` - Code-behind with CRUD demo logic
- `README.md` - This file

## Routes

- **Tutorial Page**: `/trucking-examples/step9`

## What Students Learn

1. Reading data with `ToListAsync()` and `FindAsync()`
2. Creating new records with `Add()` and `SaveChangesAsync()`
3. Updating existing records with `Update()` and `SaveChangesAsync()`
4. Deleting records with `Remove()` and `SaveChangesAsync()`
5. Using `IDbContextFactory<T>` for proper DbContext lifecycle management

## Key Concepts

- **ToListAsync()** - Retrieve all records from a table
- **FindAsync()** - Find a record by primary key
- **SaveChangesAsync()** - Commit changes to the database
- **IDbContextFactory** - Factory pattern for creating DbContext instances
- **Async/Await** - Asynchronous programming for database operations

## Architecture

- **DbContext Factory**: `IDbContextFactory<AppDbContext>` for proper scoping
- **Async Operations**: All database calls use async/await
- **Live Demo**: Interactive CRUD interface showing add/delete operations
- **In-Memory Demo**: Demonstrates CRUD patterns without actual database

## Prerequisites

- Step 8: Setup EF Core & DbContext (database configured and migrations applied)

## Next Steps

- Step 10: State Management (sharing data between components)

## Code Structure

### Reading Data

```csharp
await using var db = await DbFactory.CreateDbContextAsync();
var drivers = await db.Drivers.ToListAsync();
```

### Creating Data

```csharp
await using var db = await DbFactory.CreateDbContextAsync();
var newDriver = new Driver { Name = "John", LicenseLevel = "A" };
db.Drivers.Add(newDriver);
await db.SaveChangesAsync();
```

### Updating Data

```csharp
await using var db = await DbFactory.CreateDbContextAsync();
var driver = await db.Drivers.FindAsync(id);
if (driver != null)
{
    driver.Name = "Updated Name";
    await db.SaveChangesAsync();
}
```

### Deleting Data

```csharp
await using var db = await DbFactory.CreateDbContextAsync();
var driver = await db.Drivers.FindAsync(id);
if (driver != null)
{
    db.Drivers.Remove(driver);
    await db.SaveChangesAsync();
}
```

## Live Demo Features

The tutorial includes an interactive demo with:

- List of demo drivers displayed in a table
- Add driver form with name, license, and rate inputs
- Delete button for each driver
- Real-time UI updates after operations
- In-memory list (doesn't affect actual database)

## Styling Patterns

- Table layout for data display
- Form inputs for creating new records
- Action buttons (Add, Delete) with clear styling
- Success/error messaging
- Empty state when no records exist

## Common Issues & Solutions

### DbContext Disposed Error

**Issue**: "Cannot access a disposed context" error  
**Solution**: Use `IDbContextFactory<AppDbContext>` and create a new context for each operation with `await using var db = await DbFactory.CreateDbContextAsync();`

### SaveChanges Not Persisting

**Issue**: Changes don't save to database  
**Solution**: Always call `await db.SaveChangesAsync()` after Add/Update/Remove operations. Verify the database file exists and is writable.

### FindAsync Returns Null

**Issue**: Record not found even though it exists  
**Solution**: Verify you're using the correct primary key value and the record exists in the database. Check that migrations have been applied.

### Concurrent Operations Error

**Issue**: Error when multiple operations happen simultaneously  
**Solution**: Each operation should create its own DbContext using the factory. Never share a DbContext instance across operations.

## Related Resources

- [EF Core Querying Data](https://learn.microsoft.com/en-us/ef/core/querying/)
- [EF Core Saving Data](https://learn.microsoft.com/en-us/ef/core/saving/)
- [Async Programming](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/)
- [DbContext Lifetime](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/)
