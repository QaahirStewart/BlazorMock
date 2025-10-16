# 🚚 Blazor Learning Guide — Trucking Schedule App (Entry → Advanced)

This guide is designed for use with a local LLM assistant in VS Code (e.g., GPT-5). Each step includes:

- ✅ Learning goal
- 📄 Page to create
- 🧠 Concepts to understand
- 🛠️ Code to implement
- 🧪 Suggested prompt for your assistant

**✨ New:** Responsive typography system with consistent font sizing across all breakpoints! See `Docs/Typography-System.md` for details.

---

## 🧰 Before You Start

If you're starting from a blank machine, complete [Step 00 — Prerequisites & VS Code Setup](./Steps/Step00.md). It covers installing the .NET SDK, Node.js, and helpful VS Code extensions, plus verifying your environment.

---

## 🔰 Phase 1: Entry-Level (No Database)

### Step 1: New Clean Project

✅ Create a clean Blazor Server project with Tailwind CSS v4  
📄 Setup project structure  
🧠 dotnet CLI, npm packages, Tailwind v4, Asset pipeline  
🛠️ Commands and configuration  
🧪 Prompt:

> "Create a new clean Blazor Server project with interactive mode and set up Tailwind CSS v4."

**📖 View Tutorial:** Visit `/examples/step1` to see the full tutorial with code examples and mark this step complete!

**Commands:**

```bash
# Create new Blazor project
dotnet new blazor -o BlazorAppName --interactivity Server --all-interactive --empty

# Install Tailwind v4
npm install tailwindcss @tailwindcss/cli

# Create Styles folder and input.css with: @import "tailwindcss";

# Build Tailwind CSS (keep running with --watch)
npx @tailwindcss/cli -i ./Styles/input.css -o ./wwwroot/tailwind.css --watch

# Add to App.razor <head> section:
# <link rel="stylesheet" href="@Assets["tailwind.css"]" />
```

---

### Step 2: Razor Syntax & Display

✅ Learn Razor syntax and display dynamic content  
📄 Display date/time dynamically  
🧠 `@page` directive, Razor syntax, `DateTime`  
🛠️ Display current date and time  
🧪 Prompt:

> "Show me how to use Razor syntax to display the current date and time in a Blazor page."

**📖 View Tutorial:** Visit `/examples/step2` to see the full tutorial with code examples and mark this step complete!

---

### Step 3: Reusable Components

✅ Build reusable components  
📄 `GreetingCard.razor`  
🧠 `@code`, `[Parameter]`, component reuse  
🛠️ A card that takes `Name` and `Message` as parameters  
🧪 Prompt:

> "Create a Blazor component called `GreetingCard.razor` that accepts `Name` and `Message` as parameters and displays them in a styled card."

**📖 View Tutorial:** Visit `/examples/step3` to see the full tutorial with code examples and mark this step complete!

---

### Step 4: Event Binding

✅ Handle user interaction  
📄 `Counter.razor`  
🧠 `@onclick`, state updates  
🛠️ A button that increments a counter  
🧪 Prompt:

> "Create a `Counter.razor` page with a button that increments a number when clicked."

**📖 View Tutorial:** Visit `/examples/step4` to see the full tutorial with code examples and mark this step complete!

---

### Step 5: Forms & Validation

✅ Create a form with validation  
📄 `DriverForm.razor`  
🧠 `EditForm`, `DataAnnotationsValidator`, `ValidationSummary`, input components  
🛠️ Form to enter driver name, License Level (A/B/C concept), and years of experience  
🧪 Prompt:

> "Create a `DriverForm.razor` page with an `EditForm` that validates name (required), license level (A/B/C) via dropdown, and years of experience (range 0–60)."

**📖 View Tutorial:** Visit `/examples/step5` to see the full tutorial with code examples and mark this step complete!  
**💡 Tip:** Check `/tips` for "EditForm + Validation" and "Validation attributes" examples!

---

