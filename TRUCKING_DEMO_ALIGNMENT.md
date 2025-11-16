# Trucking Schedule Demo - Tutorial Alignment

This document shows how the Trucking Schedule Demo (`/demo/trucking-schedule`) demonstrates all concepts taught in the 13-step tutorial.

## Step-by-Step Coverage

### ✅ Step 1: Project Setup (npm, Tailwind CSS, Asset Pipeline)

**Location in Demo:**

- Entire UI uses Tailwind CSS utility classes
- Responsive grid layouts (`grid-cols-1 sm:grid-cols-2 lg:grid-cols-4`)
- Custom styled components (rounded-2xl, border, bg-white, etc.)
- Color-coded status badges with conditional styling

**Specific Examples:**

- KPI cards: `rounded-2xl border border-gray-200 bg-white p-4`
- Status badges: `bg-yellow-50 text-yellow-700 border border-yellow-200`
- Responsive breakpoints: `hidden md:block` for desktop table view

**Tutorial Reference:** The demo is the culmination of setting up Tailwind in Step 1

---

### ✅ Step 2: Component Composition

**Location in Demo:**

- Main demo component includes multiple child sections
- Reusable card patterns for KPIs
- Mobile vs desktop conditional rendering
- Cost breakdown section as a composed component

**Tutorial Reference:** Component structure follows patterns taught in Step 2

---

### ✅ Step 3: Event Binding (@onclick)

**Location in Demo:**

- Lines 251-257: Mobile card action buttons
- Lines 292-298: Desktop table action buttons
- Route operations: Start, Complete, Cancel, Delete
- Filter controls: Refresh, Reset buttons
- Cost breakdown show/hide toggle

**Specific Code:**

```razor
<button @onclick="() => StartRouteAsync(r.Id)">Start</button>
<button @onclick="() => selectedRouteForBreakdown = r">💰 Costs</button>
<button @onclick="ResetFilters">Reset</button>
```

---

### ✅ Step 4: Two-way Binding (@bind)

**Location in Demo:**

- Lines 60-62: Search input binding
- Lines 64-73: Status filter dropdown binding
- Lines 75-84: Route type filter dropdown binding

**Specific Code:**

```razor
<input @bind="search" placeholder="e.g., R-1001 or Los Angeles" />
<select @bind="statusFilterString">...</select>
<select @bind="typeFilterString">...</select>
```

---

### ✅ Step 5: Forms and Validation

**Location in Demo:**

- Lines 157-171: Validation errors display section
- Business rule violations shown in red alert box
- Form validation enforced before route operations
- Quick Actions section links to CRUD pages with full EditForm examples

**Notes:** Full EditForm examples are on dedicated /drivers, /trucks, and /routes pages. The demo shows the _result_ of validation.

---

### ✅ Step 6: Routing and Navigation

**Location in Demo:**

- Page route: `@page "/demo/trucking-schedule"`
- Lines 23: Back to Guide link
- Lines 105-109: Quick Actions navigation to CRUD pages
- Query parameter support: `/drivers?from=demo`

---

### ✅ Step 7: Data Models (POCO with Data Annotations)

**Location in Demo:**

- Uses `Driver`, `Truck`, and `Route` models throughout
- Display annotations used for enum values (RouteStatus, RouteType, LicenseLevel)
- Model properties accessed: `r.RouteNumber`, `d.Name`, `t.GetDisplayName()`

**Models Used:**

- `Route`: RouteNumber, Origin, Destination, DistanceMiles, Revenue, Status, Type
- `Driver`: Name, LicenseLevel, YearsOfExperience, IsAvailable
- `Truck`: TruckNumber, Class, CapacityLbs, IsAvailable, InMaintenance

**Tutorial Reference:** Lines 226-247 show all model properties in mobile cards

---

### ✅ Step 8: DbContext and EF Core Setup

**Location in Demo:**

- Line 6: `@inject IDbContextFactory<AppDbContext> DbFactory`
- Lines 398-404: DbContext factory usage pattern
- Database queries with Include() for relationships
- SQLite database referenced in "About this demo" section

**Specific Code:**

