# Step 08 — Setup EF Core & DbContext

- Live example: /examples/step8
- Next step: [Step 09 — CRUD Operations & Related Data](./Step09.md)
- Previous: [Step 07 — EF Core Models](./Step07.md)

In this step you will:

- Add a SQLite connection string
- Register `AppDbContext` in `Program.cs`
- Create and apply your first EF Core migration

## 1) Connection string (appsettings.json)

Add or verify a connection string named `Default`:

<details>
  <summary>Show code — appsettings.json</summary>

```
{
	"ConnectionStrings": {
		"Default": "Data Source=blazormock.db"
	}
}
```

</details>

## 2) Register DbContext (Program.cs)

Register an `IDbContextFactory<AppDbContext>` using SQLite, and apply migrations at startup in development:

<details>
  <summary>Show code — Program.cs setup</summary>

```csharp
using BlazorMock.Data;
using Microsoft.EntityFrameworkCore;

var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Data Source=blazormock.db";
builder.Services.AddDbContextFactory<AppDbContext>(options =>
		options.UseSqlite(connectionString));

// ... after app = builder.Build(); inside a scope:
using (var scope = app.Services.CreateScope())
{
		var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
		using var db = dbFactory.CreateDbContext();
		db.Database.Migrate(); // applies pending migrations in dev
}
```

</details>

Note: We use `Migrate()` instead of `EnsureCreated()` so schema changes are tracked in migrations.

## 3) Install CLI tool and create migration

Run these in the project folder:

<details>
	<summary>Show code — EF Core CLI commands</summary>

```pwsh
dotnet tool install --global dotnet-ef   # first time only
dotnet ef migrations add InitialCreate
dotnet ef database update
```

</details>

Results:

- A `Migrations/` folder is added to your project with the migration files.
- A `blazormock.db` SQLite file is created (or updated).

If the database already existed from `EnsureCreated()`, switch to `Migrate()` and then generate a new migration to bring the project into a migrations-based workflow.

You're now ready to build CRUD pages in Step 09.

Summary:

- Connection string in `appsettings.json`
- Register `AppDbContext` via factory + call `Migrate()` on startup (dev)
- Create initial migration and update the database

## How to add in‑app code samples

- Use a white card, then a gray container to visually group the sample.
- Insert a raw `<pre data-code-title="...">` block; the site enhancer adds Show code + Copy automatically.
- Escape `@` as `&#64;` inside static markup (including `<pre>`) when needed.
- No page-level toolbar; controls are per block (global policy).
- See in-app Step 13 and Step 4 for the exact look and feel.

```html
<div class="bg-white rounded-xl p-5 sm:p-6 border border-gray-200">
  <!-- heading/description/bullets ... -->
  <div class="bg-gray-100 rounded-lg p-4 overflow-x-auto break-all">
    <pre data-code-title="Razor + C# (Component.razor)">
@* Paste code here as plain text. Use &#64; to show @ in static examples. *@
		</pre
    >
  </div>
</div>
```
