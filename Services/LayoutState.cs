using Microsoft.AspNetCore.Components;

namespace BlazorMock.Services;

public class LayoutState
{
    private RenderFragment? _header;
    public RenderFragment? Header => _header;

    public event Action? Changed;

    public void SetHeader(RenderFragment? header)
    {
        _header = header;
        Changed?.Invoke();
    }
}
