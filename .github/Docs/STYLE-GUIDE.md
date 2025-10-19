# BlazorMock Style Guide

This guide ensures consistency across documentation, code, and UI components in the BlazorMock learning project.

---

## 📝 Documentation Style

### Markdown Formatting

**Headers:**

```markdown
# H1 - Main Document Title (only one per file)

## H2 - Major Sections

### H3 - Subsections

#### H4 - Minor Subsections (use sparingly)
```

**Emphasis:**

- Use **bold** for emphasis and important terms
- Use `code` for file names, code snippets, and technical terms
- Use _italics_ sparingly for subtle emphasis

**Lists:**

- Use `-` for unordered lists (not `*` or `+`)
- Use `1.`, `2.`, etc. for ordered lists
- Indent nested lists with 2 spaces

**Code Blocks:**

````markdown
```bash
# Shell commands
```
````

```csharp
// C# code
```

```razor
@* Razor/Blazor components *@
```

````

**Example Code Formatting Preference:**
- Always include Tailwind CSS classes in Razor code examples
- Show realistic, styled markup rather than minimal HTML
- Use semantic class names that demonstrate responsive design patterns
- Include container layouts (`min-h-screen`, `max-w-5xl mx-auto px-4`)
- Show component structure with proper spacing and visual hierarchy

**Links:**
- Use relative paths for internal docs: `[Step 02](./Step02.md)`
- Use absolute URLs for external resources: `[Blazor Docs](https://learn.microsoft.com/en-us/aspnet/core/blazor/)`

**Emojis:**
- ✅ Use sparingly for visual markers
- 📄 Files/Documents
- 🧠 Concepts/Learning
- 🛠️ Tools/Implementation
- 🧪 Testing/Prompts
- 📖 Tutorials
- 💡 Tips/Pro Tips
- 🎯 Goals/Achievements
- 🚀 Getting Started/Launch
- 📊 Progress/Stats
- ⚡ Important Notes
- 🎨 Design/Styling

### Step Documentation Format

Each step file (`Docs/Steps/StepXX.md`) should follow this template:

```markdown
# Step XX — Step Title

- Live example: /examples/stepX
- Next step: [Step XX — Next Title](./StepXX.md)
- Previous: [Step XX — Previous Title](./StepXX.md)

What you'll do:
- Key task 1
- Key task 2
- Key task 3

Commands:
- command examples if applicable

Verify:
- How to confirm the step worked
````

### Guide Document Format

Comprehensive guides should include:

1. **Title and Description** - What the guide covers
2. **Table of Contents** - For longer guides (optional)
3. **Phase/Section Headers** - Clearly marked phases
4. **Step Entries** - Each step with:
   - ✅ Goal
   - 📄 Files involved
   - 🧠 Concepts
   - 🛠️ Implementation details
   - 🧪 Suggested prompt (for AI assistance)
   - 📖 Link to tutorial
5. **Code Examples** - Properly formatted
6. **Next Steps** - What to do after completion

---

## 💻 Code Style

### C# Conventions

**Naming:**

- `PascalCase` for classes, methods, properties, public fields
- `camelCase` for local variables, parameters, private fields
- `_camelCase` with underscore for private instance fields (optional)
- `SCREAMING_SNAKE_CASE` for constants (use sparingly)

**File Organization:**

```csharp
// 1. Usings (grouped and sorted)
using System;
using Microsoft.AspNetCore.Components;

// 2. Namespace
namespace BlazorMock.Services;

// 3. Class/Interface/Enum
public class ExampleService
{
    // 4. Fields
    private readonly ILogger<ExampleService> _logger;

    // 5. Constructor
    public ExampleService(ILogger<ExampleService> logger)
    {
        _logger = logger;
    }

    // 6. Properties
    public string PropertyName { get; set; }

    // 7. Methods
    public async Task<Result> DoSomethingAsync()
    {
        // Implementation
    }
}
```

**Comments:**

- Use `//` for single-line comments
- Use `/* */` for multi-line comments
- Use `///` for XML documentation on public APIs
- Avoid obvious comments; code should be self-documenting

### Razor Component Conventions

**File Structure:**

```razor
@page "/route"
@inject ServiceName Service
@rendermode InteractiveServer

<PageTitle>Page Title</PageTitle>

<!-- Markup -->
<div class="container">
    <!-- Content -->
</div>

@code {
    // Fields
    private bool isLoading;

    // Parameters
    [Parameter]
    public string ParameterName { get; set; } = string.Empty;

    // Lifecycle methods
    protected override async Task OnInitializedAsync()
    {
        // Initialization
    }

    // Event handlers
    private async Task HandleClick()
    {
        // Handler logic
    }
}
```

**Naming:**

