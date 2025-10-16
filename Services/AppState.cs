using System;
using BlazorMock.Models;

namespace BlazorMock.Services;

/// <summary>
/// Simple scoped state service used to share selected driver across components.
/// Demonstrates event-based notifications for Step 10.
/// </summary>
public class AppState
{
    private Driver? _selectedDriver;

    /// <summary>
    /// Current selected driver (nullable).
    /// </summary>
    public Driver? SelectedDriver
    {
        get => _selectedDriver;
        private set
        {
            _selectedDriver = value;
            OnChange?.Invoke();
        }
    }

    /// <summary>
    /// Raised whenever state changes, so subscribers can re-render.
    /// </summary>
    public event Action? OnChange;

    public void SelectDriver(Driver? driver) => SelectedDriver = driver;
    public void ClearSelection() => SelectedDriver = null;
}
