using BlazorMock.Services;

namespace BlazorMock.Services;

public record Phase(string Id, string Title, string Subtitle, string Description, int[] StepNumbers, string ProjectKey);

public interface ILearningGuideService
{
    // Existing trucking-only APIs
    IReadOnlyList<Phase> GetPhases();
    Phase? GetPhaseById(string id);

    // Project-aware APIs
    IReadOnlyList<Phase> GetPhases(string projectKey);
    Phase? GetPhaseById(string projectKey, string id);
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
            StepNumbers: new[] { 0, 1, 2, 3, 4, 5, 6 },
            ProjectKey: "trucking"
        ),
        new Phase(
            Id: "phase-2",
            Title: "Phase 2: Data & EF Core",
            Subtitle: "Persisting Data",
            Description: "Modeling domain entities, configuring DbContext, and CRUD with Entity Framework Core.",
            StepNumbers: new[] { 7, 8, 9 },
            ProjectKey: "trucking"
        ),
        new Phase(
            Id: "phase-3",
            Title: "Phase 3: State & Business Logic",
            Subtitle: "App State and Operations",
            Description: "State management, assignment logic, pay calculations, dashboards, and reports.",
            StepNumbers: new[] { 10, 11, 12, 13 },
            ProjectKey: "trucking"
        ),

        // Pokemon Collector phases (projectKey = "pokemon")
        new Phase(
            Id: "pokemon-phase-1",
            Title: "Phase 1: Setup & API Connection",
            Subtitle: "Foundation",
            Description: "Set up your environment, create a clean project, and connect to the Pokemon API.",
            StepNumbers: new[] { 0, 1, 2 },
            ProjectKey: "pokemon"
        ),
        new Phase(
            Id: "pokemon-phase-2",
            Title: "Phase 2: Lists, Paging, Loading",
            Subtitle: "Make browsing feel good",
            Description: "Add paging, loading indicators, and error states while browsing Pokemon.",
            StepNumbers: new[] { 3, 4, 5 },
            ProjectKey: "pokemon"
        ),
        new Phase(
            Id: "pokemon-phase-3",
            Title: "Phase 3: Details & Search",
            Subtitle: "Polish the experience",
            Description: "Build detail views, search, and filtering for your Pokemon collector.",
            StepNumbers: new[] { 6 },
            ProjectKey: "pokemon"
        ),

        // Admin SaaS Dashboard phases (projectKey = "admin")
        new Phase(
            Id: "admin-phase-1",
            Title: "Phase 1: Layout & Tiles",
            Subtitle: "Project setup + shell pieces",
            Description: "Environment setup, create a new clean project, then build the layout, reusable stat cards, and status badges.",
            StepNumbers: new[] { 0, 1, 2, 3 },
            ProjectKey: "admin"
        ),
        new Phase(
            Id: "admin-phase-2",
            Title: "Phase 2: Roles & Locking",
            Subtitle: "Auth, roles, locked UI",
            Description: "Add a mock auth service, role toggle, lock patterns, and access explanation banners.",
            StepNumbers: new[] { 4, 5, 6, 7 },
            ProjectKey: "admin"
        ),
        new Phase(
            Id: "admin-phase-3",
            Title: "Phase 3: Panels & Final Dashboard",
            Subtitle: "Subscriptions, billing, profile",
            Description: "Build subscription, billing, and profile panels and compose them into the final /admin/dashboard experience.",
            StepNumbers: new[] { 8, 9, 10 },
            ProjectKey: "admin"
        )
    };

    // Trucking-only convenience methods
    public IReadOnlyList<Phase> GetPhases() => GetPhases("trucking");

    public Phase? GetPhaseById(string id) => GetPhaseById("trucking", id);

    // Project-aware implementations
    public IReadOnlyList<Phase> GetPhases(string projectKey)
        => Phases.Where(p => string.Equals(p.ProjectKey, projectKey, StringComparison.OrdinalIgnoreCase)).ToList();

    public Phase? GetPhaseById(string projectKey, string id)
        => Phases.FirstOrDefault(p => string.Equals(p.ProjectKey, projectKey, StringComparison.OrdinalIgnoreCase)
                                       && string.Equals(p.Id, id, StringComparison.OrdinalIgnoreCase));
}