```csharp
using var db = await DbFactory.CreateDbContextAsync();
routes = await db.Routes
    .Include(r => r.Driver)
    .Include(r => r.Truck)
    .OrderBy(r => r.ScheduledStartDate)
    .ToListAsync();
```

---

### ✅ Step 9: Async CRUD Operations

**Location in Demo:**

**ToListAsync() - Lines 398-404:**

```csharp
routes = await db.Routes
    .Include(r => r.Driver)
    .Include(r => r.Truck)
    .OrderBy(r => r.ScheduledStartDate)
    .ToListAsync();
```

**FirstOrDefaultAsync() - Lines 458-459:**

```csharp
var route = await db.Routes
    .Include(r => r.Driver)
    .Include(r => r.Truck)
    .FirstOrDefaultAsync(r => r.Id == routeId);
```

**SaveChangesAsync() - Lines 479, 570, 607:**

```csharp
await db.SaveChangesAsync();
```

**Include() for Eager Loading:**

- Used in all queries to load related Driver and Truck entities
- Prevents N+1 query problems
- Enables accessing navigation properties (r.Driver.Name, r.Truck.Class)

---

### ✅ Step 10: Service Lifetimes and State Management

**Location in Demo:**

**Service Injection - Lines 8-11:**

```razor
@inject IDbContextFactory<AppDbContext> DbFactory
@inject ScheduleService ScheduleService
@inject TruckingValidationService ValidationService
@inject AppState AppState
```

**StateHasChanged() - Lines 421, 536:**

```csharp
StateHasChanged(); // After KPI calculations
StateHasChanged(); // After validation error display
```

**AppState Pattern:**

- AppState service injected (demonstrates scoped service pattern)
- Event-based notifications architecture in AppState service
- Used to share state across components

**Service Registration (Program.cs):**

```csharp
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<ScheduleService>();
builder.Services.AddScoped<TruckingValidationService>();
```

---

### ✅ Step 11: Business Validation (@bind-Value:after, Switch Expressions)

**Location in Demo:**

**TruckingValidationService Usage - Lines 509-537:**

```csharp
// Validate route status
if (!ValidationService.CanStartRoute(route))
{
    ShowToast("Only scheduled routes can be started", true);
    return;
}

// Validate assignment
var errors = ValidationService.ValidateAssignment(route.Driver, route.Truck, route);
if (errors.Any())
{
    validationErrors = errors;
    ShowToast($"Cannot start route: {errors.Count} validation error(s)", true);
    StateHasChanged();
    return;
}
```

**Validation Rules Enforced:**

- License compatibility (Hazmat routes require CDL-A)
- Resource availability (driver and truck must be available)
- Route type requirements (Oversized requires 3+ years experience)
- Status validation (only scheduled routes can be started)

**Switch Expressions - Line 345:**

```csharp
private string GetStatusBadgeClass(RouteStatus status) => status switch
{
    RouteStatus.Scheduled   => "inline-flex px-2 py-1 rounded-full text-xs bg-yellow-50...",
    RouteStatus.InProgress  => "inline-flex px-2 py-1 rounded-full text-xs bg-blue-50...",
    RouteStatus.Completed   => "inline-flex px-2 py-1 rounded-full text-xs bg-green-50...",
    RouteStatus.Cancelled   => "inline-flex px-2 py-1 rounded-full text-xs bg-gray-100...",
    RouteStatus.Delayed     => "inline-flex px-2 py-1 rounded-full text-xs bg-orange-50...",
    _                        => "inline-flex px-2 py-1 rounded-full text-xs bg-gray-100..."
};
```

**Validation Error Display - Lines 157-171:**

- Red alert box shows validation errors when rules are violated
- User-friendly error messages from validation service
- Clear button to dismiss validation errors

---

### ✅ Step 12: Service Pattern for Business Logic

**Location in Demo:**

**ScheduleService Usage - Lines 110-146:**

```csharp
var driverPay = ScheduleService.CalculateDriverPay(selectedRouteForBreakdown.Driver, selectedRouteForBreakdown);
var fuelCost = ScheduleService.EstimateFuelCost(selectedRouteForBreakdown.DistanceMiles);
var totalCost = ScheduleService.CalculateTotalRouteCost(selectedRouteForBreakdown.Driver, selectedRouteForBreakdown);
var minRevenue = ScheduleService.SuggestMinimumRevenue(selectedRouteForBreakdown.Driver, selectedRouteForBreakdown);
var actualProfit = selectedRouteForBreakdown.Revenue - totalCost;
```

