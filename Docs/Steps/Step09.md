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

<details>
	<summary>Show code — Create scoped DbContext</summary>

```csharp
using var db = await DbFactory.CreateDbContextAsync();
```

</details>

Query with ordering:

<details>
	<summary>Show code — Query with ordering</summary>

```csharp
drivers = await db.Drivers.OrderBy(d => d.Name).ToListAsync();
```

</details>

Include related entities for display:

<details>
  <summary>Show code — Include related entities</summary>

```csharp
routes = await db.Routes.Include(r => r.Driver)
						.Include(r => r.Truck)
						.OrderBy(r => r.RouteNumber)
						.ToListAsync();
```

</details>

Add and reset the form model after saving:

<details>
	<summary>Show code — Add and reset form</summary>

```csharp
db.Drivers.Add(newDriver);
await db.SaveChangesAsync();
newDriver = new() { LicenseLevel = LicenseLevel.ClassC };
await LoadAsync();
```

</details>

Delete by key and refresh list:

<details>
  <summary>Show code — Delete and refresh</summary>

```csharp
var entity = await db.Drivers.FindAsync(id);
if (entity is not null)
{
	db.Remove(entity);
	await db.SaveChangesAsync();
}
await LoadAsync();
```

</details>

With these patterns, you can extend to Edit/Update flows next.

Summary:

- Use `IDbContextFactory<AppDbContext>` to create short-lived contexts in components
- Use `Include()` for related data and `OrderBy()` for stable lists
- After writes, reset the form model and reload the list
- For deletes, `FindAsync(id)` → `Remove` → `SaveChangesAsync()` → reload