- Components: `PascalCase.razor` (e.g., `GreetingCard.razor`)
- Pages: `PascalCase.razor` (e.g., `Home.razor`, `DriverForm.razor`)
- Layouts: `PascalCase.razor` (e.g., `MainLayout.razor`)

---

## 🎨 UI & Styling

### Tailwind CSS Conventions

**Class Order:**

1. Layout (display, position, flex, grid)
2. Sizing (width, height, padding, margin)
3. Typography (font, text, line-height)
4. Visual (background, border, shadow)
5. Interactive (hover, focus, active)
6. Responsive (sm:, md:, lg:, xl:)

**Example:**

```html
<div
  class="flex flex-col gap-4 p-6 bg-white border border-gray-200 rounded-lg shadow-sm hover:shadow-md sm:flex-row sm:p-8"
>
  <h2 class="text-xl font-semibold text-gray-900 sm:text-2xl">Title</h2>
</div>
```

**Responsive Design:**

- Mobile-first approach (base styles are for mobile)
- Use breakpoint prefixes: `sm:`, `md:`, `lg:`, `xl:`, `2xl:`
- Standard breakpoints:
  - sm: 640px
  - md: 768px
  - lg: 1024px
  - xl: 1280px

**Typography Scale:**

```
H1: text-3xl sm:text-4xl md:text-5xl
H2: text-xl sm:text-2xl
H3: text-lg sm:text-xl
Body: text-sm sm:text-base md:text-lg
Small: text-xs sm:text-sm
```

**Text Wrapping:**

- Always use `break-words` for text that might overflow
- Use `min-w-0` in flex containers to allow text wrapping
- Avoid fixed widths that prevent responsive behavior

**Color Palette:**

- Primary: Blue (`blue-600`, `blue-700`)
- Success: Green (`green-600`, `green-700`)
- Warning: Yellow (`yellow-600`, `yellow-700`)
- Error: Red (`red-600`, `red-700`)
- Neutral: Gray (`gray-50` through `gray-900`)
- Accent: Orange (`orange-500`)

### Component Patterns

**Card:**

```html
<div class="bg-white rounded-xl p-5 sm:p-6 border border-gray-200">
  <!-- Content -->
</div>
```

**Button (Primary):**

```html
<button
  class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg font-medium transition-colors"
>
  Click Me
</button>
```

**Button (Secondary):**

```html
<button
  class="px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
>
  Click Me
</button>
```

**Input:**

```html
<input
  type="text"
  class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
/>
```

**Badge:**

```html
<span
  class="px-3 py-1 rounded-full bg-green-100 text-green-700 text-xs font-medium"
>
  Complete
</span>
```

**Responsive CRUD List Pattern:**

This pattern provides optimal UX across all device sizes with mobile cards and desktop tables.

Mobile Layout (< 768px) - Card view with `md:hidden`:

```html
<div class="space-y-3 md:hidden">
  @foreach (var item in items) {
  <div class="rounded-2xl border border-gray-200 bg-white p-4">
    <!-- Display mode -->
    <div class="mb-3">
      <div class="text-base font-semibold text-gray-900">@item.Name</div>
      <div class="text-sm text-gray-600">Detail: @item.Detail</div>
    </div>
    <!-- Actions -->
    <div class="flex flex-wrap gap-2">
      <button
        class="px-3 py-2 rounded-lg border hover:bg-gray-50 text-sm"
        @onclick="() => BeginEdit(item)"
      >
        Edit
      </button>
      <button
        class="px-3 py-2 rounded-lg border hover:bg-gray-50 text-sm"
        @onclick="() => DeleteAsync(item.Id)"
      >
        Delete
      </button>
    </div>
  </div>
  }
</div>
```

Desktop Layout (≥ 768px) - Table view with `hidden md:block`:

```html
<div class="overflow-x-auto hidden md:block">
  <table class="min-w-full border border-gray-200 rounded-xl overflow-hidden">
    <thead class="bg-gray-50">
      <tr>
        <th class="text-left p-2 border-b">Name</th>
        <th class="text-left p-2 border-b">Detail</th>
        <th class="text-left p-2 border-b">Actions</th>
      </tr>
    </thead>
    <tbody>
      @foreach (var item in items) {
      <tr class="hover:bg-gray-50">
        <td class="p-2 border-b">@item.Name</td>
        <td class="p-2 border-b">@item.Detail</td>
        <td class="p-2 border-b">
          <div class="flex gap-2">
            <button @onclick="() => BeginEdit(item)">Edit</button>
            <button @onclick="() => DeleteAsync(item.Id)">Delete</button>
          </div>
        </td>
      </tr>
      }
    </tbody>
  </table>
</div>
```

Key Benefits:

