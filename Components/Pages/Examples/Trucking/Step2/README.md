# Step 2: Razor Syntax & Display

## Overview

This folder contains the tutorial page for Step 2 of the Trucking Guide, teaching students how to use Razor syntax to combine HTML markup with C# code expressions.

## Files in This Folder

- **Example.razor**: Main tutorial page component at route `/trucking-examples/step2`
- **Example.razor.cs**: Code-behind class `ExampleBase` handling completion tracking and lifecycle
- **README.md**: This documentation file

## Routes

| Route                      | Component     | Purpose              |
| -------------------------- | ------------- | -------------------- |
| `/trucking-examples/step2` | Example.razor | Step 2 tutorial page |

## What Students Learn

### Core Concepts

1. **@page Directive**: How to assign URL routes to Blazor components
2. **Razor Syntax**: Using the `@` symbol to access C# expressions inline
3. **DateTime Formatting**: Applying `.ToString()` patterns to format dates and times
4. **Responsive Layouts**: Building adaptive UIs with Tailwind CSS grid utilities

### Skills Developed

- **Routing**: Understanding how `@page` maps components to URLs
- **Inline Expressions**: Evaluating C# expressions directly in markup
- **String Formatting**: Using format strings to customize date/time display
- **Server-Side Rendering**: Recognizing when values are computed (server vs. client)

## Key Concepts

### @page Directive

```razor
@page "/"
```

The `@page` directive must be the first line of a routable component. It tells Blazor what URL should render this component.

### Razor Syntax (@)

```razor
<p>Current Date: @DateTime.Now.ToString("MM/dd/yyyy")</p>
```

The `@` symbol tells Blazor to evaluate the following C# expression and render its result.

### DateTime Formatting

Common format strings:

- `"dddd, MMMM dd, yyyy"` → "Monday, January 15, 2024"
- `"hh:mm tt"` → "03:45 PM"
- `"yyyy-MM-dd"` → "2024-01-15" (ISO format)

## Architecture

### Component Structure

```
Example.razor (Markup)
  ├── @page "/trucking-examples/step2"
  ├── @inherits ExampleBase
  └── Markup with inline DateTime expressions

Example.razor.cs (Code-Behind)
  ├── ExampleBase : ComponentBase, IDisposable
  ├── ILearningProgressService (DI)
  ├── IJSRuntime (DI)
  ├── isComplete state
  ├── OnInitializedAsync() → Load completion status
  ├── OnAfterRenderAsync() → Initialize code block enhancer
  ├── MarkComplete() → Update progress
  ├── ResetStep() → Clear completion
  └── Dispose() → Clean up JS module
```

### State Management

- **isComplete**: Boolean tracking whether the user has marked this step complete
- **\_copyModule**: JS interop module for code block copy functionality

### Progress Tracking

Uses `ILearningProgressService` with domain-specific API:

- `GetStepAsync("trucking", 2)` → Retrieve completion status
- `MarkStepCompleteAsync("trucking", 2)` → Mark step complete
- `MarkStepIncompleteAsync("trucking", 2)` → Reset step

## Prerequisites

Before starting this step, students should have:

- ✅ Completed Step 1 (Project Setup)
- ✅ Basic understanding of HTML
- ✅ Familiarity with C# syntax (variables, methods, properties)

## Next Steps

After completing this step, students will:

1. Move to **Step 3: Reusable Components** to learn component composition
2. Understand how to create and use custom Blazor components
3. Learn about `@code` blocks and component parameters

## Styling Patterns

### Gradient Banners

```html
<div
  class="bg-gradient-to-r from-gray-100 to-blue-100 rounded-2xl p-5 sm:p-6 mb-6"
></div>
```

Used for "What You'll Learn" and "How to do it" sections.

### White Cards

```html
<div class="bg-white rounded-xl p-6 border border-gray-200 mb-6"></div>
```

Used for sample code, notes, and major content sections.

### Code Blocks

```html
<pre
  class="code-block text-xs sm:text-sm font-mono text-gray-800 w-full max-w-full overflow-x-auto"
></pre>
```

Enhanced with JavaScript for syntax highlighting and copy functionality.

### Responsive Typography

- Page Title: `text-4xl` (fixed, no responsive variants)
- Description: `text-xl` (fixed, no responsive variants)
- Section Headings: `text-xl sm:text-2xl` (responsive)

## Common Issues

### Issue: Page shows 404 Not Found

**Cause**: Route mismatch or missing `@page` directive

**Solution**: Verify `@page "/trucking-examples/step2"` is the first line of Example.razor

### Issue: @ symbol renders as literal text

**Cause**: Inside HTML comment or incorrect Razor context

**Solution**: Use `@` directly in C# expression context. To show literal `@`, use `@@`.

### Issue: Format string throws exception

**Cause**: Invalid format specifier or typo

**Solution**: Verify format strings are case-sensitive. Use Microsoft's [DateTime format strings documentation](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings).

### Issue: Time doesn't update in real-time

**Cause**: Blazor Server only renders on page load/refresh

**Solution**: For real-time updates, implement a timer with `StateHasChanged()` (covered in later steps).

## Teaching Notes

### Concepts to Emphasize

1. **@ vs. @@**: Single `@` evaluates C# expressions; double `@@` renders a literal `@` symbol
2. **Server Time**: `DateTime.Now` shows server time, not client browser time
3. **Route Mapping**: `@page` directive creates URL-to-component mapping
4. **Format Strings**: Custom patterns control how dates/times display

### Common Student Questions

**Q: Why does the time only update on refresh?**

A: Blazor renders on the server when the page loads. The time shown is a snapshot from that moment. Real-time updates require timers (introduced in Step 4).

**Q: Can I have multiple @page directives?**

A: Yes! A component can map to multiple routes:

```razor
@page "/home"
@page "/"
```

**Q: What's the difference between "MM" and "mm"?**

A: "MM" = month (01-12), "mm" = minutes (00-59). Format strings are case-sensitive.

## Related Resources

### Blazor Documentation

- [Routing and Navigation](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/routing)
- [Razor Syntax Reference](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor)

### DateTime Formatting

- [Custom Date and Time Format Strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- [Standard Date and Time Format Strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings)

### Tips Referenced

- **@page route** (slug: `page-route`) - Explains how @page directive maps components to URLs
