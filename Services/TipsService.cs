using System.Collections.Concurrent;

namespace BlazorMock.Services;

public record TipTopic(
    string Title,
    string Category, // e.g., "Blazor", "C#", "Routing"
    string Type,     // e.g., "Event", "Parameter", "Method"
    string ELI5,
    string Example,
    IReadOnlyList<string> Tips
)
{
    public string? LongELI5 { get; init; }
    public string? ELI5Example { get; init; }
    
    // Generate a URL-friendly slug from the title
    public string Slug => Title
        .ToLowerInvariant()
        .Replace("@", "")
        .Replace("[", "")
        .Replace("]", "")
        .Replace("<", "")
        .Replace(">", "")
        .Replace(".", "")
        .Replace(" ", "-")
        .Replace("(", "")
        .Replace(")", "");
}

public interface ITipsContributor
{
    IEnumerable<TipTopic> GetTopics();
}

public interface ITipsService
{
    IReadOnlyList<string> Categories { get; }
    IReadOnlyList<TipTopic> GetByCategory(string category);
    void AddContributor(ITipsContributor contributor);
}

public class TipsService : ITipsService
{
    private readonly ConcurrentDictionary<string, List<TipTopic>> _byCategory = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyList<string> Categories => _byCategory.Keys.OrderBy(k => k).ToList();

    public IReadOnlyList<TipTopic> GetByCategory(string category)
        => _byCategory.TryGetValue(category, out var list)
            ? list.OrderBy(t => t.Title, StringComparer.OrdinalIgnoreCase).ToList()
            : Array.Empty<TipTopic>();

    public void AddContributor(ITipsContributor contributor)
    {
        foreach (var t in contributor.GetTopics())
        {
            var list = _byCategory.GetOrAdd(t.Category, _ => []);
            list.Add(t);
        }
    }
}

// Default starter topics
public class DefaultTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "@onclick",
            Category: "Blazor — Event Binding",
            Type: "Event",
            ELI5: "Connect a UI event (like click) to a C# method in your component.",
            Example: "<button @onclick=\"Increment\">+1</button>\n@code { int count; void Increment() => count++; }",
            Tips: new[] { "Handlers can be async Task.", "Keep logic small; move heavy work to methods/services." }
        )
        {
            LongELI5 = "\n\n'@onclick' is like wiring a button to a little action in your component. When the user clicks, Blazor runs your method. You update your state (like count++), and Blazor redraws the UI so the user sees the new value.",
            ELI5Example = "<button @onclick=\"async () => await SaveAsync()\">Save</button>\n@code { bool saving; async Task SaveAsync(){ saving = true; try { await Task.Delay(300); } finally { saving = false; } } }"
        };

        yield return new TipTopic(
            Title: "[Parameter]",
            Category: "Blazor — Component Parameters",
            Type: "Parameter",
            ELI5: "Let a parent pass data into a child component.",
            Example: "[Parameter] public string Name { get; set; }\n<Child Name=\"Sam\" />",
            Tips: new[] { "Prefer one-way parameters unless you need two-way binding.", "Use clear PascalCase names." }
        )
        {
            LongELI5 = "\n\nA parent puts data into a child's lunchbox. The child doesn't make it; it just uses it. '[Parameter]' is how a child says: 'I accept this from my parent.'",
            ELI5Example = "// Parent.razor\n<HelloCard Name=\"Sam\" />\n\n// HelloCard.razor\n<h3>Hello, @Name!</h3>\n@code { [Parameter] public string Name { get; set; } }"
        };

        yield return new TipTopic(
            Title: "private vs async Task",
            Category: "C# — Methods",
            Type: "Method",
            ELI5: "'private' limits access to this component/class; 'void' returns nothing; use 'async Task' for async work.",
            Example: "void DoIt() {}\nasync Task DoItAsync() { await Task.Delay(1); }",
            Tips: new[] { "Avoid async void except event handlers.", "Return Task for async methods." }
        )
        {
            LongELI5 = "\n\n'private' is like a note you keep inside your own notebook — only you read it. 'void' means you did the chore but didn't bring back a receipt. If the chore takes time (like going to the store), use 'async Task' so you can say 'I'll be back later' instead of blocking the whole room.",
            ELI5Example = "void Paint() { /* quick, no return */ }\nasync Task PaintAsync(){ await Task.Delay(100); /* long running */ }"
        };

        yield return new TipTopic(
            Title: "private void Increment()",
            Category: "C# — Methods",
            Type: "Method",
            ELI5: "A private method that does work and doesn't return a value.",
            Example: "<button @onclick=\"Increment\">+1</button>\n@code { private int count; private void Increment() => count++; }",
            Tips: new[]
            {
                "Keep UI event handlers small and focused.",
                "Use private for component-internal helpers.",
                "If you await work, switch to async Task."
            }
        )
        {
            LongELI5 = "\n\n'private' = 'only for me' (the thing that owns it).\nThink of a secret button inside your toy box that only toys in that box can press.\n\n'void' = 'doesn't give anything back.'\nIt does something (like move a toy), but it doesn't hand you any new toy afterward.\n\nSo private void Increment() means:\n- It's a little action the component keeps to itself (private).\n- When you call it, it does something (increment a number) but it doesn't return any value to whoever called it (void).\n\nWhy we use it in Blazor (real simple):\n- You make a method to run when the user clicks a button. The method only needs to change internal state — it doesn't need to return anything — so 'void' is fine.\n- You make it private because only that component uses it; other parts of the app don't need to call it directly.",
            ELI5Example = "@code { private int count; private void Increment() => count++; private void IncrementTwice(){ Increment(); Increment(); } }"
        };

    }
}

public class FormsTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "EditForm + Validation",
            Category: "Blazor — Forms",
            Type: "Form",
            ELI5: "Wrap inputs with EditForm so Blazor can validate and manage submission.",
            Example: "<EditForm Model=\"m\" OnValidSubmit=\"Handle\"><DataAnnotationsValidator /><ValidationSummary /></EditForm>",
            Tips: new[] { "Use ValidationMessage for per-field errors.", "Add DataAnnotations on your model props." }
        )
        {
            LongELI5 = "\n\n'EditForm' is like a teacher checking homework. You put your work (inputs) in the folder (form), and when you hand it in (submit), the teacher checks the rules (validation) and either approves it or shows you what's wrong.",
            ELI5Example = "@code { Person m = new(); bool tried; async Task Handle(){ await Task.Delay(10); } }\n<EditForm Model=\"m\" OnValidSubmit=\"Handle\" OnInvalidSubmit=\"_ => tried = true\">\n  <DataAnnotationsValidator />\n  <InputText @bind-Value=\"m.Name\" />\n  <ValidationMessage For=\"() => m.Name\" />\n  <button type=\"submit\">Submit</button>\n</EditForm>"
        };

        yield return new TipTopic(
            Title: "Validation attributes",
            Category: "Blazor — Forms",
            Type: "Validation",
            ELI5: "Decorate model properties with attributes like [Required] so Blazor knows what is valid.",
            Example: "[Required] public string Name { get; set; }",
            Tips: new[] { "Use [StringLength], [EmailAddress], [Range] as needed.", "Custom validation can be added via IValidatableObject." }
        )
        {
            LongELI5 = "\n\nAttributes are like stickers on your homework: 'Name is required', 'Email must look like an email'. When you turn it in, the teacher (validation) checks the stickers and tells you what to fix.",
            ELI5Example = "public class Person {\n  [Required] public string Name { get; set; }\n  [EmailAddress] public string Email { get; set; }\n}"
        };

        yield return new TipTopic(
            Title: "InputSelect + enum binding",
            Category: "Blazor — Forms",
            Type: "Form",
            ELI5: "Bind a <select> to an enum so the value is strongly-typed in C#.",
            Example: "<InputSelect @bind-Value=\"TruckType\">\n  @foreach (var t in Enum.GetValues<TruckType>())\n  { <option value=\"@t\">@t</option> }\n</InputSelect>\n@code { private TruckType TruckType { get; set; } }",
            Tips: new[] { "Use Enum.GetValues<T>() to loop options.", "Ensure option values match enum names or numeric values.", "Model binder converts automatically." }
        )
        {
            LongELI5 = "\n\nThis is like using a dropdown that only allows valid truck types. Instead of strings you have a real enum value in C#—less errors, more clarity.",
            ELI5Example = "public enum TruckType { Box, Flatbed, Reefer }\n@code { private TruckType TruckType { get; set; } = TruckType.Box; }"
        };

        yield return new TipTopic(
            Title: "InputRadioGroup",
            Category: "Blazor — Forms",
            Type: "Form",
            ELI5: "Use radio buttons for mutually exclusive choices and bind them to a typed value.",
            Example: "<InputRadioGroup @bind-Value=\"Status\">\n  <InputRadio Value=\"Active\" /> Active\n  <InputRadio Value=\"Inactive\" /> Inactive\n</InputRadioGroup>\n@code { private string Status { get; set; } = \"Active\"; }",
            Tips: new[] { "Group radios with InputRadioGroup for binding.", "Supports enums or primitives.", "Great for small, clear choice sets." }
        )
        {
            LongELI5 = "\n\nLike a paper form where you can pick only one bubble. The bound property updates as you pick.",
            ELI5Example = "public enum DriverStatus { Active, Inactive }\n@code { private DriverStatus Status { get; set; } }"
        };

        // New: Form model (View model)
        yield return new TipTopic(
            Title: "Form model (View model)",
            Category: "Blazor — Forms",
            Type: "Model",
            ELI5: "A small class that represents the fields of your form with validation attributes. Keeps UI concerns separate from your domain/EF entity.",
            Example: "public class DriverRegistrationModel {\n  [Required] public string FirstName { get; set; } = string.Empty;\n  [Required] public string LastName { get; set; } = string.Empty;\n}\n<EditForm Model=\"m\"><DataAnnotationsValidator /><InputText @bind-Value=\"m.FirstName\" /></EditForm>",
            Tips: new[] { "Keep the form model lean and UI-focused.", "Use DataAnnotations like [Required], [EmailAddress], [Range].", "Map the form model to your entity on submit instead of binding the entity directly." }
        )
        {
            LongELI5 = "\n\nThink of a form model like a worksheet you fill out before typing it into the official record. It has just what the form needs, uses attributes to validate, and you copy the approved data to your real entity (which may have more rules/relations) after submit.",
            ELI5Example = "// Map to domain entity on submit\nvar entity = new Driver {\n  Name = $\"{m.FirstName} {m.LastName}\"\n};"
        };
    }
}