- Native mobile UX with touch-friendly cards
- Efficient desktop UX with scannable tables
- Single codebase serving both layouts
- Breakpoint at md (768px) aligns with Tailwind conventions
- Both layouts support inline editing for feature parity

See `Components/Pages/Drivers/DriverList.razor`, `TruckList.razor`, and `RouteList.razor` for complete examples.

---

## � In‑App Code Sample Pattern (Show code + Copy)

Use this pattern to present example code inside pages (e.g., example steps) with a consistent, learner-friendly UX that mirrors Step 13 and Step 4.

Policy:

- Per-block controls only. We do not show page-level "Expand/Collapse all" toolbars. This global toolbar was intentionally removed.
- Each code block renders its own collapsible "Show code" control and a Copy button.

How to use:

1. Provide a white card context (optional but recommended) with a heading, 1–2 sentences, and bullets.
2. Inside the card, place the code block within a gray container for visual grouping.
3. Use a raw <pre> element; the site JS enhancer will add the collapsible header and Copy button automatically.
4. Set a descriptive title with the `data-code-title` attribute.

Markup skeleton:

```html
<div class="bg-white rounded-xl p-5 sm:p-6 border border-gray-200">
  <h3 class="text-lg sm:text-xl font-semibold text-gray-900 mb-2">
    🧩 Code Example: Title
  </h3>
  <p class="text-sm sm:text-base text-gray-600 mb-3">
    Short description of what this code shows.
  </p>
  <ul class="list-disc pl-5 text-sm sm:text-base text-gray-700 space-y-1 mb-4">
    <li>Bullet point one</li>
    <li>Bullet point two</li>
  </ul>
  <div class="bg-gray-100 rounded-lg p-4 overflow-x-auto break-all">
    <pre data-code-title="Razor + C# (Component.razor)">
@* Paste code here as plain text. See note on escaping @ below. *@
    </pre>
  </div>
  <p class="text-xs text-gray-500 mt-3">
    Tip: Use Show code to expand/collapse and Copy to clipboard.
  </p>
  <p class="text-xs text-gray-500">
    No page-level toolbar is used; controls are per code block.
  </p>
  <p class="text-xs text-gray-500">
    Examples: Step 13 and Step 4 follow this pattern.
  </p>
  <!-- Notes: The JS enhancer auto-initializes; no extra wiring needed here. -->
  <!-- Policy: Page-level expand/collapse toolbars are intentionally not rendered globally. -->
</div>
```

Notes for Razor escape sequences:

- In Razor files, the `@` symbol starts code. When you want to display `@` literally inside static markup (including inside `<pre>`), escape it as `&#64;`.
- Alternatively, duplicate the symbol (`@@`) to render a single `@`, but `&#64;` is preferred for clarity in static examples.

Behavior details:

- The site-wide JS module enhances any `<pre>` with `data-code-title` by adding a collapsible summary ("Show code") and a per-block Copy button.
- The gray container (`bg-gray-100 rounded-lg p-4 overflow-x-auto break-all`) ensures readable, scrollable code on all screens.
- If a page ever needs bulk expand/collapse behavior, implement it locally within that page; do not reintroduce global toolbars.

---

## �📁 File Organization

### Project Structure

```
BlazorMock/
├── Components/
│   ├── _Imports.razor          # Global imports
│   ├── App.razor               # Root component
│   ├── Routes.razor            # Routing config
│   ├── Layout/                 # Layout components
│   │   ├── MainLayout.razor
│   │   └── ...
│   └── Pages/                  # Page components
│       ├── Home.razor
│       ├── Examples/           # Example/tutorial pages
│       │   ├── Step1Example.razor
│       │   └── ...
│       └── ...
├── Models/                     # Data models
│   ├── Driver.cs
│   ├── Truck.cs
│   └── Route.cs
├── Services/                   # Business logic services
│   ├── LearningProgressService.cs
│   └── TipsService.cs
├── Styles/                     # CSS source files
│   └── input.css
├── wwwroot/                    # Static files
│   ├── tailwind.css            # Compiled CSS
│   └── app.css
├── Docs/                       # Documentation
│   ├── README.md               # Docs hub
│   ├── Steps/                  # Step-by-step guides
│   │   ├── Step01.md
│   │   └── ...
│   ├── BlazorLearningGuide.md
│   ├── STYLE-GUIDE.md          # This file
│   └── ...
├── Program.cs                  # App entry point
├── appsettings.json            # Configuration
└── BlazorMock.csproj           # Project file
```

### Naming Conventions

**Files:**

- Components: `PascalCase.razor`
- Services: `PascalCase.cs`
- Models: `PascalCase.cs`
- Docs: `SCREAMING-KEBAB-CASE.md` for meta docs, `PascalCase.md` for guides

**Folders:**

- `PascalCase` for code folders
- `lowercase` for static asset folders

