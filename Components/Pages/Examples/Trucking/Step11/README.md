# Step 11: Assignment Logic & Business Rules

## Overview

This step teaches students how to implement business rules and validation logic for driver-truck-route assignments. Students learn to validate license requirements, experience levels, and availability status.

## Files in This Folder

- `Example.razor` - Tutorial page (route: `/trucking-examples/step11`)
- `Example.razor.cs` - Code-behind with validation demo logic
- `README.md` - This file

## Routes

- **Tutorial Page**: `/trucking-examples/step11`

## What Students Learn

1. Implementing business validation rules
2. Checking driver license levels match truck requirements
3. Validating experience for route types (Hazmat, Oversized, etc.)
4. Using `@bind-Value:after` for instant validation feedback
5. Displaying user-friendly error messages

## Key Concepts

- **Business Validation** - Enforcing domain rules beyond simple data validation
- **@bind-Value:after** - Trigger methods immediately after binding updates
- **Switch Expressions** - Pattern matching for multi-condition logic
- **Validation Warnings** - Separating hard errors from soft warnings

## Architecture

- **Validation Method**: `ValidateAssignment()` checks all business rules
- **Demo State**: In-memory driver/truck/route selection
- **Rule Types**: Hard rules (block submission) vs. soft warnings
- **User Feedback**: Real-time validation messages as user selects options

## Prerequisites

- Step 10: State Management (understanding of component state)

## Next Steps

- Step 12: Pay & Expense Calculation (business logic in services)

## Code Structure

### Validation Logic

```csharp
private void ValidateAssignment()
{
    validationWarnings.Clear();
    isValid = true;

    // Rule 1: License level check
    if (!truck.CanBeOperatedBy(driver))
    {
        validationWarnings.Add("❌ License level too low for this truck");
        isValid = false;
    }

    // Rule 2: Experience check
    bool hasExperience = routeType switch
    {
        RouteType.Hazmat => driver.YearsOfExperience >= 2,
        RouteType.Oversized => driver.YearsOfExperience >= 3,
        RouteType.LongHaul => driver.YearsOfExperience >= 1,
        _ => true
    };

    if (!hasExperience)
    {
        validationWarnings.Add("⚠️ Insufficient experience for this route");
        isValid = false;
    }
}
```

### Instant Validation Binding

```razor
<InputSelect @bind-Value="newRoute.DriverId" @bind-Value:after="ValidateAssignment">
    ...
</InputSelect>
```

## Business Rules Summary

### Hard Rules (Must Pass)

- Driver license level must match or exceed truck requirements
- Driver must have required experience for route type
- Driver must be available (not on another route)
- Truck must be available (not in use or maintenance)

### Soft Warnings (Informational)

- Truck needs maintenance soon
- Driver approaching maximum hours
- Long distance route
- Weather conditions

## Live Demo Features

- Interactive assignment validator
- Three dropdowns: Route Type, Truck, Driver
- Real-time validation warnings
- Visual indicators (✅ valid, ❌ invalid)
- Reset button to clear selections

## Styling Patterns

- Hard rules displayed in red cards
- Soft warnings in yellow cards
- Validation messages with emoji icons
- Disabled submit button when invalid
- Success message when all rules pass

## Common Issues & Solutions

### Validation Doesn't Trigger

**Issue**: Selecting options doesn't run validation  
**Solution**: Use `@bind-Value:after="ValidateAssignment"` on InputSelect components.

### Submit Button Always Disabled

**Issue**: Button stays disabled even when valid  
**Solution**: Check that `isValid` is set to `true` when all rules pass and no warnings exist.

### Warnings Don't Clear

**Issue**: Old warnings stay visible after fixing issues  
**Solution**: Call `validationWarnings.Clear()` at the start of `ValidateAssignment()`.

### Business Logic in Wrong Place

**Issue**: Validation logic mixed with UI code  
**Solution**: Move complex business rules to model methods (like `CanBeOperatedBy`) or dedicated validation services.

## Related Resources

- [Business Logic Patterns](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-model-layer-validations)
- [Switch Expressions](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/switch-expression)
- [Blazor Form Validation](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms-and-validation)