public class RoutingTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "@page route",
            Category: "Blazor — Routing",
            Type: "Route",
            ELI5: "Add @page \"/path\" at the top of a component so the router can navigate to it.",
            Example: "@page \"/counter\"\n<h1>Counter</h1>",
            Tips: new[] { "Routes start with /", "Parameters use {id} and can be typed with constraints." }
        )
        {
            LongELI5 = "\n\n'@page \"/abc\"' is like putting an address on a house. Now the mail (navigation) can find it. Type the address in the bar, and Blazor shows that component.",
            ELI5Example = "@page \"/greet/{name}\"\n<h3>Hello @name!</h3>\n@code { [Parameter] public string name { get; set; } }"
        };
        yield return new TipTopic(
            Title: "NavLink",
            Category: "Blazor — Routing",
            Type: "Link",
            ELI5: "Like an anchor tag but knows when the route is active and can style itself.",
            Example: "<NavLink href=\"/\" ActiveClass=\"active\">Home</NavLink>",
            Tips: new[] { "Use ActiveClass for selected styling.", "Prefer NavLink over <a> for SPA routes." }
        )
        {
            LongELI5 = "\n\n'NavLink' is like a smart signpost. It not only sends you to the place, it also lights up when you're already there.",
            ELI5Example = "<NavLink href=\"/reports\" Match=\"NavLinkMatch.All\" ActiveClass=\"active\">Reports</NavLink>"
        };

        yield return new TipTopic(
            Title: "Route constraints",
            Category: "Blazor — Routing",
            Type: "Route",
            ELI5: "Restrict route parameters to certain types or patterns for safer navigation.",
            Example: "@page \"/orders/{id:int}\"\n@page \"/files/{*path}\"",
            Tips: new[] { "Common: :int, :guid, :bool, :decimal", "Catch-all with {*} for nested paths.", "Routes are matched top-to-bottom; keep specific ones first." }
        )
        {
            LongELI5 = "\n\nConstraints are like guards at the door: only numbers go into this page, or only GUIDs. Less chance of weird errors.",
            ELI5Example = "@page \"/driver/{id:int}\"\n@code { [Parameter] public int id { get; set; } }"
        };

        yield return new TipTopic(
            Title: "Optional parameters",
            Category: "Blazor — Routing",
            Type: "Route",
            ELI5: "Make part of your route optional to show default content when not provided.",
            Example: "@page \"/reports/{year:int?}\"\n@if (year is null) { <p>All years</p> } else { <p>@year</p> }",
            Tips: new[] { "Use ? to mark optional route params.", "Parameter type must be nullable (int? etc.).", "Provide sensible defaults in the UI." }
        )
        {
            LongELI5 = "\n\nIt’s like a store that shows all items if you don’t choose a year, or filters by the year if you do.",
            ELI5Example = "@code { [Parameter] public int? year { get; set; } }"
        };
    }
}

public class ServicesTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "Dependency Injection (DI)",
            Category: "Blazor — Services",
            Type: "Service",
            ELI5: "Register classes in Program.cs and ask Blazor to give you an instance with @inject.",
            Example: "builder.Services.AddSingleton<MyService>();\n@inject MyService Svc",
            Tips: new[] { "Use Singleton for stateless or app-wide state.", "Scoped is per-circuit (Blazor Server)." }
        )
        {
            LongELI5 = "\n\nDI is like a tool cart the classroom shares. You register the tools once, and any student (component) can ask the teacher (Blazor) for the tool when needed.",
            ELI5Example = "// Program.cs\nbuilder.Services.AddScoped<WeatherClient>();\n\n// Component\n@inject WeatherClient Client\n@code { protected override async Task OnInitializedAsync() => await Client.LoadAsync(); }"
        };
        yield return new TipTopic(
            Title: "Scoped vs Singleton",
            Category: "Blazor — Services",
            Type: "Lifetime",
            ELI5: "Singleton = 1 for the whole app; Scoped = 1 per user circuit; Transient = new every time.",
            Example: "builder.Services.AddScoped<MyScoped>();",
            Tips: new[] { "Prefer Scoped for per-user data in Blazor Server.", "Avoid singletons when holding user-specific state." }
        )
        {
            LongELI5 = "\n\nSingleton: one big water cooler for the whole office. Scoped: one water bottle per person. Transient: a brand-new cup every time you ask.",
            ELI5Example = "// Program.cs\nbuilder.Services.AddSingleton<AppClock>();\nbuilder.Services.AddScoped<UserCart>();\nbuilder.Services.AddTransient<Formatter>();"
        };
    }
}

public class LifecycleTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "OnInitialized/OnInitializedAsync",
            Category: "Blazor — Lifecycle",
            Type: "Lifecycle",
            ELI5: "Runs when the component is first set up; good place to load data.",
            Example: "protected override async Task OnInitializedAsync(){ await Load(); }",
            Tips: new[] { "Async version is preferred for I/O.", "Avoid heavy CPU work here; offload to services." }
        )
        {
            LongELI5 = "\n\nThis is like the first day the classroom opens. Set up your desks (state), bring in books (data). If you need to fetch from outside, use the async version.",
            ELI5Example = "protected override async Task OnInitializedAsync(){ items = await Api.GetTopAsync(count: 5); }"
        };
        yield return new TipTopic(
            Title: "OnParametersSet",
            Category: "Blazor — Lifecycle",
            Type: "Lifecycle",
            ELI5: "Runs when parameters from a parent change.",
            Example: "protected override void OnParametersSet(){ /* react to new params */ }",
            Tips: new[] { "Re-run dependent logic when inputs change.", "Avoid triggering infinite re-renders." }
        )
        {
            LongELI5 = "\n\nLike getting a new care package from home. When the box arrives (parent updates), you unpack it and adjust your room (re-calc state).",
            ELI5Example = "protected override void OnParametersSet(){ pageTitle = $\"Order #{OrderId}\"; }"
        };

        yield return new TipTopic(
            Title: "ShouldRender override",
            Category: "Blazor — Lifecycle",
            Type: "Lifecycle",
            ELI5: "Control whether the component re-renders after state changes.",
            Example: "protected override bool ShouldRender() => _canRender;",
            Tips: new[] { "Return false to skip expensive renders.", "Use sparingly; most perf issues can be solved otherwise.", "Don’t forget state still updates even if not rendered." }
        )
        {
            LongELI5 = "\n\nLike pausing screen refresh while you do a batch of changes. You still make changes, you just don’t draw them yet.",
            ELI5Example = "bool _canRender = true; protected override bool ShouldRender() => _canRender;"
        };
    }
}

public class NavigationTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "NavigationManager",
            Category: "Blazor — Navigation",
            Type: "Navigation",
            ELI5: "Service to read current URI and navigate to other pages.",
            Example: "@inject NavigationManager Nav\n<button @onclick=\"() => Nav.NavigateTo(\"/tips\")\">Go</button>",
            Tips: new[] { "Use NavigateTo for programmatic navigation.", "Check Nav.Uri for the current URL." }
        )
        {
            LongELI5 = "\n\nNavigationManager is your in-app GPS. It tells you where you are (Uri) and lets you drive to a new address (NavigateTo). Unlike reloading the whole site, it's like taking streets inside the same neighborhood — fast and smooth.",
            ELI5Example = "@inject NavigationManager Nav\n<p>You're at: @Nav.Uri</p>\n<button @onclick=\"() => Nav.NavigateTo(\"/orders?tab=recent\")\">Recent orders</button>"
        };

        yield return new TipTopic(
            Title: "NavLink Match modes",
            Category: "Blazor — Navigation",
            Type: "Link",
            ELI5: "Control when a NavLink counts as active: exact match or starts-with.",
            Example: "<NavLink href=\"/reports\" Match=\"NavLinkMatch.All\">Reports</NavLink>",
            Tips: new[] { "All = exact match only.", "Prefix = any child route under it.", "Helps highlight parent sections." }
        )
        {
            LongELI5 = "\n\nThink of a folder tree: All matches only the folder itself, Prefix matches the folder and all subfolders.",
            ELI5Example = "<NavLink href=\"/settings\" Match=\"NavLinkMatch.Prefix\">Settings</NavLink>"
        };

        yield return new TipTopic(
            Title: "forceLoad navigation",
            Category: "Blazor — Navigation",
            Type: "Navigation",
            ELI5: "Force a full page load when navigating if the target must be served by the server.",
            Example: "@inject NavigationManager Nav\n<button @onclick=\"() => Nav.NavigateTo(\"/external\", forceLoad: true)\">Open</button>",
            Tips: new[] { "Use when navigating outside the SPA or to non-component endpoints.", "Avoid unless necessary to keep SPA fast." }
        )
        {
            LongELI5 = "\n\nNormally you take in-neighborhood roads (SPA). forceLoad is like taking the highway and re-entering the city—useful when the destination isn’t in your neighborhood.",
            ELI5Example = "Nav.NavigateTo(\"/Account/Logout\", forceLoad: true);"
        };
    }
}

