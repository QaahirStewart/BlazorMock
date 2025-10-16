using BlazorMock.Services;

namespace BlazorMock.Services;

public record Phase(string Id, string Title, string Subtitle, string Description, int[] StepNumbers);

public interface ILearningGuideService
{
    IReadOnlyList<Phase> GetPhases();
    Phase? GetPhaseById(string id);
}

public class LearningGuideService : ILearningGuideService
{
    private static readonly List<Phase> Phases = new()
    {
        new Phase(
            Id: "phase-1",
            Title: "Phase 1: Entry-Level",
            Subtitle: "No Database",
            Description: "Blazor fundamentals: environment setup, components, binding, forms, validation, and routing.",
            StepNumbers: new[] { 0, 1, 2, 3, 4, 5, 6 }
        ),
        new Phase(
            Id: "phase-2",
            Title: "Phase 2: Data & EF Core",
            Subtitle: "Persisting Data",
            Description: "Modeling domain entities, configuring DbContext, and CRUD with Entity Framework Core.",
            StepNumbers: new[] { 7, 8, 9 }
        ),
        new Phase(
            Id: "phase-3",
            Title: "Phase 3: State & Business Logic",
            Subtitle: "App State and Operations",
            Description: "State management, assignment logic, pay calculations, dashboards, and reports.",
            StepNumbers: new[] { 10, 11, 12, 13 }
        )
    };

    public IReadOnlyList<Phase> GetPhases() => Phases;

    public Phase? GetPhaseById(string id)
        => Phases.FirstOrDefault(p => string.Equals(p.Id, id, StringComparison.OrdinalIgnoreCase));
}