## 📋 How Progress Tracking Works

**✅ Marking Steps Complete:**

- Navigate to each step's example page (e.g., `/examples/step1`)
- Read through the tutorial and code examples
- Click "Mark as Complete" at the bottom when you're done
- Your progress is automatically saved

**📊 Viewing Progress:**

- Visit `/guide` to see all steps and your completion status
- Steps show as "Not Started" or "Complete"
- Complete 4+ steps to unlock "Phase 1 Complete!" badge
- Visit `/progress` for a detailed breakdown of all 13 steps

**🎯 Achievement:** Complete Steps 1-4 to unlock Phase 1!

---

<!-- Removed duplicate/misnumbered section: Step 5 Routing & Navigation (belongs to Step 6). -->

## 🧩 Phase 2: Intermediate (EF Core + State)

### Step 6: Routing & Navigation

✅ Link pages together and handle navigation  
📄 Pages: `Home.razor`, `DriverForm.razor`, etc.  
🧠 `@page`, `NavLink`, `NavigationManager`, route parameters  
🛠️ Add navigation bar and programmatic navigation  
🧪 Prompt:

> "Show me how to use NavLink for navigation and NavigationManager for programmatic routing in Blazor."

**📖 View Tutorial:** Visit `/examples/step6` to see the full tutorial with code examples and mark this step complete!

---

### Step 7: EF Core Models

✅ Define domain models for Entity Framework Core  
📄 `Models/Driver.cs`, `Models/Truck.cs`, `Models/Route.cs`  
🧠 POCOs, enums, data annotations, relationships  
🛠️ Create models with properties and enums  
🧪 Prompt:

> "Create EF Core models for `Driver`, `Truck`, and `Route` with enums for license level, truck class, and route type."

**📖 View Tutorial:** Visit `/examples/step7` to see the full tutorial with code examples and mark this step complete!

---

### Step 8: Setup EF Core & DbContext

✅ Add database support with Entity Framework Core  
📄 `Data/AppDbContext.cs`, `appsettings.json`  
🧠 `DbContext`, migrations, SQLite setup  
🛠️ Configure EF Core with SQLite  
🧪 Prompt:

> "Set up EF Core with SQLite using `AppDbContext.cs` and connection string in `appsettings.json`. Include `DbSet` for Driver, Truck, Route."

**📖 View Tutorial:** Visit `/examples/step8` to see the full tutorial with code examples and mark this step complete!

---

### Step 9: CRUD Operations & Related Data

✅ Build data-driven pages with full CRUD support  
📄 `DriverList.razor`, `TruckList.razor`, `RouteList.razor`  
🧠 CRUD operations, `Include()` for related data, async loading  
🛠️ List, add, edit, delete records with EF Core  
🧪 Prompt:

> "Create a `DriverList.razor` page that lists all drivers from the database and allows adding, editing, and deleting."

**📖 View Tutorial:** Visit `/examples/step9` to see the full tutorial with code examples and mark this step complete!

---

### Step 10: State Management

✅ Share data across components with services  
📄 `Services/AppState.cs`  
🧠 Scoped services, events, `StateHasChanged()`, `IDisposable`  
🛠️ Track selected driver or route across components  
🧪 Prompt:

> "Create a scoped `AppState` service to track selected driver and inject it into multiple components."

**📖 View Tutorial:** Visit `/examples/step10` to see the full tutorial with code examples and mark this step complete!

---

## 🚀 Phase 3: Advanced (Business Rules + Scheduling)

### Step 11: Assignment Logic & Business Rules

✅ Implement business rules and validation for assignments  
📄 `AssignmentForm.razor`  
🧠 Validation, rule enforcement, driver-truck matching  
🛠️ Prevent invalid assignments with business rules  
🧪 Prompt:

> "Create an `AssignmentForm.razor` that assigns a driver to a truck and route, enforcing license level and experience rules."