**Interactive Cost Breakdown:**

1. Click "💰 Costs" button on any route
2. See detailed breakdown section appear (lines 103-155)
3. Displays:
   - Driver Pay (base + experience bonus + route bonuses)
   - Fuel Cost (distance ÷ MPG × fuel price)
   - Total Cost (driver pay + fuel + other costs)
   - Suggested Minimum Revenue (20% profit margin)
   - Actual Profit (green if positive, red if negative)

**Business Logic in ScheduleService:**

- `CalculateDriverPay()`: Considers experience, route type bonuses
- `EstimateFuelCost()`: Uses MPG and fuel price constants
- `CalculateTotalRouteCost()`: Combines all expense categories
- `SuggestMinimumRevenue()`: Applies 20% profit margin

**KPI Calculations - Lines 407-418:**

```csharp
// Calculate estimated profit using ScheduleService
decimal totalCost = 0;
foreach (var route in activeRoutes.Where(r => r.Driver is not null))
{
    totalCost += ScheduleService.CalculateTotalRouteCost(route.Driver!, route);
}
kpiEstimatedProfit = kpiTotalRevenue - totalCost;
```

---

### ✅ Step 13: LINQ Aggregation and Conditional Styling

**Location in Demo:**

**LINQ Aggregation for KPIs - Lines 407-421:**

```csharp
var activeRoutes = routes.Where(r => r.Status == RouteStatus.Scheduled || r.Status == RouteStatus.InProgress).ToList();

kpiScheduled = routes.Count(r => r.Status == RouteStatus.Scheduled && r.ScheduledStartDate <= next7);
kpiInProgress = routes.Count(r => r.Status == RouteStatus.InProgress);
kpiTotalRevenue = activeRoutes.Sum(r => r.Revenue);
kpiDriversAvailable = await db.Drivers.CountAsync(d => d.IsAvailable);
kpiTrucksAvailable = await db.Trucks.CountAsync(t => t.IsAvailable && !t.InMaintenance);
```

**LINQ Operations Used:**

- `Where()`: Filtering routes by status and date
- `Count()`: Counting scheduled and in-progress routes
- `Sum()`: Calculating total revenue
- `OrderBy()`: Sorting routes by scheduled date
- `GroupBy()`: Grouping routes by date (line 192)
- `Any()`: Checking if filtered results exist

**Filtering Logic - Lines 318-334:**

```csharp
private IEnumerable<Route> FilteredRoutes()
{
    IEnumerable<Route> q = routes;
    if (!string.IsNullOrWhiteSpace(search))
    {
        var s = search.Trim().ToLowerInvariant();
        q = q.Where(r => (r.RouteNumber?.ToLower().Contains(s) ?? false)
                      || (r.Origin?.ToLower().Contains(s) ?? false)
                      || (r.Destination?.ToLower().Contains(s) ?? false));
    }
    // ... status and type filters
    return q;
}
```

**Conditional Styling - Lines 345-354:**

- Switch expression returns different Tailwind classes based on RouteStatus
- Color-coded badges: Yellow (Scheduled), Blue (In Progress), Green (Completed), Red (Delayed), Gray (Cancelled)
- Applied throughout mobile and desktop views

**KPI Display - Lines 27-51:**

- 4 primary KPIs in responsive grid
- Total Revenue: Sum aggregation
- Est. Profit: Revenue minus calculated costs
- Scheduled/In Progress: Count aggregations with date filters
- Additional resource availability KPIs (drivers/trucks)

---

## Interactive Features

### Cost Breakdown Demo (Step 12)

**How to Use:**

1. Click any "💰 Costs" button on a route
2. Blue card appears showing detailed calculations
3. See experience bonuses, route type bonuses, fuel costs
4. Compare actual profit vs suggested minimum revenue
5. Color-coded profit (green=profitable, red=losing money)

### Validation Demo (Step 11)

**How to Trigger:**