public class JsInteropTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "IJSRuntime.InvokeAsync",
            Category: "Blazor — JS Interop",
            Type: "Interop",
            ELI5: "Call JavaScript functions from C# when you need browser APIs.",
            Example: "@inject IJSRuntime JS\nawait JS.InvokeAsync<object>(\"console.log\", \"Hello\");",
            Tips: new[] { "Prefer JS modules (IJSObjectReference) for organization.", "Keep interop isolated to a service or wrapper." }
        )
        {
            LongELI5 = "\n\nInterop is like using a walkie‑talkie to ask the browser for help. C# says, 'Hey JS, can you do this thing only you can do (like read the clipboard)?' JS does it and replies back. Use modules when you have many calls so your tools stay in one labeled toolbox.",
            ELI5Example = "@inject IJSRuntime JS\n@code {\n  private async Task CopyAsync(string text) {\n    var mod = await JS.InvokeAsync<IJSObjectReference>(\"import\", \"./js/clipboard.js\");\n    await mod.InvokeVoidAsync(\"copy\", text);\n    await mod.DisposeAsync();\n  }\n}"
        };

        yield return new TipTopic(
            Title: "JS modules (import)",
            Category: "Blazor — JS Interop",
            Type: "Interop",
            ELI5: "Import a JS module once and call functions from it; dispose when done.",
            Example: "@inject IJSRuntime JS\nIJSObjectReference? _mod;\n_mod = await JS.InvokeAsync<IJSObjectReference>(\"import\", \"./Interop/clipboard.js\");\nawait _mod.InvokeVoidAsync(\"copy\", text);\nawait _mod.DisposeAsync();",
            Tips: new[] { "Cache module in a field for multiple calls.", "Dispose modules in DisposeAsync.", "Use relative path from the calling component’s location." }
        )
        {
            LongELI5 = "\n\nIt’s like opening a toolbox once and reusing the tools, then putting the toolbox away when done.",
            ELI5Example = "public class ClipboardInterop : IAsyncDisposable {\n  private readonly IJSRuntime _js; private IJSObjectReference? _mod;\n  public ClipboardInterop(IJSRuntime js) => _js = js;\n  public async Task CopyAsync(string t){ _mod ??= await _js.InvokeAsync<IJSObjectReference>(\"import\", \"./js/clipboard.js\"); await _mod.InvokeVoidAsync(\"copy\", t);}\n  public async ValueTask DisposeAsync(){ if(_mod is not null) await _mod.DisposeAsync(); }\n}"
        };

        yield return new TipTopic(
            Title: "Passing ElementReference to JS",
            Category: "Blazor — JS Interop",
            Type: "Interop",
            ELI5: "You can pass a reference to a DOM element from Blazor to JS for manipulation.",
            Example: "<input @ref=\"input\" />\n@code { ElementReference input; await JS.InvokeVoidAsync(\"focusEl\", input); }",
            Tips: new[] { "Works after first render (use OnAfterRenderAsync).", "Avoid storing ElementReference across renders unless needed.", "Prefer small JS helpers that act on the element." }
        )
        {
            LongELI5 = "\n\nThink of it like handing JS a pointer to the actual element, so JS can call focus(), measure size, etc.",
            ELI5Example = "// JS\nexport function focusEl(el){ el?.focus(); }"
        };
    }
}

public class LifecycleAdvancedTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "OnAfterRender/OnAfterRenderAsync",
            Category: "Blazor — Lifecycle",
            Type: "Lifecycle",
            ELI5: "Runs after the component has rendered; good place to interact with JS or measure the DOM.",
            Example: "protected override async Task OnAfterRenderAsync(bool firstRender){ if(firstRender){ /* JS interop */ } }",
            Tips: new[] { "Use firstRender to avoid loops.", "Avoid calling StateHasChanged repeatedly unless needed." }
        )
        {
            LongELI5 = "\n\nThink of painting a wall: only after the paint dries can you stick a hook on it. OnAfterRender runs after Blazor finishes drawing the UI, so it's safe to measure sizes or call JS. Use 'firstRender' like 'only do this once' tape to avoid doing heavy setup every time.",
            ELI5Example = "protected override async Task OnAfterRenderAsync(bool firstRender) {\n  if (firstRender) {\n    // e.g., import a JS module and wire up listeners once\n  }\n}"
        };
        yield return new TipTopic(
            Title: "IDisposable / IAsyncDisposable (disposing)",
            Category: "Blazor — Lifecycle",
            Type: "Dispose",
            ELI5: "Clean up timers, subscriptions, or JS object references when the component is removed.",
            Example: "public void Dispose(){ _timer?.Dispose(); }",
            Tips: new[] { "Dispose JS module references via IAsyncDisposable.", "Unsubscribe from events to prevent leaks." }
        )
        {
            LongELI5 = "\n\nWhen you leave a campsite, you put out the fire and pack your trash. Components should do the same: stop timers, close streams, and dispose JS modules. If teardown needs 'await' (like telling JS to release something), implement IAsyncDisposable.",
            ELI5Example = "public sealed class Clock : IAsyncDisposable {\n  private IJSObjectReference? _mod;\n  public async ValueTask DisposeAsync() { if (_mod is not null) await _mod.DisposeAsync(); }\n}"
        };
    }
}

public class DataBindingTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "Two-way binding (@bind)",
            Category: "Blazor — Data Binding",
            Type: "Binding",
            ELI5: "Connect an input to a C# property so UI changes update the property and changes to the property update the UI.",
            Example: "<InputText @bind-Value=\"Name\" />\n<p>@Name</p>",
            Tips: new[] { "Backed by Value and ValueChanged.", "Use bind-Value:event to customize the event." }
        )
        {
            LongELI5 = "\n\nTwo‑way binding is like a two‑way radio between your UI and your state. Speak on one side (type), the other side hears it (property updates). Change the property in code, the UI hears it too and refreshes.",
            ELI5Example = "<InputNumber @bind-Value=\"Age\" />\n<p>Age: @Age</p>"
        };
        yield return new TipTopic(
            Title: "bind-value:event",
            Category: "Blazor — Data Binding",
            Type: "Binding",
            ELI5: "Choose which DOM event updates your bound value (e.g., oninput vs onchange).",
            Example: "<InputText @bind-Value=\"Name\" @bind-Value:event=\"oninput\" />",
            Tips: new[] { "oninput updates per keystroke.", "onchange updates when the control loses focus or Enter." }
        )
        {
            LongELI5 = "\n\nIt's like choosing when to take a snapshot. 'oninput' is rapid‑fire photos every time the user types; 'onchange' is a single photo after they're done. Pick the one that fits your UX and performance needs.",
            ELI5Example = "<textarea @bind-Value=\"Notes\" @bind-Value:event=\"oninput\"></textarea>"
        };

        yield return new TipTopic(
            Title: "bind-value:format (Date/Numbers)",
            Category: "Blazor — Data Binding",
            Type: "Binding",
            ELI5: "Specify a format string when binding to format how values appear in inputs.",
            Example: "<InputDate @bind-Value=\"Start\" @bind-Value:format=\"yyyy-MM-dd\" />\n@code { DateTime Start { get; set; } = DateTime.Today; }",
            Tips: new[] { "Works with InputDate, InputNumber, etc.", "Use culture-appropriate formats.", "Format affects display; bound value stays typed." }
        )
        {
            LongELI5 = "\n\nLike choosing how a date is printed on the form (YYYY-MM-DD or MM/DD/YYYY) while keeping the actual C# DateTime intact.",
            ELI5Example = "<InputNumber @bind-Value=\"Weight\" @bind-Value:format=\"0.##\" />"
        };

        yield return new TipTopic(
            Title: "Custom two-way binding (get/set)",
            Category: "Blazor — Data Binding",
            Type: "Binding",
            ELI5: "Create your own two-way bindable parameter using Value + ValueChanged + ValueExpression.",
            Example: "[Parameter] public string? Value { get; set; }\n[Parameter] public EventCallback<string?> ValueChanged { get; set; }\nvoid OnInput(ChangeEventArgs e) => ValueChanged.InvokeAsync(e.Value?.ToString());",
            Tips: new[] { "Expose Value and ValueChanged for @bind support.", "Name must match for @bind-Value to work.", "Add ValueExpression for validation scenarios." }
        )
        {
            LongELI5 = "\n\nThis lets your custom component act like InputText: parent can write @bind-Value and it just works.",
            ELI5Example = "<MyInput @bind-Value=\"Name\" />"
        };
    }
}

