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
    { // Trucking Schedule phases (projectKey = "trucking")
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

        // Admin Dashboard phases (projectKey = "admin-dashboard")
        new Phase(
            Id: "admin-dashboard-phase-1",
            Title: "Phase 1: Project & Auth Basics",
            Subtitle: "Clean project + sign in",
            Description: "Create a clean Blazor project, set up EF Core for users, and add basic sign-in/sign-up pages.",
            StepNumbers: new[] { 1, 2, 3, 4 },
            ProjectKey: "admin-dashboard"
        ),
        new Phase(
            Id: "admin-dashboard-phase-2",
            Title: "Phase 2: Profile, Analytics & Layout",
            Subtitle: "User insights and dashboard shell",
            Description: "Implement the profile page, simple analytics, and the main admin dashboard layout.",
            StepNumbers: new[] { 5, 6, 7, 8, 9 },
            ProjectKey: "admin-dashboard"
        ),
        new Phase(
            Id: "admin-dashboard-phase-3",
            Title: "Phase 3: Route Protection & Navigation",
            Subtitle: "Lock things down",
            Description: "Protect routes, wire in desktop and mobile navigation, and polish the experience.",
            StepNumbers: new[] { 10, 11, 12 },
            ProjectKey: "admin-dashboard"
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