1. Try to start a route without proper assignments
2. Red validation error box appears
3. Lists all business rule violations
4. Examples: Missing driver, wrong license level, resources unavailable

### LINQ Filtering Demo (Step 13)

**How to Use:**

1. Use search box to filter by route number, origin, or destination
2. Select status filter (Scheduled, In Progress, etc.)
3. Select type filter (Standard, Hazmat, Oversized, LongHaul)
4. Watch KPIs recalculate with aggregations
5. See grouped results by date

### State Management Demo (Step 10)

**How to Observe:**

1. Start a route - watch driver and truck become unavailable in KPIs
2. Complete a route - watch resources become available again
3. KPIs update immediately via StateHasChanged()
4. Resource availability counts recalculated on every load

---

## Code Organization

### Services Used

- **AppDbContext** (Step 8): Database access via EF Core
- **ScheduleService** (Step 12): Business calculations for routes
- **TruckingValidationService** (Step 11): Business rule validation
- **AppState** (Step 10): Shared state management with events

### Models Used

- **Route** (Step 7): RouteNumber, Origin, Destination, DistanceMiles, Revenue, Status, Type, ScheduledStartDate, ActualStartDate, ActualEndDate
- **Driver** (Step 7): Name, LicenseLevel, YearsOfExperience, IsAvailable
- **Truck** (Step 7): TruckNumber, Class, CapacityLbs, IsAvailable, InMaintenance

### Key Methods

- **LoadAsync()**: Demonstrates Include(), ToListAsync(), LINQ aggregations
- **StartRouteAsync()**: Validation service, state management, SaveChangesAsync()
- **CompleteRouteAsync()**: Status updates, resource release
- **CancelRouteAsync()**: Conditional logic, resource cleanup
- **FilteredRoutes()**: LINQ Where() with multiple predicates
- **GetStatusBadgeClass()**: Switch expressions for conditional styling

---

## Tutorial Step Summary

| Step | Concept           | Demo Implementation                                |
| ---- | ----------------- | -------------------------------------------------- |
| 1    | npm, Tailwind CSS | All styling uses Tailwind utilities                |
| 2    | Components        | Composed sections, reusable patterns               |
| 3    | Event Binding     | @onclick on all action buttons                     |
| 4    | Two-way Binding   | @bind on search and filter inputs                  |
| 5    | Forms/Validation  | Validation error display, links to EditForm pages  |
| 6    | Routing           | Page route, navigation links                       |
| 7    | Data Models       | Driver, Truck, Route POCOs                         |
| 8    | DbContext/EF      | DbContextFactory, Include()                        |
| 9    | Async CRUD        | ToListAsync, FirstOrDefaultAsync, SaveChangesAsync |
| 10   | Services/State    | Scoped services, StateHasChanged, AppState events  |
| 11   | Validation        | TruckingValidationService, switch expressions      |
| 12   | Business Logic    | ScheduleService calculations, cost breakdown       |
| 13   | LINQ/Styling      | Count, Sum, Where, GroupBy, conditional classes    |

---

## How to Test the Demo

1. **Run the application**: `dotnet run` or `dotnet watch`
2. **Navigate to**: `/demo/trucking-schedule`
3. **Reset demo data**: Click "Reset Demo Data" to restore sample data
4. **Test filtering**: Use search box and dropdowns
5. **Test cost breakdown**: Click "💰 Costs" on any route
6. **Test validation**: Try to start routes with missing assignments
7. **Test operations**: Start → Complete → view KPI updates
8. **Test responsive design**: Resize browser to see mobile/desktop views

---

## Notes for Learners

This demo is the **complete implementation** that the tutorial teaches piece by piece. Each step in the tutorial focuses on one concept, but they all come together here to create a fully-functional application.

**Learning Path:**

1. Follow Steps 1-13 in order to learn each concept
2. Each step builds on the previous one
3. Refer to this demo to see how everything integrates
4. The demo includes comments linking sections to tutorial steps

**Key Takeaways:**

- Real-world apps combine ALL these concepts
- Services promote code reusability and testability
- LINQ makes data manipulation elegant and readable
- Validation keeps business rules consistent
- EF Core handles database complexity
- Tailwind CSS enables rapid UI development
