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

// Blazor component styling and patterns