public class CSharpLanguageTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "Classes",
            Category: "C# — Types",
            Type: "Type",
            ELI5: "Reference types for modeling objects with state, behavior, and identity.",
            Example: "public class Customer {\n  public int Id { get; set; }\n  public string Name { get; set; }\n  public void PlaceOrder() { }\n}",
            Tips: new[] { "Use classes for entities with identity and mutable state.", "Prefer composition over deep inheritance.", "Sealed classes can improve performance." }
        )
        {
            LongELI5 = "\n\nClasses are like folders in a filing cabinet. Each folder has a unique identity (even if two folders have identical papers inside, they're still different folders). You can add, remove, or change papers (properties), and the folder has actions it can do (methods).",
            ELI5Example = "public class ShoppingCart {\n  private List<string> items = [];\n  public void Add(string item) => items.Add(item);\n  public int Count => items.Count;\n}"
        };

        yield return new TipTopic(
            Title: "Records",
            Category: "C# — Types",
            Type: "Type",
            ELI5: "Immutable data containers with built-in equality and copy methods.",
            Example: "public record Person(string Name, int Age);",
            Tips: new[] { "Use records for DTOs and value objects.", "Supports 'with' expressions for non-destructive mutation.", "Records have value-based equality by default." }
        )
        {
            LongELI5 = "\n\nRecords are like sealed envelopes with printed info. Once sealed, you can't change what's inside—but you can copy the envelope with one detail changed (using 'with'). Two envelopes with the same info are considered identical.",
            ELI5Example = "public record Order(int Id, decimal Total);\nvar o1 = new Order(1, 99.99m);\nvar o2 = o1 with { Total = 120m }; // new copy"
        };

        yield return new TipTopic(
            Title: "LINQ (Where, Select)",
            Category: "C# — Collections",
            Type: "Method",
            ELI5: "Query and transform collections using method chaining.",
            Example: "var adults = people.Where(p => p.Age >= 18).Select(p => p.Name);",
            Tips: new[] { "LINQ is lazy—results are computed when enumerated.", "Use ToList()/ToArray() to materialize immediately.", "Combine with async for IAsyncEnumerable." }
        )
        {
            LongELI5 = "\n\nLINQ is like a conveyor belt with filters and machines. Items flow through: 'Where' removes items that don't match, 'Select' transforms each item. Nothing actually moves until you pull the lever (enumerate).",
            ELI5Example = "var names = users\n  .Where(u => u.Active)\n  .Select(u => u.FullName)\n  .ToList();"
        };

        yield return new TipTopic(
            Title: "async / await",
            Category: "C# — Async",
            Type: "Keyword",
            ELI5: "Write asynchronous code that looks synchronous; keeps your app responsive.",
            Example: "async Task<string> FetchAsync() => await httpClient.GetStringAsync(url);",
            Tips: new[] { "Always await async calls; don't block with .Result.", "Return Task for async methods, not void (except event handlers).", "Use ConfigureAwait(false) in libraries to avoid context capture." }
        )
        {
            LongELI5 = "\n\nAsync/await is like ordering food: you place the order (start the task), do other things while it cooks (await pauses without blocking), and when it's ready you pick it up (resume after await). The kitchen (thread pool) handles many orders at once.",
            ELI5Example = "async Task LoadDataAsync() {\n  var data = await api.GetAsync(\"/data\");\n  items = data.Items;\n}"
        };

        yield return new TipTopic(
            Title: "Nullable reference types",
            Category: "C# — Null Safety",
            Type: "Feature",
            ELI5: "Annotate which reference types can be null to catch errors at compile time.",
            Example: "string? nullable = null; // can be null\nstring required = \"ok\"; // cannot be null",
            Tips: new[] { "Enable in .csproj with <Nullable>enable</Nullable>.", "Use null-forgiving operator (!) sparingly.", "Annotate APIs clearly for consumers." }
        )
        {
            LongELI5 = "\n\nNullable reference types are like labeled boxes: some say 'may be empty' (string?), others say 'always has something' (string). The compiler checks you don't accidentally treat an 'empty-allowed' box as 'always full'.",
            ELI5Example = "string? FindUser(int id) => id > 0 ? \"Alice\" : null;\nvar name = FindUser(1) ?? \"Guest\";"
        };

        yield return new TipTopic(
            Title: "Pattern matching (is/switch)",
            Category: "C# — Patterns",
            Type: "Syntax",
            ELI5: "Test values and extract data in one expression; more expressive than if-else chains.",
            Example: "if (obj is Person { Age: > 18 } p) { Console.WriteLine(p.Name); }",
            Tips: new[] { "Use switch expressions for concise mapping.", "Combine with when clauses for extra conditions.", "Patterns work with types, constants, and property checks." }
        )
        {
            LongELI5 = "\n\nPattern matching is like a smart sorting machine: it checks the shape and contents of an item, then routes it to the right bin—all in one step. Traditional if-else is like checking each bin manually.",
            ELI5Example = "var result = input switch {\n  > 100 => \"High\",\n  > 50 => \"Medium\",\n  _ => \"Low\"\n};"
        };

        yield return new TipTopic(
            Title: "List vs IEnumerable",
            Category: "C# — Collections",
            Type: "Type",
            ELI5: "List is a concrete mutable collection; IEnumerable is an abstract sequence (often lazy).",
            Example: "IEnumerable<int> query = nums.Where(n => n > 0); // lazy\nList<int> list = query.ToList(); // materialized",
            Tips: new[] { "Prefer IEnumerable for method parameters (flexible).", "Use List when you need indexing or mutation.", "Avoid multiple enumeration of IEnumerable (cache if needed)." }
        )
        {
            LongELI5 = "\n\nIEnumerable is like a recipe: it describes how to get items, but doesn't cook them until you ask. List is a plate of food: everything is ready and you can pick any item instantly.",
            ELI5Example = "IEnumerable<string> GetLines() {\n  yield return \"A\";\n  yield return \"B\";\n}\nvar all = GetLines().ToList();"
        };

        yield return new TipTopic(
            Title: "Expression-bodied members",
            Category: "C# — Syntax",
            Type: "Syntax",
            ELI5: "Use => for concise one-liner methods, properties, and constructors.",
            Example: "public int Double(int x) => x * 2;\npublic string Name => _name;",
            Tips: new[] { "Use for simple getters and methods.", "Improves readability for single expressions.", "Works with methods, properties, indexers, and constructors." }
        )
        {
            LongELI5 = "\n\nExpression-bodied members are like shorthand notes: instead of writing a full sentence with punctuation (braces and return), you write the core idea with an arrow. Faster to read and write.",
            ELI5Example = "public class Circle {\n  public double Radius { get; }\n  public double Area => Math.PI * Radius * Radius;\n}"
        };

        yield return new TipTopic(
            Title: "String interpolation",
            Category: "C# — Strings",
            Type: "Syntax",
            ELI5: "Embed expressions directly in strings with $ prefix.",
            Example: "var msg = $\"Hello, {name}! You have {count} items.\";",
            Tips: new[] { "Use for readability over string.Format or concatenation.", "Supports formatting: {value:C} for currency, {date:yyyy-MM-dd}.", "Use @ for verbatim strings, combine as $@\"...\"." }
        )
        {
            LongELI5 = "\n\nString interpolation is like mail merge: you write a template with blanks ({name}), and C# fills them in with real values. Cleaner than gluing strings together with +.",
            ELI5Example = "var price = 19.99m;\nvar text = $\"Total: {price:C} (tax included)\";"
        };

        yield return new TipTopic(
            Title: "Tuples (ValueTuple)",
            Category: "C# — Types",
            Type: "Type",
            ELI5: "Lightweight data structures for returning multiple values without a custom class.",
            Example: "(int x, int y) GetCoords() => (10, 20);\nvar (x, y) = GetCoords();",
            Tips: new[] { "Use tuples for internal methods returning 2-3 values.", "Name tuple elements for clarity.", "For public APIs, consider a record or class instead." }
        )
        {
            LongELI5 = "\n\nTuples are like a small basket: you put a couple things in, pass it around, and unpack them. No need to build a fancy box (class) for a quick errand.",
            ELI5Example = "(string Name, int Score) GetWinner() => (\"Alice\", 95);\nvar winner = GetWinner();\nConsole.WriteLine($\"{winner.Name}: {winner.Score}\");"
        };

        yield return new TipTopic(
            Title: "Local functions",
            Category: "C# — Methods",
            Type: "Feature",
            ELI5: "Define helper functions inside a method to keep logic private and scoped.",
            Example: "void Process() {\n  int Helper(int x) => x * 2;\n  var result = Helper(5);\n}",
            Tips: new[] { "Use for single-caller helpers to avoid polluting class scope.", "Can capture local variables from enclosing method.", "Supports async, generic, and static modifiers." }
        )
        {
            LongELI5 = "\n\nLocal functions are like sticky notes inside a recipe: they're helper steps only that recipe needs, not shared with other recipes. Keeps your kitchen organized.",
            ELI5Example = "int Fibonacci(int n) {\n  int Fib(int i) => i <= 1 ? i : Fib(i-1) + Fib(i-2);\n  return Fib(n);\n}"
        };

        // New: Enums
        yield return new TipTopic(
            Title: "Enums",
            Category: "C# — Types",
            Type: "Type",
            ELI5: "A named list of fixed choices. Safer and clearer than magic strings for options like statuses or license levels.",
            Example: "public enum LicenseLevel { ClassC = 1, ClassB = 2, ClassA = 3 }",
            Tips: new[] { "Use enums for finite sets of options.", "Apply [Display] to show friendly names in the UI.", "Bind to <InputSelect> with Enum.GetValues<T>()." }
        )
        {
            LongELI5 = "\n\nAn enum is like a labeled box of crayons with a fixed set of colors. You pick one by name (ClassA), not by remembering a number. Less typos, more intent.",
            ELI5Example = "public enum LicenseLevel {\n  [Display(Name = \"Class C (Light)\")] ClassC = 1,\n  [Display(Name = \"Class B (Medium)\")] ClassB = 2,\n  [Display(Name = \"Class A (Heavy)\")] ClassA = 3\n}\n\n// Helper to read DisplayAttribute name\nstatic string GetDisplay(LicenseLevel level){\n  var m = typeof(LicenseLevel).GetMember(level.ToString())[0];\n  var d = (System.ComponentModel.DataAnnotations.DisplayAttribute?)\n           Attribute.GetCustomAttribute(m, typeof(System.ComponentModel.DataAnnotations.DisplayAttribute));\n  return string.IsNullOrWhiteSpace(d?.Name) ? level.ToString() : d!.Name!;\n}"
        };

        yield return new TipTopic(
            Title: "Switch expressions in Razor",
            Category: "C# — Syntax",
            Type: "Syntax",
            ELI5: "Use concise switch expressions directly in Razor to map values to strings or UI.",
            Example: "<p>Status: @(status switch { 0 => \"New\", 1 => \"Active\", _ => \"Unknown\" })</p>",
            Tips: new[] { "Great for small mapping tables.", "Keep logic readable; factor out if long.", "Works in code-behind too." }
        )
        {
            LongELI5 = "\n\nIt’s like a tiny dictionary inline—input on the left, result on the right.",
            ELI5Example = "var name = code switch { 200 => \"OK\", 404 => \"Not Found\", _ => \"?\" };"
        };

        yield return new TipTopic(
            Title: "Null-coalescing assignment (??=)",
            Category: "C# — Syntax",
            Type: "Syntax",
            ELI5: "If a variable is null, assign a default value in one step.",
            Example: "list ??= new List<int>();",
            Tips: new[] { "Handy for lazy init of fields.", "Keeps code compact and safe.", "Use with dictionaries, lists, services, etc." }
        )
        {
            LongELI5 = "\n\nLike saying: if the cup is empty, fill it now—otherwise leave it alone.",
            ELI5Example = "_cache ??= new();"
        };
    }
}

