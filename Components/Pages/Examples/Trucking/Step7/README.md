# Step 7: EF Core Models

## Overview

This step introduces Entity Framework Core models and data annotations. Students learn how to define entity classes that map to database tables and use data annotations for validation.

## Files in This Folder

- `Example.razor` - Tutorial page (route: `/trucking-examples/step7`)
- `Example.razor.cs` - Code-behind with sample model and validation demo
- `README.md` - This file

## Routes

- **Tutorial Page**: `/trucking-examples/step7`

## What Students Learn

1. Creating POCO (Plain Old CLR Object) models
2. Using Data Annotations for validation ([Required], [StringLength], [Range])
3. Navigation properties for entity relationships
4. Model best practices and conventions
5. Interactive validation with EditForm

## Key Concepts

- **POCO Models** - Simple C# classes representing data
- **Data Annotations** - Attributes for validation and metadata
- **Navigation Properties** - Relationships between entities
- **EditForm** - Blazor's form component with validation

## Architecture

- **Tutorial Page**: Uses `ExampleBase` from code-behind
- **Live Demo**: Interactive form showing data annotation validation
- **Sample Model**: Driver class with validation attributes

## Prerequisites

- Step 6: Conditional Rendering & Styling (dynamic UI based on state)

## Next Steps

- Step 8: Setup EF Core & DbContext (connecting to database)

## Code Structure

### Sample Model with Data Annotations

```csharp
public class SampleDriver
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(20)]
    public string LicenseNumber { get; set; } = "";

    [Range(18, 70, ErrorMessage = "Age must be between 18 and 70")]
    public int Age { get; set; }

    [Range(0, 50, ErrorMessage = "Experience must be between 0 and 50 years")]
    public int YearsOfExperience { get; set; }
}
```

### Live Demo Form

```csharp
private SampleDriver sampleDriver = new();
private bool showSuccess = false;

private void HandleSubmit()
{
    showSuccess = true;
}
```

## Styling Patterns

- Form validation messages in red
- Success message in green
- Clean form layout with labels and inputs
- Validation summary showing all errors

## Common Issues & Solutions

### Validation Not Triggering

**Issue**: Form submits even with invalid data  
**Solution**: Ensure `<DataAnnotationsValidator />` is included in the EditForm and you're using `OnValidSubmit` (not `OnSubmit`).

### Error Messages Not Showing

**Issue**: Validation errors don't appear  
**Solution**: Add `<ValidationMessage For="@(() => model.PropertyName)" />` for each field you want to validate.

### Navigation Properties Null

**Issue**: Related entities are null when querying  
**Solution**: Use `.Include()` in EF Core queries to eagerly load navigation properties (covered in Step 9).

## Related Resources

- [EF Core Models Documentation](https://learn.microsoft.com/en-us/ef/core/modeling/)
- [Data Annotations](https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties)
- [Blazor Forms & Validation](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms-and-validation)