**📖 View Tutorial:** Visit `/examples/step11` to see the full tutorial with code examples and mark this step complete!

---

<!-- Removed duplicated Phase 2/3 sections with misnumbering for Steps 7–10. The correct versions above remain. -->

### Step 12: Pay & Expense Calculation

✅ Build a service for business calculations  
📄 `Services/ScheduleService.cs`  
🧠 Business logic services, calculation algorithms, decimal precision  
🛠️ Calculate driver pay and estimate route expenses  
🧪 Prompt:

> "Create a `ScheduleService.cs` that calculates driver pay, fuel costs, and profit margins for routes."

**📖 View Tutorial:** Visit `/examples/step12` to see the full tutorial with code examples and mark this step complete!

---

### Step 13: Dashboard & Reports

✅ Visualize data with a comprehensive dashboard  
📄 `Dashboard.razor`  
🧠 Data visualization, filtering, summaries, conditional styling, LINQ aggregation  
🛠️ Show KPIs, active routes, driver availability, and financials  
🧪 Prompt:

> “Create a `Dashboard.razor` page that shows upcoming routes, driver availability, and total payroll/expenses with color-coded statuses.”

**📖 View Tutorial:** Visit `/examples/step13` to see the full tutorial with code examples and mark this step complete!

Demo: Explore the finished Trucking Schedule experience at `/demo/trucking-schedule`—grouped schedule, filters, KPIs, and live status actions (Start/Complete).

---

> “Create a `Dashboard.razor` page that shows upcoming routes, driver availability, and total payroll/expenses with color-coded statuses.”

---

---

## 💡 Bonus Features

### 🎨 Responsive Typography System

✅ Professional, consistent typography across all pages  
📄 `Styles/input.css`, all component files  
🧠 Tailwind CSS v4 theming, responsive design, mobile-first approach  
🛠️ Responsive text scales, proper wrapping, no overflow issues

**Key Features:**

- **Consistent Font Scaling:** H1 (3xl→4xl→5xl), H2 (xl→2xl), Body (sm→base→lg)
- **Proper Text Wrapping:** All text uses `break-words` to prevent overflow
- **Mobile-First Design:** Scales smoothly from 320px to desktop
- **Global Configuration:** Centralized in `Styles/input.css` for easy maintenance
- **Responsive Components:** All pages, forms, and navigation adapted

**Documentation:** See `Docs/Typography-System.md` for complete guidelines and best practices!

### 💡 ELI5 Tips System

✅ Comprehensive tips system at `/tips`  
📄 `Tips.razor`, `TipsService.cs`  
🧠 Service architecture, DI, contributor pattern, accordion UI  
🛠️ 20+ topics covering Blazor and C# essentials

**Topics Covered:**

- **Blazor:** Event binding, Parameters, Forms, Routing, Navigation, Services, Lifecycle, JS Interop, Data binding
- **C#:** Classes, Records, LINQ, async/await, Nullable types, Pattern matching, Collections, String interpolation, Tuples, Local functions

**Features:**

- Category-based organization (Blazor/C#/Other)
- Dual-card layout: Summary + Deep Dive
- Code examples with proper wrapping
- ELI5 explanations with unique analogies
- Practical tips for each concept
- **Fully responsive** with mobile-optimized layout

**How to Use:** Navigate to `/tips`, select a category, expand topics to see examples and explanations!

---

## 🧠 How to Use This Guide

- Use each step as a prompt to your local AI assistant
- Reference the `/tips` page for quick examples and explanations
- Check `Docs/Typography-System.md` for responsive design guidelines
- Ask the assistant to explain concepts and walk you through the code
- Example:
  > "I'm following a Blazor learning guide. Step 4 is about forms and validation. Help me build `DriverForm.razor` with an `EditForm` that validates driver information."

**Pro Tips:**

- Visit `/tips` first to see working examples of the concepts you're about to implement!
- Review `Docs/Typography-System.md` for responsive typography best practices when styling new components

---