---

## ✍️ Writing Style

### Documentation Tone

- **Clear and Concise** - Get to the point quickly
- **Friendly** - Use conversational language
- **Educational** - Explain the "why" not just the "how"
- **Encouraging** - Use positive language
- **Inclusive** - Use "you" and "we" appropriately

**Good:**

> "You'll create a clean Blazor Server project with Tailwind CSS. This gives you a solid foundation without extra sample code."

**Avoid:**

> "The user should execute the dotnet CLI command to instantiate a new project artifact."

### Technical Writing

**Be Specific:**

- ✅ "Click the blue 'Mark as Complete' button"
- ❌ "Click the button"

**Use Active Voice:**

- ✅ "Run the command to build the project"
- ❌ "The command should be run to build the project"

**Provide Context:**

- ✅ "Install Tailwind CSS v4, which simplifies the setup process compared to v3"
- ❌ "Install Tailwind CSS"

**Include Examples:**

- Always show code examples for technical concepts
- Use realistic examples from the trucking app domain
- Format examples properly with syntax highlighting

---

## 🔍 Code Review Checklist

### Before Committing

**Code:**

- [ ] Follows naming conventions
- [ ] Properly formatted (consistent indentation)
- [ ] No unused imports or variables
- [ ] Comments added for complex logic
- [ ] Error handling in place
- [ ] Async methods named with `Async` suffix
- [ ] Parameters validated where needed

**Razor Components:**

- [ ] Responsive design (mobile-first)
- [ ] Proper text wrapping (`break-words`)
- [ ] Accessibility attributes added
- [ ] Event handlers follow naming pattern
- [ ] `@rendermode` specified if needed
- [ ] `PageTitle` set for pages

**Documentation:**

- [ ] Headers properly nested
- [ ] Links tested and working
- [ ] Code examples formatted
- [ ] Spelling and grammar checked
- [ ] Emojis used consistently
- [ ] Cross-references updated

**Styling:**

- [ ] Tailwind classes in recommended order
- [ ] Responsive breakpoints used
- [ ] Typography scale followed
- [ ] Color palette consistent
- [ ] Hover/focus states defined
- [ ] Transitions smooth (0.15s-0.3s)

---

## 🎯 Best Practices

### Component Design

1. **Single Responsibility** - Each component does one thing well
2. **Reusability** - Design for reuse, but don't over-engineer
3. **Props Over State** - Pass data down, events up
4. **Composition** - Build complex UIs from simple components
5. **Performance** - Use `@key` for lists, avoid unnecessary re-renders

### Service Design

1. **Interface-Based** - Define interfaces for services
2. **Dependency Injection** - Register services properly
3. **Async by Default** - Use async/await for I/O operations
4. **Error Handling** - Handle and log errors gracefully
5. **Testability** - Design with unit testing in mind

### Documentation

1. **Keep It Updated** - Update docs when code changes
2. **Code Examples** - Show, don't just tell
3. **Progressive Disclosure** - Start simple, add complexity
4. **Cross-Reference** - Link related concepts
5. **Version Info** - Note when features require specific versions

---

## 🚀 Quick Reference

### Common Patterns

**Inject Service:**

```razor
@inject IServiceName ServiceName
```

**Parameter:**

```csharp
[Parameter]
public string PropertyName { get; set; } = string.Empty;
```

**Event Handler:**

```csharp
private async Task HandleEventAsync()
{
    // Implementation
}
```

**Conditional Rendering:**

```razor
@if (condition)
{
    <div>Show this</div>
}
else
{
    <div>Show that</div>
}
```

**Loop:**

```razor
@foreach (var item in items)
{
    <div @key="item.Id">@item.Name</div>
}
```

### Common Tailwind Combinations

**Container:**

```html
class="w-full mx-auto px-4 max-w-5xl"
```

**Flex Center:**

```html
class="flex items-center justify-center"
```

**Flex Column with Gap:**

```html
class="flex flex-col gap-4"
```

**Responsive Grid:**

```html
class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4"
```

**Card Shadow:**

```html
class="shadow-sm hover:shadow-md transition-shadow"
```

---

## 📚 Resources

### Internal

- [Typography System](./Typography-System.md) - Detailed typography guidelines
- [Documentation Structure](./DOCUMENTATION-STRUCTURE.md) - How docs are organized
- [Learning Guide](./BlazorLearningGuide.md) - Complete learning path

### External

- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [Tailwind CSS Docs](https://tailwindcss.com/docs)
- [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Markdown Guide](https://www.markdownguide.org/)

---

**Last Updated:** October 16, 2025  
**Maintainer:** BlazorMock Project Team

---

**Note:** This is a living document. As the project evolves, update this guide to reflect new patterns and conventions.
