using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorMock.Services;

public class StepProgress
{
    public int StepNumber { get; set; }
    public string StepId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ProjectKey { get; set; } = "trucking";
    public bool IsComplete { get; set; }
    public DateTime? CompletedDate { get; set; }
}

public interface ILearningProgressService
{
    // Existing trucking-only APIs (default project)
    Task<List<StepProgress>> GetAllStepsAsync();
    Task<StepProgress?> GetStepAsync(int stepNumber);
    Task MarkStepCompleteAsync(int stepNumber);
    Task MarkStepIncompleteAsync(int stepNumber);
    Task<int> GetCompletedCountAsync();
    Task ResetAllProgressAsync();

    // Project-aware APIs
    Task<List<StepProgress>> GetAllStepsAsync(string projectKey);
    Task<StepProgress?> GetStepAsync(string projectKey, int stepNumber);
    Task MarkStepCompleteAsync(string projectKey, int stepNumber);
    Task MarkStepIncompleteAsync(string projectKey, int stepNumber);
    Task<int> GetCompletedCountAsync(string projectKey);
}

public class LearningProgressService : ILearningProgressService
{
    private const string StorageKey = "blazor_learning_progress";
    private readonly IJSRuntime _jsRuntime;
    private List<StepProgress>? _cachedSteps;

    public LearningProgressService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    private List<StepProgress> GetDefaultSteps()
    {
        return new List<StepProgress>
        {
            new() { StepNumber = 0, StepId = "step0", Title = "Prerequisites & VS Code Setup", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 1, StepId = "step1", Title = "New Clean Project", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 2, StepId = "step2", Title = "Razor Syntax & Display", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 3, StepId = "step3", Title = "Reusable Components", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 4, StepId = "step4", Title = "Event Binding", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 5, StepId = "step5", Title = "Forms & Validation", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 6, StepId = "step6", Title = "Routing & Navigation", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 7, StepId = "step7", Title = "EF Core Models", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 8, StepId = "step8", Title = "Setup EF Core & DbContext", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 9, StepId = "step9", Title = "CRUD Pages", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 10, StepId = "step10", Title = "State Management", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 11, StepId = "step11", Title = "Assignment Logic", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 12, StepId = "step12", Title = "Pay Calculation", ProjectKey = "trucking", IsComplete = false },
            new() { StepNumber = 13, StepId = "step13", Title = "Dashboard & Reports", ProjectKey = "trucking", IsComplete = false },

            // Pokemon Collector steps (projectKey = "pokemon")
            new() { StepNumber = 0, StepId = "pokemon-step0", Title = "Prerequisites & VS Code Setup", ProjectKey = "pokemon", IsComplete = false },
            new() { StepNumber = 1, StepId = "pokemon-step1", Title = "New Clean Project", ProjectKey = "pokemon", IsComplete = false },
            new() { StepNumber = 2, StepId = "pokemon-step2", Title = "Configure HttpClient for PokéAPI", ProjectKey = "pokemon", IsComplete = false },
            new() { StepNumber = 3, StepId = "pokemon-step3", Title = "Call PokéAPI and render a list", ProjectKey = "pokemon", IsComplete = false },
            new() { StepNumber = 4, StepId = "pokemon-step4", Title = "Add paging to the Pokémon list", ProjectKey = "pokemon", IsComplete = false },
            new() { StepNumber = 5, StepId = "pokemon-step5", Title = "Show loading and error states", ProjectKey = "pokemon", IsComplete = false },
            new() { StepNumber = 6, StepId = "pokemon-step6", Title = "Add search, filtering, and a details view", ProjectKey = "pokemon", IsComplete = false },

            // Admin SaaS Dashboard steps (projectKey = "admin")
            new() { StepNumber = 0, StepId = "admin-step0", Title = "Prerequisites & VS Code Setup", ProjectKey = "admin", IsComplete = false },
            new() { StepNumber = 1, StepId = "admin-step1", Title = "New Clean Project", ProjectKey = "admin", IsComplete = false },
            new() { StepNumber = 2, StepId = "admin-step2", Title = "Reusable Stat Card", ProjectKey = "admin", IsComplete = false },
            new() { StepNumber = 3, StepId = "admin-step3", Title = "Status Badges & Dashboard Shell", ProjectKey = "admin", IsComplete = false },
            new() { StepNumber = 4, StepId = "admin-step4", Title = "Auth Service & Admin Roles", ProjectKey = "admin", IsComplete = false },
            new() { StepNumber = 5, StepId = "admin-step5", Title = "Role Toggle & Profile Chip", ProjectKey = "admin", IsComplete = false },
            new() { StepNumber = 6, StepId = "admin-step6", Title = "Locked Tiles & Disabled Actions", ProjectKey = "admin", IsComplete = false },
            new() { StepNumber = 7, StepId = "admin-step7", Title = "Access Explanation Banner", ProjectKey = "admin", IsComplete = false },
            new() { StepNumber = 8, StepId = "admin-step8", Title = "Subscription Panel", ProjectKey = "admin", IsComplete = false },
            new() { StepNumber = 9, StepId = "admin-step9", Title = "Billing Summary Panel", ProjectKey = "admin", IsComplete = false },
            new() { StepNumber = 10, StepId = "admin-step10", Title = "Profile Settings Panel", ProjectKey = "admin", IsComplete = false }
        };
    }

