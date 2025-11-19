using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using BlazorMock.Services;

namespace BlazorMock.Components.Pages;

public partial class TipsBase : ComponentBase, IDisposable
{
    [Inject] protected ITipsService TipsService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    protected readonly Dictionary<string, List<TipTopic>> grouped = new(StringComparer.OrdinalIgnoreCase);
    protected List<string> categories = [];
    protected List<string> blazorCats = [];
    protected List<string> csharpCats = [];
    protected List<string> otherCats = [];
    protected string? selectedTechnology;
    protected string? selectedCategory;
    protected int? openIndex;

    protected override void OnInitialized()
    {
        foreach (var c in TipsService.Categories)
        {
            var list = TipsService.GetByCategory(c).ToList();
            if (list.Count == 0) continue;
            grouped[c] = list;
            categories.Add(c);
        }

        blazorCats = categories.Where(c => c.StartsWith("Blazor", StringComparison.OrdinalIgnoreCase)).OrderBy(c => c).ToList();
        csharpCats = categories.Where(c => c.StartsWith("C#", StringComparison.OrdinalIgnoreCase)).OrderBy(c => c).ToList();
        otherCats = categories.Except(blazorCats.Concat(csharpCats), StringComparer.OrdinalIgnoreCase).OrderBy(c => c).ToList();

        Navigation.LocationChanged += HandleLocationChanged;
        HandleUrlHash();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            HandleUrlHash();
        }
    }

    private void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        HandleUrlHash();
        StateHasChanged();
    }

    private void HandleUrlHash()
    {
        var uri = new Uri(Navigation.Uri);
        var hash = uri.Fragment.TrimStart('#');

        if (string.IsNullOrEmpty(hash)) return;

        foreach (var category in categories)
        {
            var tips = grouped[category];
            for (int i = 0; i < tips.Count; i++)
            {
                if (tips[i].Slug.Equals(hash, StringComparison.OrdinalIgnoreCase))
                {
                    if (category.StartsWith("Blazor", StringComparison.OrdinalIgnoreCase))
                        selectedTechnology = "Blazor";
                    else if (category.StartsWith("C#", StringComparison.OrdinalIgnoreCase))
                        selectedTechnology = "C#";

                    selectedCategory = category;
                    openIndex = i;
                    return;
                }
            }
        }
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= HandleLocationChanged;
    }

    protected void SelectTechnology(string tech)
    {
        selectedTechnology = tech;
        selectedCategory = null;
        openIndex = null;
    }

    protected void SelectBlazor() => SelectTechnology("Blazor");
    protected void SelectCSharp() => SelectTechnology("C#");

    protected void SelectCategory(string category)
    {
        selectedCategory = category;
        openIndex = null;
    }

    protected void BackToTechnologies()
    {
        selectedTechnology = null;
        selectedCategory = null;
        openIndex = null;
        Navigation.NavigateTo("/tips", false);
    }

    protected void BackToCategories()
    {
        selectedCategory = null;
        openIndex = null;
        Navigation.NavigateTo("/tips", false);
    }

    protected string CategoryDisplay(string category)
    {
        // For now, just return the category as-is; customize if needed.
        return category;
    }

    protected void Toggle(int index)
    {
        if (openIndex == index)
            openIndex = null;
        else
            openIndex = index;
    }
}
