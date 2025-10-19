# Step 09 — CRUD Operations & Related Data

- Live example: /examples/step9
- Next step: [Step 10 — State Management](./Step10.md)
- Previous: [Step 08 — Setup EF Core & DbContext](./Step08.md)

In this step you will:

- Build list/add pages with validation using `EditForm`
- Use `IDbContextFactory<AppDbContext>` inside components
- Load related data with `Include()`
- Add Delete actions and refresh the list
- Implement responsive layouts with mobile cards and desktop tables

## Reference pages in the app

- Drivers: `/drivers` — add + list + delete (responsive)
- Trucks: `/trucks` — add + list + delete (responsive)
- Routes: `/routes` — add + list (with Driver/Truck) + delete (responsive)

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

## Responsive CRUD List Pattern

All CRUD pages implement a responsive layout that shows mobile-friendly cards on small screens and efficient tables on desktop.

### Mobile Layout (< 768px)

<details>
  <summary>Show code — Mobile card layout</summary>

```razor
<div class="space-y-3 md:hidden">
    @foreach (var d in drivers)
    {
        <div class="rounded-2xl border border-gray-200 bg-white p-4">
            <div class="mb-3">
                <div class="text-base font-semibold text-gray-900">@d.Name</div>
                <div class="text-sm text-gray-600">License: @d.LicenseNumber</div>
                <div class="text-sm text-gray-600">Level: @d.LicenseLevel</div>
                <div class="text-sm text-gray-600">Experience: @d.YearsOfExperience years</div>
            </div>
            <div class="flex flex-wrap gap-2">
                <button class="px-3 py-2 rounded-lg border hover:bg-gray-50 text-sm"
                    @onclick="() => BeginEdit(d)">Edit</button>
                <button class="px-3 py-2 rounded-lg border hover:bg-gray-50 text-sm"
                    @onclick="() => DeleteAsync(d.Id)">Delete</button>
            </div>
        </div>
    }
</div>
```

</details>

### Desktop Layout (≥ 768px)

<details>
  <summary>Show code — Desktop table layout</summary>

```razor
<div class="overflow-x-auto hidden md:block">
    <table class="min-w-full border border-gray-200 rounded-xl overflow-hidden">
        <thead class="bg-gray-50">
            <tr>
                <th class="text-left p-2 border-b">Name</th>
                <th class="text-left p-2 border-b">License #</th>
                <th class="text-left p-2 border-b">Level</th>
                <th class="text-left p-2 border-b">Experience</th>
                <th class="text-left p-2 border-b">Actions</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var d in drivers)
            {
                <tr class="hover:bg-gray-50">
                    <td class="p-2 border-b">@d.Name</td>
                    <td class="p-2 border-b">@d.LicenseNumber</td>
                    <td class="p-2 border-b">@d.LicenseLevel</td>
                    <td class="p-2 border-b">@d.YearsOfExperience</td>
                    <td class="p-2 border-b">
                        <div class="flex gap-2">
                            <button @onclick="() => BeginEdit(d)">Edit</button>
                            <button @onclick="() => DeleteAsync(d.Id)">Delete</button>
                        </div>
                    </td>
                </tr>
            }
        </tbody>
    </table>
</div>
```

</details>

### Key Points

- Use `md:hidden` to show cards on mobile (< 768px) and hide on desktop
- Use `hidden md:block` to hide tables on mobile and show on desktop (≥ 768px)
- Both layouts support inline editing for feature parity
- Touch-friendly buttons on mobile, compact actions on desktop
- See `/drivers`, `/trucks`, and `/routes` for complete working examples

With these patterns, you can extend to Edit/Update flows next.

Summary:

- Use `IDbContextFactory<AppDbContext>` to create short-lived contexts in components
- Use `Include()` for related data and `OrderBy()` for stable lists
- After writes, reset the form model and reload the list
- For deletes, `FindAsync(id)` → `Remove` → `SaveChangesAsync()` → reload
- Implement responsive layouts with mobile cards (`md:hidden`) and desktop tables (`hidden md:block`)

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