    public async Task<List<StepProgress>> GetAllStepsAsync()
        => await GetAllStepsAsync("trucking");

    public async Task<List<StepProgress>> GetAllStepsAsync(string projectKey)
    {
        if (_cachedSteps != null)
            return _cachedSteps.Where(s => s.ProjectKey == projectKey).ToList();

        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", StorageKey);
            
            if (!string.IsNullOrEmpty(json))
            {
                _cachedSteps = JsonSerializer.Deserialize<List<StepProgress>>(json) ?? GetDefaultSteps();

                // Migration: ensure ProjectKey is set (assume existing data is trucking)
                foreach (var step in _cachedSteps)
                {
                    if (string.IsNullOrWhiteSpace(step.ProjectKey))
                    {
                        step.ProjectKey = "trucking";
                    }
                }
            }
            else
            {
                _cachedSteps = GetDefaultSteps();
                await SaveStepsAsync(_cachedSteps);
            }
        }
        catch
        {
            // If localStorage fails (SSR or first render), use defaults
            _cachedSteps = GetDefaultSteps();
        }

        return _cachedSteps.Where(s => s.ProjectKey == projectKey).ToList();
    }

    public async Task<StepProgress?> GetStepAsync(int stepNumber)
        => await GetStepAsync("trucking", stepNumber);

    public async Task<StepProgress?> GetStepAsync(string projectKey, int stepNumber)
    {
        var steps = await GetAllStepsAsync(projectKey);
        return steps.FirstOrDefault(s => s.StepNumber == stepNumber);
    }

    public async Task MarkStepCompleteAsync(int stepNumber)
        => await MarkStepCompleteAsync("trucking", stepNumber);

    public async Task MarkStepCompleteAsync(string projectKey, int stepNumber)
    {
        var allSteps = await GetAllStepsAsyncInternal();
        var step = allSteps.FirstOrDefault(s => s.ProjectKey == projectKey && s.StepNumber == stepNumber);
        
        if (step != null && !step.IsComplete)
        {
            step.IsComplete = true;
            step.CompletedDate = DateTime.Now;
            await SaveStepsAsync(allSteps);
        }
    }

    public async Task MarkStepIncompleteAsync(int stepNumber)
        => await MarkStepIncompleteAsync("trucking", stepNumber);

    public async Task MarkStepIncompleteAsync(string projectKey, int stepNumber)
    {
        var allSteps = await GetAllStepsAsyncInternal();
        var step = allSteps.FirstOrDefault(s => s.ProjectKey == projectKey && s.StepNumber == stepNumber);
        
        if (step != null && step.IsComplete)
        {
            step.IsComplete = false;
            step.CompletedDate = null;
            await SaveStepsAsync(allSteps);
        }
    }

    public async Task<int> GetCompletedCountAsync()
        => await GetCompletedCountAsync("trucking");

    public async Task<int> GetCompletedCountAsync(string projectKey)
    {
        var steps = await GetAllStepsAsync(projectKey);
        return steps.Count(s => s.IsComplete);
    }

    public async Task ResetAllProgressAsync()
    {
        _cachedSteps = GetDefaultSteps();
        await SaveStepsAsync(_cachedSteps);
    }

    private async Task SaveStepsAsync(List<StepProgress> steps)
    {
        try
        {
            var json = JsonSerializer.Serialize(steps);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
            _cachedSteps = steps;
        }
        catch
        {
            // localStorage might fail in prerendering - that's okay
        }
    }

    // Internal helper to get all steps across all projects (no filtering)
    private async Task<List<StepProgress>> GetAllStepsAsyncInternal()
    {
        if (_cachedSteps != null)
            return _cachedSteps;

        // Reuse main loading logic
        await GetAllStepsAsync("trucking");
        return _cachedSteps ?? GetDefaultSteps();
    }

    // Synchronous helpers for components (uses cached data)
    public bool IsStepComplete(string stepId)
    {
        if (_cachedSteps == null) return false;
        var step = _cachedSteps.FirstOrDefault(s => s.StepId == stepId);
        return step?.IsComplete ?? false;
    }

    public void MarkStepComplete(string stepId)
    {
        if (_cachedSteps == null)
        {
            _cachedSteps = GetDefaultSteps();
        }

        var step = _cachedSteps.FirstOrDefault(s => s.StepId == stepId);
        if (step != null && !step.IsComplete)
        {
            step.IsComplete = true;
            step.CompletedDate = DateTime.Now;
            
            // Save asynchronously without awaiting
            _ = SaveStepsAsync(_cachedSteps);
        }
    }
}
