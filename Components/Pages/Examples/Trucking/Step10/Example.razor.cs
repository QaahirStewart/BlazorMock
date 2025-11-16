using BlazorMock.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorMock.Components.Pages.Examples.Trucking.Step10;

public partial class Example : IDisposable
{
    [Inject] private ILearningProgressService ProgressService { get; set; } = default!;
    [Inject] private AppState AppState { get; set; } = default!;

    private bool isComplete = false;

    protected override async Task OnInitializedAsync()
    {
        var step = await ProgressService.GetStepAsync(10);
        isComplete = step?.IsComplete ?? false;
    }

    protected override void OnInitialized()
    {
        AppState.OnChange += OnAppStateChanged;
    }

    private async Task MarkComplete()
    {
        await ProgressService.MarkStepCompleteAsync(10);
        isComplete = true;
    }

    private void OnAppStateChanged() => InvokeAsync(StateHasChanged);

    public void Dispose()
    {
        AppState.OnChange -= OnAppStateChanged;
    }
}
