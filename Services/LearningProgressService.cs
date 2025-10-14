using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorMock.Services;

public class StepProgress
{
    public int StepNumber { get; set; }
    public string StepId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
    public DateTime? CompletedDate { get; set; }
}

public interface ILearningProgressService
{
    Task<List<StepProgress>> GetAllStepsAsync();
    Task<StepProgress?> GetStepAsync(int stepNumber);
    Task MarkStepCompleteAsync(int stepNumber);
    Task MarkStepIncompleteAsync(int stepNumber);
    Task<int> GetCompletedCountAsync();
    Task ResetAllProgressAsync();
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
            new() { StepNumber = 1, StepId = "step1", Title = "New Clean Project", IsComplete = false },
            new() { StepNumber = 2, StepId = "step2", Title = "Razor Syntax & Display", IsComplete = false },
            new() { StepNumber = 3, StepId = "step3", Title = "Reusable Components", IsComplete = false },
            new() { StepNumber = 4, StepId = "step4", Title = "Event Binding", IsComplete = false },
            new() { StepNumber = 5, StepId = "step5", Title = "Forms & Validation", IsComplete = false },
            new() { StepNumber = 6, StepId = "step6", Title = "Routing & Navigation", IsComplete = false },
            new() { StepNumber = 7, StepId = "step7", Title = "EF Core Models", IsComplete = false },
            new() { StepNumber = 8, StepId = "step8", Title = "Setup EF Core & DbContext", IsComplete = false },
            new() { StepNumber = 9, StepId = "step9", Title = "CRUD Pages", IsComplete = false },
            new() { StepNumber = 10, StepId = "step10", Title = "State Management", IsComplete = false },
            new() { StepNumber = 11, StepId = "step11", Title = "Assignment Logic", IsComplete = false },
            new() { StepNumber = 12, StepId = "step12", Title = "Pay Calculation", IsComplete = false },
            new() { StepNumber = 13, StepId = "step13", Title = "Dashboard & Reports", IsComplete = false }
        };
    }

    public async Task<List<StepProgress>> GetAllStepsAsync()
    {
        if (_cachedSteps != null)
            return _cachedSteps;

        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", StorageKey);
            
            if (!string.IsNullOrEmpty(json))
            {
                _cachedSteps = JsonSerializer.Deserialize<List<StepProgress>>(json) ?? GetDefaultSteps();
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

        return _cachedSteps;
    }

    public async Task<StepProgress?> GetStepAsync(int stepNumber)
    {
        var steps = await GetAllStepsAsync();
        return steps.FirstOrDefault(s => s.StepNumber == stepNumber);
    }

    public async Task MarkStepCompleteAsync(int stepNumber)
    {
        var steps = await GetAllStepsAsync();
        var step = steps.FirstOrDefault(s => s.StepNumber == stepNumber);
        
        if (step != null && !step.IsComplete)
        {
            step.IsComplete = true;
            step.CompletedDate = DateTime.Now;
            await SaveStepsAsync(steps);
        }
    }

    public async Task MarkStepIncompleteAsync(int stepNumber)
    {
        var steps = await GetAllStepsAsync();
        var step = steps.FirstOrDefault(s => s.StepNumber == stepNumber);
        
        if (step != null && step.IsComplete)
        {
            step.IsComplete = false;
            step.CompletedDate = null;
            await SaveStepsAsync(steps);
        }
    }

    public async Task<int> GetCompletedCountAsync()
    {
        var steps = await GetAllStepsAsync();
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
