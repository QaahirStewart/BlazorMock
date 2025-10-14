# Step 09 — CRUD Operations & Related Data

- Live example: /examples/step9
- Next step: [Step 10 — State Management](./Step10.md)
- Previous: [Step 08 — Setup EF Core & DbContext](./Step08.md)

In this step you will:

- Build list/add pages with validation using `EditForm`
- Use `IDbContextFactory<AppDbContext>` inside components
- Load related data with `Include()`
- Add Delete actions and refresh the list

## Reference pages in the app

- Drivers: `/drivers` — add + list + delete
- Trucks: `/trucks` — add + list + delete
- Routes: `/routes` — add + list (with Driver/Truck) + delete

## Typical patterns

Create a DbContext per operation using the factory:

```csharp
using var db = await DbFactory.CreateDbContextAsync();
```

Query with ordering:

```csharp
drivers = await db.Drivers.OrderBy(d => d.Name).ToListAsync();
```

Include related entities for display:

```csharp
routes = await db.Routes.Include(r => r.Driver)
						.Include(r => r.Truck)
						.OrderBy(r => r.RouteNumber)
						.ToListAsync();
```

Add and reset the form model after saving:

```csharp
db.Drivers.Add(newDriver);
await db.SaveChangesAsync();
newDriver = new() { LicenseLevel = LicenseLevel.ClassC };
await LoadAsync();
```

Delete by key and refresh list:

```csharp
var entity = await db.Drivers.FindAsync(id);
if (entity is not null)
{
	db.Remove(entity);
	await db.SaveChangesAsync();
}
await LoadAsync();
```

With these patterns, you can extend to Edit/Update flows next.