public class HttpAndDataTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "IHttpClientFactory",
            Category: "Blazor — HTTP & Data",
            Type: "Service",
            ELI5: "A factory that creates and manages HttpClient instances with pre-configured settings (like base URLs and headers). Register it in Program.cs and inject it into your components.",
            Example: "// Program.cs\nbuilder.Services.AddHttpClient(\"PokeApi\", client => client.BaseAddress = new Uri(\"https://pokeapi.co/api/v2/\"));\n\n// Component\n@inject IHttpClientFactory HttpClientFactory\n@code {\n  protected override async Task OnInitializedAsync() {\n    var client = HttpClientFactory.CreateClient(\"PokeApi\");\n    var data = await client.GetStringAsync(\"pokemon?limit=20\");\n  }\n}",
            Tips: new[] {
                "Name your clients for clarity (\"PokeApi\", \"WeatherApi\").",
                "Configure base URLs, default headers, and timeout in Program.cs.",
                "Reuse named clients across components—no need to repeat config."
            }
        )
        {
            LongELI5 = "\n\nThink of IHttpClientFactory like a car rental service. Instead of building a car from scratch every time you need one (new HttpClient()), you ask the rental service for a car that's already set up with GPS, insurance, and a full tank (configured base URL, headers, etc.).\n\nYou register the 'rental plan' once in Program.cs with a name like \"PokeApi\", specifying what the car comes with (base address = https://pokeapi.co). Then whenever a component needs to call that API, it asks the factory: 'Give me the PokeApi client' and gets a ready-to-use HttpClient.\n\nWhy use it?\n- Prevents socket exhaustion (reusing connections).\n- Centralizes config so you don't repeat BaseAddress everywhere.\n- Makes testing easier (swap out the factory for a mock).",
            ELI5Example = "// In Program.cs:\nbuilder.Services.AddHttpClient(\"PokeApi\", client => {\n  client.BaseAddress = new Uri(\"https://pokeapi.co/api/v2/\");\n  client.DefaultRequestHeaders.Add(\"User-Agent\", \"BlazorApp\");\n});\n\n// In your component:\n@inject IHttpClientFactory Factory\n@code {\n  var client = Factory.CreateClient(\"PokeApi\");\n  var json = await client.GetStringAsync(\"pokemon/25\"); // calls https://pokeapi.co/api/v2/pokemon/25\n}"
        };

        yield return new TipTopic(
            Title: "GetFromJsonAsync<T>",
            Category: "Blazor — HTTP & Data",
            Type: "Method",
            ELI5: "A one-liner extension method on HttpClient that fetches JSON from an API and deserializes it into a C# object in a single step.",
            Example: "var response = await client.GetFromJsonAsync<PokemonListResponse>(\"pokemon?limit=20\");\n// response is now a PokemonListResponse object, not raw JSON string",
            Tips: new[] {
                "Requires System.Net.Http.Json namespace (built into .NET).",
                "Returns null if the response is empty or invalid.",
                "Use it instead of GetStringAsync + JsonSerializer.Deserialize for cleaner code."
            }
        )
        {
            LongELI5 = "\n\nImagine you order a pizza online. Normally you'd:\n1. Call the restaurant (HTTP GET).\n2. Get a menu card with text (JSON string).\n3. Read the menu and write down what you want on your own paper (deserialize into an object).\n\nGetFromJsonAsync<T> does all three steps at once: it calls the restaurant, reads the menu, and hands you a typed order form already filled out.\n\nExample: Instead of:\n  var json = await client.GetStringAsync(\"pokemon?limit=20\");\n  var response = JsonSerializer.Deserialize<PokemonListResponse>(json);\n\nYou write:\n  var response = await client.GetFromJsonAsync<PokemonListResponse>(\"pokemon?limit=20\");\n\nCleaner, safer, and less code.",
            ELI5Example = "public class PokemonListResponse {\n  public List<PokemonItem> Results { get; set; } = new();\n}\n\npublic class PokemonItem {\n  public string Name { get; set; } = \"\";\n  public string Url { get; set; } = \"\";\n}\n\n@code {\n  private List<PokemonItem> pokemon = new();\n  protected override async Task OnInitializedAsync() {\n    var client = HttpClientFactory.CreateClient(\"PokeApi\");\n    var response = await client.GetFromJsonAsync<PokemonListResponse>(\"pokemon?limit=20\");\n    if (response?.Results != null) pokemon = response.Results;\n  }\n}"
        };

        yield return new TipTopic(
            Title: "DTO models",
            Category: "Blazor — HTTP & Data",
            Type: "Pattern",
            ELI5: "DTO (Data Transfer Object) models are simple C# classes that match the shape of JSON you get from an API. They let you work with strongly-typed objects instead of parsing raw strings.",
            Example: "public class PokemonItem {\n  public string Name { get; set; } = \"\";\n  public string Url { get; set; } = \"\";\n}\n\npublic class PokemonListResponse {\n  public List<PokemonItem> Results { get; set; } = new();\n}",
            Tips: new[] {
                "Property names must match JSON keys (or use [JsonPropertyName] attribute).",
                "Keep DTOs simple—just properties, no business logic.",
                "Use records for immutability if you don't need to modify them."
            }
        )
        {
            LongELI5 = "\n\nA DTO is like a shopping list template. When you call an API, it returns data in a specific format (like a receipt). You create a C# class that mirrors that receipt's layout—item names, prices, etc.\n\nInstead of reading the receipt as raw text and parsing line by line, you hand the receipt to GetFromJsonAsync<T> and it fills out your template class automatically. Now you can say 'receipt.Total' instead of hunting for the total in a string.\n\nWhy 'DTO'? Because these classes only exist to transfer data between your app and the API—they're messengers, not the core of your app.\n\nExample: PokeAPI returns:\n{\n  \"results\": [\n    { \"name\": \"bulbasaur\", \"url\": \"https://...\" },\n    ...\n  ]\n}\n\nSo you make:\npublic class PokemonListResponse {\n  public List<PokemonItem> Results { get; set; } = new();\n}\npublic class PokemonItem {\n  public string Name { get; set; } = \"\";\n  public string Url { get; set; } = \"\";\n}\n\nNow 'response.Results[0].Name' gives you \"bulbasaur\" without manual parsing.",
            ELI5Example = "// If JSON key is different from C# property, use attribute:\npublic class PokemonDetail {\n  [JsonPropertyName(\"base_experience\")]\n  public int BaseExperience { get; set; }\n}"
        };

        yield return new TipTopic(
            Title: "@foreach in Razor",
            Category: "Blazor — Rendering",
            Type: "Loop",
            ELI5: "Loop through a collection in Razor markup to render one block of HTML per item. Like a stamping machine that prints a card for each item in a list.",
            Example: "<ul>\n  @foreach (var item in items) {\n    <li>@item.Name</li>\n  }\n</ul>",
            Tips: new[] {
                "Use @foreach for lists, arrays, or any IEnumerable.",
                "Wrap in a null-check or show a 'Loading...' message if the collection can be null.",
                "Keep the loop body simple—extract complex logic into methods or properties."
            }
        )
        {
            LongELI5 = "\n\n@foreach is how you turn a C# list into repeated HTML. Imagine you have a stack of Pokemon cards (a List<PokemonItem>). You want to display each card on the screen. Instead of writing <div>bulbasaur</div>, <div>charmander</div>, ... manually, you write one template and let @foreach repeat it for every card.\n\nBlazor steps through your list, and for each item, it stamps out the HTML you defined inside the loop—substituting @item.Name with the actual name.\n\nExample:\nIf you have:\nList<PokemonItem> pokemon = new() {\n  new() { Name = \"bulbasaur\" },\n  new() { Name = \"charmander\" }\n};\n\nAnd you write:\n<ul>\n  @foreach (var p in pokemon) {\n    <li class=\"card\">@p.Name</li>\n  }\n</ul>\n\nBlazor renders:\n<ul>\n  <li class=\"card\">bulbasaur</li>\n  <li class=\"card\">charmander</li>\n</ul>\n\nIt's the Razor version of a for-loop, but cleaner for UI.",
            ELI5Example = "@if (pokemon is null) {\n  <p>Loading Pokemon...</p>\n}\nelse if (pokemon.Count == 0) {\n  <p>No Pokemon found.</p>\n}\nelse {\n  <div class=\"grid grid-cols-4 gap-3\">\n    @foreach (var p in pokemon) {\n      <div class=\"card\">\n        <p class=\"font-bold\">@p.Name</p>\n        <img src=\"@p.SpriteUrl\" alt=\"@p.Name\" />\n      </div>\n    }\n  </div>\n}"
        };

        yield return new TipTopic(
            Title: "dotnet CLI",
            Category: "Tooling — .NET",
            Type: "Tool",
            ELI5: "The command-line interface for creating, building, and running .NET projects. Like a Swiss Army knife for .NET development.",
            Example: "dotnet new blazor -n MyApp --interactivity Server --all-interactive --empty\ndotnet build\ndotnet run",
            Tips: new[] {
                "Use 'dotnet new list' to see all available project templates.",
                "Add --help to any command to see detailed options.",
                "Combine flags to customize your project structure from the start."
            }
        )
        {
            LongELI5 = "\n\nThe dotnet CLI is like a construction foreman for .NET projects. Instead of clicking through menus in Visual Studio, you type commands that tell .NET exactly what to build.\n\n'dotnet new blazor' creates a new Blazor project from a template. The flags customize it:\n- '-n MyApp' names your project\n- '--interactivity Server' enables server-side interactivity\n- '--all-interactive' makes all components interactive by default\n- '--empty' creates a minimal project without sample pages\n\n'dotnet build' compiles your code into DLLs.\n'dotnet run' builds and starts your app.\n\nWhy use CLI instead of IDE menus? It's faster, scriptable, and works everywhere (Windows, Mac, Linux, CI/CD pipelines).",
            ELI5Example = "# Create a new Blazor Server app\ndotnet new blazor -n PokemonCollector --interactivity Server --all-interactive --empty\n\n# Navigate into the project\ncd PokemonCollector\n\n# Build the project\ndotnet build\n\n# Run the app\ndotnet run"
        };

        yield return new TipTopic(
            Title: "npm",
            Category: "Tooling — Frontend",
            Type: "Tool",
            ELI5: "Node Package Manager—a tool for installing JavaScript libraries and tools like Tailwind CSS. Think of it as an app store for frontend development tools.",
            Example: "npm install @tailwindcss/cli@next\nnpm run build:css",
            Tips: new[] {
                "package.json defines your project's dependencies and scripts.",
                "Use 'npm install' to download all dependencies listed in package.json.",
                "Define custom scripts in package.json to simplify common tasks."
            }
        )
        {
            LongELI5 = "\n\nnpm is like a vending machine for code tools. You tell it what you need (like Tailwind CSS), and it downloads and installs it into your project.\n\nWhen you run 'npm install @tailwindcss/cli@next', npm:\n1. Downloads the Tailwind CLI tool\n2. Saves it in a 'node_modules' folder\n3. Records it in package.json so teammates can install the same version\n\npackage.json is your shopping list. It lists all the tools your project needs and defines shortcuts (scripts) for common tasks like 'build:css' or 'watch:css'.\n\nWhy use npm in a .NET project? Because modern CSS tools like Tailwind are JavaScript-based. npm bridges the .NET and frontend worlds.",
            ELI5Example = "// package.json\n{\n  \"scripts\": {\n    \"build:css\": \"npx @tailwindcss/cli@next -i ./Styles/input.css -o ./wwwroot/app.css\",\n    \"watch:css\": \"npx @tailwindcss/cli@next -i ./Styles/input.css -o ./wwwroot/app.css --watch\"\n  },\n  \"devDependencies\": {\n    \"@tailwindcss/cli\": \"^4.0.0-alpha.25\"\n  }\n}\n\n# Run the build script\nnpm run build:css"
        };

        yield return new TipTopic(
            Title: "Tailwind CSS",
            Category: "Styling — CSS",
            Type: "Framework",
            ELI5: "A utility-first CSS framework that provides pre-built classes like 'text-blue-500' or 'rounded-lg' so you can style without writing custom CSS.",
            Example: "<div class=\"bg-blue-100 p-4 rounded-lg text-gray-800\">\n  Hello Tailwind!\n</div>",
            Tips: new[] {
                "Version 4 simplifies setup—just @import in CSS, no config file needed.",
                "Use responsive prefixes like 'sm:text-lg' for mobile-first design.",
                "Combine utilities to build complex designs without leaving HTML."
            }
        )
        {
            LongELI5 = "\n\nTailwind CSS is like Lego blocks for styling. Instead of writing custom CSS rules, you snap together pre-made utility classes directly in your HTML.\n\nTraditional CSS:\n<div class=\"my-card\">...</div>\n\n.my-card {\n  background-color: #dbeafe;\n  padding: 1rem;\n  border-radius: 0.5rem;\n  color: #1f2937;\n}\n\nTailwind way:\n<div class=\"bg-blue-100 p-4 rounded-lg text-gray-800\">...</div>\n\nNo need to name classes or switch between HTML and CSS files. The classes describe exactly what they do: 'bg-blue-100' = blue background, 'p-4' = padding, 'rounded-lg' = large border radius.\n\nTailwind v4 makes setup even simpler—just @import 'tailwindcss' in your CSS file and start using utilities. No complex config files required.",
            ELI5Example = "<!-- Gradient banner with responsive text sizes -->\n<div class=\"bg-gradient-to-r from-gray-100 to-blue-100 rounded-2xl p-6\">\n  <h2 class=\"text-xl sm:text-2xl font-semibold text-gray-800 mb-4\">\n    What You'll Learn\n  </h2>\n  <p class=\"text-sm sm:text-base text-gray-700\">\n    Build modern UIs with utility classes.\n  </p>\n</div>"
        };

        yield return new TipTopic(
            Title: "Watch Mode",
            Category: "Tooling — Development",
            Type: "Feature",
            ELI5: "A mode where a tool automatically rebuilds your files whenever you save changes. Like having a helper that re-compiles CSS every time you edit it.",
            Example: "npx @tailwindcss/cli -i ./Styles/input.css -o ./wwwroot/app.css --watch",
            Tips: new[] {
                "Keep watch mode running in a separate terminal while developing.",
                "Use npm scripts to simplify the command: 'npm run watch:css'.",
                "Watch mode detects changes and rebuilds automatically—no manual refresh needed."
            }
        )
        {
            LongELI5 = "\n\nWatch mode is like having a robot assistant that monitors your work. When you edit input.css and save, the robot sees the change and immediately runs the build command to regenerate app.css.\n\nWithout watch mode, you'd have to:\n1. Edit input.css\n2. Switch to terminal\n3. Run 'npm run build:css'\n4. Switch back to browser\n5. Refresh\n\nWith watch mode running (--watch flag), steps 2-3 happen automatically. You just save and refresh the browser.\n\nExample workflow:\n1. Open terminal, run 'npm run watch:css' (starts watching)\n2. Edit Styles/input.css, save\n3. Watch mode rebuilds wwwroot/app.css automatically\n4. Refresh browser to see changes\n\nLeave watch mode running all day while developing—it's your auto-save for CSS.",
            ELI5Example = "# Terminal 1: Run watch mode (keeps running)\nnpm run watch:css\n\n# Output when you save input.css:\n# Rebuilding...\n# Done in 45ms.\n\n# Terminal 2: Run your Blazor app\ndotnet run"
        };

        yield return new TipTopic(
            Title: ".NET Asset Pipeline",
            Category: "Blazor — Assets",
            Type: "Feature",
            ELI5: "A system in .NET that copies static files (CSS, JS, images) from your project to the output folder and serves them to the browser. It's how wwwroot/app.css becomes accessible at /app.css in your app.",
            Example: "<!-- In Layout.razor -->\n<link rel=\"stylesheet\" href=\"app.css\" />\n\n<!-- .NET serves this from wwwroot/app.css -->",
            Tips: new[] {
                "Files in wwwroot are served as static assets automatically.",
                "Reference assets by their filename (app.css) not the full path (wwwroot/app.css).",
                "Asset pipeline runs during build—ensure files exist before referencing them."
            }
        )
        {
            LongELI5 = "\n\nThe .NET asset pipeline is like a delivery truck that moves your static files (CSS, JS, images) from your source code into the running app so browsers can request them.\n\nWhere files live:\n- Source: 'wwwroot/app.css' (in your project)\n- Output: 'bin/Debug/net10.0/wwwroot/app.css' (copied during build)\n- Browser: 'https://localhost:5001/app.css' (served by Kestrel)\n\nWhen you reference '<link rel=\"stylesheet\" href=\"app.css\" />', the browser requests '/app.css' and .NET serves it from wwwroot.\n\nKey rules:\n1. Put static files in wwwroot (CSS, JS, images, fonts)\n2. Reference them by relative path (app.css, not wwwroot/app.css)\n3. Build the project so .NET copies them to the output folder\n\nTailwind workflow example:\n- You run 'npm run build:css' which generates wwwroot/app.css\n- You reference it in MainLayout.razor as 'app.css'\n- When you 'dotnet build', .NET copies wwwroot/app.css to the output\n- Browser requests /app.css and gets the Tailwind styles",
            ELI5Example = "<!-- MainLayout.razor head section -->\n<head>\n    <link rel=\"stylesheet\" href=\"app.css\" />\n    <!-- .NET serves from wwwroot/app.css -->\n</head>\n\n# Terminal: Generate and build\nnpm run build:css  # Creates wwwroot/app.css\ndotnet build       # Copies to output folder"
        };

        yield return new TipTopic(
            Title: "LINQ Skip/Take",
            Category: "C# — LINQ",
            Type: "Method",
            ELI5: "Skip a certain number of items in a collection and take the next batch. Perfect for pagination.",
            Example: "var page2 = items.Skip(20).Take(20).ToList(); // Items 21-40",
            Tips: new[] {
                "Skip(n) skips the first n items, Take(n) grabs the next n items.",
                "Combine them for pagination: Skip((page-1)*pageSize).Take(pageSize).",
                "Works on any IEnumerable<T> including lists, arrays, and query results."
            }
        )
        {
            LongELI5 = "\n\nImagine a deck of cards. Skip(5) means 'put the first 5 cards aside'. Take(10) means 'grab the next 10 cards from what's left'. Together, they let you grab any slice of the deck.\n\nFor pagination:\n- Page 1: Skip(0).Take(20) → first 20 items\n- Page 2: Skip(20).Take(20) → items 21-40\n- Page 3: Skip(40).Take(20) → items 41-60\n\nThe formula is: Skip((currentPage - 1) * pageSize).Take(pageSize)\n\nWhy (currentPage - 1)?\n- Page 1: (1-1)*20 = 0, so Skip(0) starts at the beginning\n- Page 2: (2-1)*20 = 20, so Skip(20) starts at item 21\n- Page 3: (3-1)*20 = 40, so Skip(40) starts at item 41\n\nSkip and Take don't modify the original collection—they return a new sequence with just the items you want.",
            ELI5Example = "// Paginate a list of 151 Pokemon\nList<Pokemon> allPokemon = GetAll(); // 151 items\nint currentPage = 2;\nint pageSize = 20;\n\n// Get page 2 (items 21-40)\nvar page = allPokemon\n    .Skip((currentPage - 1) * pageSize)  // Skip 20\n    .Take(pageSize)                       // Take 20\n    .ToList();\n\n// page now contains Pokemon #21 through #40"
        };

        yield return new TipTopic(
            Title: "Math.Ceiling",
            Category: "C# — Math",
            Type: "Method",
            ELI5: "Round a decimal number up to the nearest whole number. Used to calculate total pages when items don't divide evenly.",
            Example: "int totalPages = (int)Math.Ceiling(151 / 20.0); // 8 pages",
            Tips: new[] {
                "Always rounds UP: Ceiling(7.1) = 8, Ceiling(7.9) = 8.",
                "Cast one operand to double to avoid integer division: count / (double)pageSize.",
                "Essential for pagination to ensure the last partial page is counted."
            }
        )
        {
            LongELI5 = "\n\nMath.Ceiling is like a strict teacher grading homework. Even if you're 99% done with page 8, the teacher says 'you need 8 full pages', so they round up.\n\nWhy use it for pagination?\nIf you have 151 Pokemon and show 20 per page:\n- 151 ÷ 20 = 7.55 pages\n- Without Ceiling: you'd think you have 7 pages, and the last 11 Pokemon disappear!\n- With Ceiling(7.55) = 8 pages, the last 11 Pokemon get their own page 8.\n\nCommon mistake: Integer division\n```csharp\nint totalPages = 151 / 20;  // WRONG: 7 (integer division drops decimals)\nint totalPages = (int)Math.Ceiling(151 / 20.0);  // CORRECT: 8\n```\n\nYou MUST convert to double before dividing, or C# does integer division and throws away the decimal before Ceiling sees it.\n\nAlternative ways to ensure double division:\n- count / (double)pageSize\n- (double)count / pageSize\n- count / 20.0 (add .0 to make it a double literal)",
            ELI5Example = "// Calculate pages for 151 items, 20 per page\nint itemCount = 151;\nint pageSize = 20;\n\n// WRONG - integer division\nint wrong = itemCount / pageSize;  // 7 (loses last 11 items!)\n\n// CORRECT - cast to double first\nint correct = (int)Math.Ceiling(itemCount / (double)pageSize);  // 8\n\n// Also correct\nint alsoCorrect = (int)Math.Ceiling((double)itemCount / pageSize);  // 8"
        };

        yield return new TipTopic(
            Title: "disabled Attribute",
            Category: "Blazor — HTML Attributes",
            Type: "Attribute",
            ELI5: "Disable a button or input so users can't interact with it. In Blazor, use disabled=\"@(condition)\" to make it dynamic.",
            Example: "<button disabled=\"@(currentPage == 1)\">Previous</button>",
            Tips: new[] {
                "In Blazor, wrap the condition in @(): disabled=\"@(condition)\".",
                "Use CSS like disabled:opacity-50 to style disabled elements.",
                "Disabled buttons don't fire onclick events—guards are still good practice."
            }
        )
        {
            LongELI5 = "\n\nThe 'disabled' attribute is like putting a plastic cover over a button. You can see it, but you can't press it.\n\nIn regular HTML:\n```html\n<button disabled>Can't click me</button>\n```\n\nIn Blazor, you want it to be dynamic (enable/disable based on conditions):\n```razor\n<button disabled=\"@(currentPage == 1)\">Previous</button>\n```\n\nThis reads as: 'Disable this button when currentPage equals 1.'\n\nWhy the @() syntax?\n- The @ tells Blazor 'this is C# code, not plain text'\n- The () groups the condition so Blazor evaluates it as true/false\n- Result: When currentPage is 1, disabled=\"true\", otherwise disabled=\"false\"\n\nCommon mistake:\n```razor\n<!-- WRONG - Blazor treats this as the string 'currentPage == 1', not code -->\n<button disabled=\"currentPage == 1\">Previous</button>\n\n<!-- CORRECT -->\n<button disabled=\"@(currentPage == 1)\">Previous</button>\n```\n\nStyling disabled buttons:\n```razor\n<button class=\"disabled:opacity-50 disabled:cursor-not-allowed\"\n        disabled=\"@(currentPage >= totalPages)\">\n    Next\n</button>\n```\n\nThe disabled:opacity-50 class makes the button look faded when disabled, giving users a visual cue.",
            ELI5Example = "// Pagination buttons with disabled states\n<button disabled=\"@(currentPage == 1)\"\n        @onclick=\"PreviousPage\">\n    ← Previous\n</button>\n\n<button disabled=\"@(currentPage >= totalPages)\"\n        @onclick=\"NextPage\">\n    Next →\n</button>\n\n@code {\n    int currentPage = 1;\n    int totalPages = 8;\n    \n    void NextPage() {\n        if (currentPage < totalPages) currentPage++;\n    }\n    \n    void PreviousPage() {\n        if (currentPage > 1) currentPage--;\n    }\n}"
        };

        yield return new TipTopic(
            Title: "try-catch-finally",
            Category: "C# — Error Handling",
            Type: "Pattern",
            ELI5: "Protect your code from crashing by catching errors and ensuring cleanup happens no matter what.",
            Example: "try {\n    await LoadDataAsync();\n} catch (Exception ex) {\n    errorMessage = ex.Message;\n} finally {\n    isLoading = false;\n}",
            Tips: new[] {
                "Use try to wrap risky code (API calls, file access, etc.).",
                "Use catch to handle errors gracefully instead of crashing.",
                "Use finally to ensure cleanup code ALWAYS runs (loading flags, disposing resources)."
            }
        )
        {
            LongELI5 = "\n\ntry-catch-finally is like a safety net for tightrope walking.\n\n- **try**: You walk the rope (run risky code like an API call)\n- **catch**: If you fall (an error happens), the safety net catches you and you handle it gracefully\n- **finally**: No matter what (fell or made it across), you take off your harness (cleanup code)\n\nWhy use it?\nWithout try-catch, one error crashes your whole app. With it, you can show a friendly error message and let the user retry.\n\nfinally is critical for flags like isLoading:\n```csharp\ntry {\n    isLoading = true;\n    await Http.GetFromJsonAsync<Data>(url);\n} catch (Exception ex) {\n    errorMessage = ex.Message;\n    // If this was the end, isLoading would stay true forever!\n} finally {\n    isLoading = false;  // ALWAYS runs, even if catch triggers\n}\n```\n\nWithout finally, if the API call fails, isLoading stays true and your UI shows a spinner forever.",
            ELI5Example = "// Robust API call with loading and error states\nbool isLoading;\nstring? errorMessage;\nList<Item> items = new();\n\nprotected override async Task OnInitializedAsync()\n{\n    try\n    {\n        isLoading = true;\n        errorMessage = null;\n        items = await Http.GetFromJsonAsync<List<Item>>(\"api/items\") ?? new();\n    }\n    catch (HttpRequestException ex)\n    {\n        errorMessage = $\"Network error: {ex.Message}\";\n    }\n    catch (Exception ex)\n    {\n        errorMessage = $\"Unexpected error: {ex.Message}\";\n    }\n    finally\n    {\n        isLoading = false;  // Always runs\n    }\n}"
        };

        yield return new TipTopic(
            Title: "Loading State Pattern",
            Category: "Blazor — State Management",
            Type: "Pattern",
            ELI5: "Track whether data is being fetched so you can show a spinner or skeleton loader instead of a blank screen.",
            Example: "bool isLoading = true;\n@if (isLoading) {\n    <p>Loading...</p>\n} else {\n    @* Show data *@\n}",
            Tips: new[] {
                "Initialize isLoading = true if data loads in OnInitializedAsync.",
                "Set isLoading = false in finally block to ensure it always resets.",
                "Use Tailwind's animate-spin for simple spinners."
            }
        )
        {
            LongELI5 = "\n\nLoading states are like the 'please wait' sign at a restaurant kitchen. Instead of staring at an empty table wondering if your food will ever come, you see a sign that says 'your order is being prepared.'\n\nThe pattern:\n1. Start with isLoading = true (assumes data needs loading)\n2. In OnInitializedAsync, fetch your data\n3. In finally, set isLoading = false (whether success or error)\n4. In the UI, show different content based on isLoading\n\nWhy it matters:\nWithout loading states, users see a blank screen and wonder if the app is broken. With them, users know the app is working and data is coming.\n\nCommon mistake: Forgetting finally\n```csharp\ntry {\n    isLoading = true;\n    data = await FetchAsync();\n    isLoading = false;  // WRONG: If FetchAsync throws, this never runs!\n}\n```\n\nCorrect:\n```csharp\ntry {\n    isLoading = true;\n    data = await FetchAsync();\n} finally {\n    isLoading = false;  // CORRECT: Always runs\n}\n```",
            ELI5Example = "bool isLoading = true;\nList<Pokemon> pokemon = new();\n\nprotected override async Task OnInitializedAsync()\n{\n    try\n    {\n        isLoading = true;\n        pokemon = await Http.GetFromJsonAsync<List<Pokemon>>(url) ?? new();\n    }\n    finally\n    {\n        isLoading = false;\n    }\n}\n\n// In markup:\n@if (isLoading)\n{\n    <div class=\"flex items-center gap-3\">\n        <div class=\"animate-spin rounded-full h-8 w-8 border-4 border-blue-600 border-t-transparent\"></div>\n        <span>Loading Pokemon...</span>\n    </div>\n}\nelse\n{\n    <div class=\"grid grid-cols-3 gap-4\">\n        @foreach (var p in pokemon)\n        {\n            <div>@p.Name</div>\n        }\n    </div>\n}"
        };

        yield return new TipTopic(
            Title: "Error State Pattern",
            Category: "Blazor — State Management",
            Type: "Pattern",
            ELI5: "Store error messages in a nullable string so you can show friendly errors instead of crashing.",
            Example: "string? errorMessage;\ntry { await LoadAsync(); }\ncatch (Exception ex) { errorMessage = ex.Message; }",
            Tips: new[] {
                "Use string? errorMessage to store error text (null = no error).",
                "Clear errorMessage before retrying: errorMessage = null.",
                "Show user-friendly messages, not raw stack traces."
            }
        )
        {
            LongELI5 = "\n\nError states are like a friendly note when the vending machine is broken: 'Sorry, out of order. Try again later.' Instead of just not giving you your snack and saying nothing, it tells you what's wrong.\n\nThe pattern:\n1. Declare: string? errorMessage; (nullable = no error by default)\n2. In try, set errorMessage = null (clear old errors)\n3. In catch, set errorMessage = ex.Message or a friendly version\n4. In UI, check if errorMessage has a value and show it\n\nWhy it matters:\nWithout error states, your app fails silently. Users see nothing, have no idea what happened, and get frustrated. With error states, they know what went wrong and can retry or report it.\n\nUser-friendly messages:\n```csharp\ncatch (HttpRequestException ex)\n{\n    errorMessage = \"Unable to connect to the server. Please check your internet connection.\";\n}\ncatch (Exception ex)\n{\n    errorMessage = \"Something went wrong. Please try again later.\";\n    Console.WriteLine($\"Error details: {ex}\");  // Log full details for debugging\n}\n```",
            ELI5Example = "string? errorMessage;\nList<Pokemon> pokemon = new();\n\nprotected override async Task OnInitializedAsync()\n{\n    try\n    {\n        errorMessage = null;  // Clear previous errors\n        pokemon = await Http.GetFromJsonAsync<List<Pokemon>>(url) ?? new();\n    }\n    catch (HttpRequestException ex)\n    {\n        errorMessage = $\"Network error: {ex.Message}\";\n    }\n    catch (Exception ex)\n    {\n        errorMessage = \"Failed to load Pokemon. Please try again.\";\n        Console.WriteLine(ex);  // Log for debugging\n    }\n}\n\n// In markup:\n@if (!string.IsNullOrEmpty(errorMessage))\n{\n    <div class=\"bg-red-50 border border-red-200 rounded-lg p-4\">\n        <h3 class=\"text-red-800 font-semibold\">⚠️ Error</h3>\n        <p class=\"text-red-700\">@errorMessage</p>\n        <button @onclick=\"OnInitializedAsync\" class=\"mt-2 px-4 py-2 bg-red-600 text-white rounded\">Retry</button>\n    </div>\n}"
        };

        yield return new TipTopic(
            Title: "StateHasChanged",
            Category: "Blazor — Lifecycle",
            Type: "Method",
            ELI5: "Tell Blazor to re-render the component because you changed state outside the normal flow.",
            Example: "private void UpdateFromTimer() {\n    count++;\n    StateHasChanged();\n}",
            Tips: new[] {
                "Blazor auto-rerenders after event handlers (@onclick, etc.)—no need to call it there.",
                "You DO need it after timer callbacks, background tasks, or event subscriptions.",
                "Call it when state changes but Blazor doesn't know it changed."
            }
        )
        {
            LongELI5 = "\n\nStateHasChanged is like tapping someone on the shoulder to say 'hey, things changed, take a look.'\n\nBlazor automatically re-renders after:\n- @onclick and other event handlers\n- OnInitializedAsync completes\n- Parameter changes from parent\n\nBut Blazor DOESN'T automatically re-render after:\n- Timer.Elapsed events\n- Task.Run background work\n- External event subscriptions (SignalR, etc.)\n\nIn those cases, you call StateHasChanged() to say 'I updated state, please redraw the UI.'\n\nExample without StateHasChanged (BROKEN):\n```csharp\nprotected override void OnInitialized()\n{\n    var timer = new Timer(1000);\n    timer.Elapsed += (s, e) => {\n        count++;  // State changes, but UI doesn't update!\n    };\n    timer.Start();\n}\n```\n\nFixed:\n```csharp\ntimer.Elapsed += async (s, e) => {\n    await InvokeAsync(() => {\n        count++;\n        StateHasChanged();  // Now UI updates!\n    });\n};\n```\n\nNote: Use InvokeAsync when calling from a background thread to avoid threading issues.",
            ELI5Example = "// Example: Manual retry button\nprivate async Task RetryLoad()\n{\n    await OnInitializedAsync();  // Re-fetch data\n    StateHasChanged();  // Tell Blazor to redraw with new data\n}\n\n// Example: Timer updating UI\nprivate int seconds;\nprivate Timer? _timer;\n\nprotected override void OnInitialized()\n{\n    _timer = new Timer(1000);\n    _timer.Elapsed += async (s, e) => {\n        await InvokeAsync(() => {\n            seconds++;\n            StateHasChanged();\n        });\n    };\n    _timer.Start();\n}\n\npublic void Dispose() => _timer?.Dispose();"
        };
    }
}

// Blazor component styling and patterns
