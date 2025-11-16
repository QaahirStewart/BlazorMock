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

        yield return new TipTopic(
            Title: "Tailwind Responsive Grid",
            Category: "Styling — CSS",
            Type: "Utility",
            ELI5: "Use responsive grid-cols-* classes to create grids that adapt from mobile to desktop.",
            Example: "<div class=\"grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5\">\n    <!-- Cards -->\n</div>",
            Tips: new[] {
                "Default (no prefix) applies to all screen sizes, sm: for 640px+, md: for 768px+, lg: for 1024px+.",
                "Start mobile-first: grid-cols-2 (base) then scale up with sm:, md:, lg: prefixes.",
                "Combine with gap-* for consistent spacing between grid items."
            }
        )
        {
            LongELI5 = "\n\nTailwind's responsive grid is like building blocks that rearrange themselves based on screen size.\n\nHow it works:\n- Mobile (small screens): grid-cols-2 shows 2 columns\n- Tablet (sm: 640px+): sm:grid-cols-3 shows 3 columns\n- Small laptop (md: 768px+): md:grid-cols-4 shows 4 columns\n- Large screens (lg: 1024px+): lg:grid-cols-5 shows 5 columns\n\nMobile-first approach:\nTailwind applies styles from smallest screen up. Each breakpoint overrides the previous one:\n```html\n<div class=\"grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4\">\n```\n\nThis means:\n- 0-639px: 2 columns (grid-cols-2)\n- 640-767px: 3 columns (sm:grid-cols-3 overrides)\n- 768px+: 4 columns (md:grid-cols-4 overrides)\n\nBreakpoint reference:\n- sm: 640px (tablets)\n- md: 768px (small laptops)\n- lg: 1024px (desktops)\n- xl: 1280px (large desktops)\n- 2xl: 1536px (ultra-wide)\n\nCommon patterns:\n- Cards: grid-cols-1 sm:grid-cols-2 lg:grid-cols-3\n- Thumbnails: grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6\n- Dashboard: grid-cols-1 md:grid-cols-2 lg:grid-cols-3",
            ELI5Example = "// Pokemon grid that adapts to screen size\n<div class=\"grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4\">\n    @foreach (var pokemon in allPokemon)\n    {\n        <div class=\"border rounded-lg p-4\">\n            <img src=\"@pokemon.SpriteUrl\" alt=\"@pokemon.Name\" />\n            <p>@pokemon.Name</p>\n        </div>\n    }\n</div>\n\n// Result:\n// Mobile: 2 cards per row\n// Tablet: 3 cards per row\n// Laptop: 4 cards per row\n// Desktop: 5 cards per row"
        };

        yield return new TipTopic(
            Title: "Tailwind Hover Effects",
            Category: "Styling — CSS",
            Type: "Utility",
            ELI5: "Add hover: prefix to any utility class to apply it only when hovering over an element.",
            Example: "<button class=\"bg-blue-600 hover:bg-blue-700\">Hover me</button>",
            Tips: new[] {
                "Common hover patterns: hover:bg-*, hover:text-*, hover:border-*, hover:shadow-lg.",
                "Combine with transition-* for smooth animations.",
                "Works on any element, not just buttons."
            }
        )
        {
            LongELI5 = "\n\nHover effects are like magic tricks that happen when you move your mouse over something. In Tailwind, just add hover: before any utility class.\n\nCommon hover effects:\n\n**Background color changes:**\n```html\n<div class=\"bg-blue-500 hover:bg-blue-700\">Darkens on hover</div>\n```\n\n**Border and shadow:**\n```html\n<div class=\"border border-gray-200 hover:border-blue-400 hover:shadow-lg\">\n    Card lifts on hover\n</div>\n```\n\n**Text color:**\n```html\n<a class=\"text-blue-600 hover:text-blue-800\">Link</a>\n```\n\n**Scale (grow/shrink):**\n```html\n<img class=\"hover:scale-110 transition-transform\" />\n```\n\n**Multiple effects at once:**\n```html\n<button class=\"bg-green-600 hover:bg-green-700 \n               text-white hover:shadow-lg \n               transition-all\">\n    Click me\n</button>\n```\n\nWhy use transitions:\nWithout transition-*, changes are instant and jarring. With it, changes are smooth:\n```html\n<div class=\"bg-blue-500 hover:bg-blue-700 transition-colors\">\n    Smooth color fade\n</div>\n```\n\ntransition-* options:\n- transition-colors: Just colors\n- transition-all: Everything (color, size, shadow, etc.)\n- transition-transform: Just scale, rotate, etc.\n- transition-opacity: Just opacity",
            ELI5Example = "// Pokemon card with hover effects\n<div class=\"border border-gray-200 rounded-lg p-4 \n            hover:border-blue-400 hover:shadow-lg \n            transition-all cursor-pointer\">\n    <img src=\"@pokemon.SpriteUrl\" \n         class=\"hover:scale-110 transition-transform\" />\n    <p class=\"text-gray-800 hover:text-blue-600 transition-colors\">\n        @pokemon.Name\n    </p>\n</div>\n\n// Result: Card border turns blue, shadow appears, image grows slightly, text turns blue"
        };

        yield return new TipTopic(
            Title: "Tailwind Transitions",
            Category: "Styling — CSS",
            Type: "Utility",
            ELI5: "Add smooth animations to property changes with transition-* classes.",
            Example: "<button class=\"bg-blue-600 hover:bg-blue-700 transition-colors\">Smooth</button>",
            Tips: new[] {
                "transition-all animates all properties, but can be heavy—prefer specific transitions.",
                "transition-colors for background/text colors, transition-transform for scale/rotate.",
                "Combine with hover:, focus:, active: for interactive effects."
            }
        )
        {
            LongELI5 = "\n\nTransitions are like slow-motion for style changes. Instead of instant snaps, you get smooth fades and slides.\n\nWithout transition:\n```html\n<button class=\"bg-blue-600 hover:bg-blue-700\">Click</button>\n```\nResult: Color changes instantly (jarring)\n\nWith transition:\n```html\n<button class=\"bg-blue-600 hover:bg-blue-700 transition-colors\">Click</button>\n```\nResult: Color fades smoothly over ~150ms\n\nTransition types:\n\n**transition-colors** (colors only):\n```html\n<div class=\"bg-blue-500 hover:bg-blue-700 text-white hover:text-gray-100 transition-colors\">\n    Smooth color fade\n</div>\n```\n\n**transition-transform** (scale, rotate, translate):\n```html\n<img class=\"hover:scale-110 transition-transform\" />\n```\n\n**transition-opacity** (fade in/out):\n```html\n<div class=\"opacity-50 hover:opacity-100 transition-opacity\">\n    Fades in on hover\n</div>\n```\n\n**transition-all** (everything):\n```html\n<div class=\"bg-blue-500 hover:bg-blue-700 hover:shadow-lg transition-all\">\n    Animates color AND shadow\n</div>\n```\n\nWhy prefer specific transitions?\ntransition-all watches EVERY property for changes. If you only change colors, use transition-colors—it's more performant.\n\nDuration modifiers:\n- duration-75: Very fast (75ms)\n- duration-150: Default (150ms)\n- duration-300: Slow (300ms)\n- duration-500: Very slow (500ms)\n\nExample with custom duration:\n```html\n<button class=\"hover:bg-blue-700 transition-colors duration-300\">\n    Slow fade\n</button>\n```",
            ELI5Example = "// Pokemon card with smooth transitions\n<div class=\"border-2 border-gray-200 rounded-xl p-4 \n            hover:border-blue-400 hover:shadow-xl \n            transition-all duration-200 cursor-pointer\">\n    \n    <img src=\"@pokemon.SpriteUrl\" \n         class=\"hover:scale-110 transition-transform duration-300\" />\n    \n    <p class=\"text-gray-700 hover:text-blue-600 \n              transition-colors duration-150\">\n        @pokemon.Name\n    </p>\n</div>\n\n// Result: \n// - Border and shadow animate smoothly (200ms)\n// - Image scales up smoothly (300ms)\n// - Text color fades smoothly (150ms)"
        };

        yield return new TipTopic(
            Title: "String Formatting (ToString)",
            Category: "C# — Formatting",
            Type: "Method",
            ELI5: "Format numbers and values as strings with specific patterns like leading zeros or currency.",
            Example: "int id = 5;\nstring formatted = id.ToString(\"D3\");  // \"005\"",
            Tips: new[] {
                "\"D3\" pads integers with leading zeros to 3 digits: 5 becomes \"005\".",
                "\"C\" formats as currency: 1234.56 becomes \"$1,234.56\".",
                "\"N2\" formats with commas and 2 decimals: 1234.567 becomes \"1,234.57\"."
            }
        )
        {
            LongELI5 = "\n\nToString() is like a translator that converts numbers into pretty strings.\n\nCommon format patterns:\n\n**\"D\" (Decimal with leading zeros):**\n```csharp\nint id = 5;\nid.ToString(\"D3\");   // \"005\"\nid.ToString(\"D4\");   // \"0005\"\n\nint num = 25;\nnum.ToString(\"D3\");  // \"025\"\n```\nUseful for Pokemon IDs, invoice numbers, etc.\n\n**\"C\" (Currency):**\n```csharp\ndecimal price = 1234.56m;\nprice.ToString(\"C\");  // \"$1,234.56\" (US locale)\n```\n\n**\"N\" (Number with commas):**\n```csharp\nint count = 1234567;\ncount.ToString(\"N0\");  // \"1,234,567\" (no decimals)\n\ndouble value = 1234.5678;\nvalue.ToString(\"N2\");  // \"1,234.57\" (2 decimals)\n```\n\n**\"P\" (Percent):**\n```csharp\ndouble ratio = 0.75;\nratio.ToString(\"P0\");  // \"75%\" (no decimals)\nratio.ToString(\"P2\");  // \"75.00%\" (2 decimals)\n```\n\n**\"F\" (Fixed-point):**\n```csharp\ndouble value = 123.456;\nvalue.ToString(\"F2\");  // \"123.46\" (2 decimals, no commas)\n```\n\nCustom formats:\n```csharp\nint num = 5;\nnum.ToString(\"000\");    // \"005\"\nnum.ToString(\"#,##0\");  // \"5\" (commas if needed)\n\nDateTime now = DateTime.Now;\nnow.ToString(\"yyyy-MM-dd\");           // \"2025-11-15\"\nnow.ToString(\"MMM dd, yyyy\");         // \"Nov 15, 2025\"\nnow.ToString(\"hh:mm tt\");             // \"02:30 PM\"\n```\n\nPokemon example:\n```csharp\nint pokemonId = 25;  // Pikachu\nstring displayId = $\"#{pokemonId.ToString(\"D3\")}\";  // \"#025\"\n```",
            ELI5Example = "// Format Pokemon IDs with leading zeros\n@foreach (var pokemon in allPokemon)\n{\n    int id = GetPokemonId(pokemon.Url);\n    string displayId = id.ToString(\"D3\");  // 1 → \"001\", 25 → \"025\"\n    \n    <div class=\"pokemon-card\">\n        <span class=\"text-gray-500\">#{displayId}</span>\n        <p class=\"font-bold capitalize\">@pokemon.Name</p>\n    </div>\n}\n\n// Other formatting examples:\nint count = 1234567;\nstring formatted = count.ToString(\"N0\");  // \"1,234,567\"\n\ndecimal price = 19.99m;\nstring priceText = price.ToString(\"C\");   // \"$19.99\"\n\ndouble percent = 0.856;\nstring percentText = percent.ToString(\"P1\");  // \"85.6%\""
        };

        yield return new TipTopic(
            Title: "aspect-square (Tailwind)",
            Category: "Styling — CSS",
            Type: "Utility",
            ELI5: "Force an element to be perfectly square (width equals height) regardless of content.",
            Example: "<div class=\"aspect-square\">\n    <img src=\"image.png\" class=\"w-full h-full object-cover\" />\n</div>",
            Tips: new[] {
                "Useful for card grids where all items should have the same height.",
                "Combine with object-cover on images to prevent distortion.",
                "Other aspect ratios: aspect-video (16:9), aspect-[4/3] (custom)."
            }
        )
        {
            LongELI5 = "\n\naspect-square is like a magic box that always stays perfectly square no matter what you put in it.\n\nThe problem:\nWithout aspect-square, cards in a grid have different heights based on content:\n```html\n<div class=\"grid grid-cols-3 gap-4\">\n    <div class=\"border p-4\">\n        <img src=\"tall-image.png\" />  <!-- Tall card -->\n    </div>\n    <div class=\"border p-4\">\n        <img src=\"wide-image.png\" />  <!-- Short card -->\n    </div>\n</div>\n```\nResult: Ugly, uneven grid\n\nThe solution:\n```html\n<div class=\"grid grid-cols-3 gap-4\">\n    <div class=\"border p-4 aspect-square\">\n        <img src=\"any-image.png\" class=\"w-full h-full object-cover\" />\n    </div>\n    <div class=\"border p-4 aspect-square\">\n        <img src=\"any-image.png\" class=\"w-full h-full object-cover\" />\n    </div>\n</div>\n```\nResult: Perfect, even grid where every card is exactly square\n\nHow it works:\naspect-square sets aspect-ratio: 1 / 1 in CSS, which means:\n- Width = 100% of container\n- Height automatically adjusts to match width\n- Result: always a perfect square\n\nCombine with object-cover:\nobject-cover makes images fill the container without distortion:\n```html\n<div class=\"aspect-square bg-gray-100\">\n    <img src=\"pokemon.png\" class=\"w-full h-full object-cover\" />\n</div>\n```\n- Image fills the entire square\n- Excess is cropped (not stretched)\n- No white space\n\nOther aspect ratios:\n- aspect-video: 16:9 (like YouTube)\n- aspect-[4/3]: Custom 4:3 ratio\n- aspect-[21/9]: Ultra-wide\n\nPokemon card example:\n```html\n<div class=\"border rounded-lg overflow-hidden aspect-square\">\n    <img src=\"@GetSpriteUrl(pokemon.Id)\" \n         class=\"w-full h-full object-contain\" />\n</div>\n```\nobject-contain vs object-cover:\n- contain: Shows entire image, may have white space\n- cover: Fills space, may crop image",
            ELI5Example = "// Pokemon grid with consistent square cards\n<div class=\"grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4\">\n    @foreach (var pokemon in allPokemon)\n    {\n        <div class=\"border-2 border-gray-200 rounded-xl overflow-hidden \n                    hover:border-blue-400 transition-colors\">\n            \n            <!-- Square image container -->\n            <div class=\"aspect-square bg-gray-100\">\n                <img src=\"@GetSpriteUrl(pokemon.Id)\" \n                     class=\"w-full h-full object-contain\" \n                     alt=\"@pokemon.Name\" />\n            </div>\n            \n            <!-- Info section -->\n            <div class=\"p-3\">\n                <span class=\"text-xs text-gray-500\">\n                    #{pokemon.Id.ToString(\"D3\")}\n                </span>\n                <p class=\"font-semibold capitalize\">@pokemon.Name</p>\n            </div>\n        </div>\n    }\n</div>\n\n// Result: All cards have same height, perfect grid alignment"
        };
    }
}

public class Step6TipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "@bind:after",
            Category: "Blazor — Data Binding",
            Type: "Directive Attribute",
            ELI5: "Run code automatically after two-way binding updates a value.",
            Example: "@bind:after=\"OnSearchChanged\"",
            Tips: new[]
            {
                "Perfect for resetting pagination when search changes",
                "Executes after the bound value updates",
                "Can be synchronous or asynchronous method",
                "Common use: currentPage = 1 after search input changes"
            }
        )
        {
            LongELI5 = """

@bind:after is like saying "Do this extra thing whenever the value changes."

Without @bind:after, you need manual event handling:
```razor
<input @oninput="OnInput" value="@searchQuery" />

@code {
    void OnInput(ChangeEventArgs e)
    {
        searchQuery = e.Value?.ToString() ?? "";
        OnSearchChanged();  // Manual call
    }
}
```

With @bind:after, it's automatic:
```razor
<input @bind="searchQuery" @bind:event="oninput" @bind:after="OnSearchChanged" />

@code {
    string searchQuery = "";
    
    void OnSearchChanged()
    {
        currentPage = 1;  // Reset pagination automatically
    }
}
```

Common pattern for search with pagination:
```razor
<input type="text" 
       @bind="searchQuery" 
       @bind:event="oninput" 
       @bind:after="ResetPagination"
       placeholder="Search..." />

@code {
    string searchQuery = "";
    int currentPage = 1;
    
    void ResetPagination()
    {
        currentPage = 1;  // Always show page 1 when search changes
        UpdateResults();
    }
}
```

Why this matters:
When users search, they expect to see results from page 1, not stuck on page 5 of the old results. @bind:after ensures pagination resets automatically whenever the search query changes.
""",
            ELI5Example = "User types in search box → @bind updates searchQuery → @bind:after runs OnSearchChanged() → currentPage resets to 1 → shows first page of filtered results"
        };

        yield return new TipTopic(
            Title: "@bind:event",
            Category: "Blazor — Data Binding",
            Type: "Directive Attribute",
            ELI5: "Choose which event triggers two-way binding updates (oninput vs onchange).",
            Example: "@bind:event=\"oninput\"",
            Tips: new[]
            {
                "oninput: Updates as user types (real-time)",
                "onchange: Updates when input loses focus (default)",
                "Use oninput for search boxes to filter as user types",
                "Use onchange for forms to avoid excessive updates"
            }
        )
        {
            LongELI5 = """

@bind:event controls WHEN the bound value updates.

Default behavior (@bind alone uses onchange):
```razor
<input @bind="username" />
<!-- Updates when user clicks away or presses Enter -->
```

Real-time updates with oninput:
```razor
<input @bind="searchQuery" @bind:event="oninput" />
<!-- Updates as user types each character -->
```

Comparison:

**onchange** (default):
- Updates when input loses focus (blur)
- Or when user presses Enter
- Better for forms (fewer updates, better performance)
- Example: Name, email, password fields

**oninput**:
- Updates on every keystroke
- Perfect for search/filter functionality
- Real-time character counter
- Live validation feedback

Search box example:
```razor
<input type="text" 
       @bind="searchQuery" 
       @bind:event="oninput"
       placeholder="Search Pokemon..." />

<p>Found: @filteredResults.Count results</p>

@code {
    string searchQuery = "";
    
    List<Pokemon> filteredResults => allPokemon
        .Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
        .ToList();
}
```

Users type "char" and see results update in real-time:
- "c" → Shows Charizard, Charmander, etc.
- "ch" → Narrows to Charizard, Charmander
- "cha" → Shows Charizard, Charmander, Chansey

Form field example (uses default onchange):
```razor
<input @bind="email" type="email" />
<!-- Only validates when user finishes typing and clicks away -->
```

Rule of thumb: Use oninput for search/filter, use onchange (default) for form fields.
""",
            ELI5Example = "Search box with @bind:event=\"oninput\" updates results instantly as user types 'pika' → Pikachu appears. Without it, results only update when user clicks away."
        };

        yield return new TipTopic(
            Title: "@onclick:stopPropagation",
            Category: "Blazor — Event Handling",
            Type: "Event Modifier",
            ELI5: "Prevent clicks from bubbling up to parent elements.",
            Example: "@onclick:stopPropagation",
            Tips: new[]
            {
                "Essential for modal overlays (click modal content, don't close)",
                "Stops event from reaching parent elements",
                "Prevents accidental closes when clicking inside modal",
                "Use on modal content div, not the overlay"
            }
        )
        {
            LongELI5 = """

@onclick:stopPropagation stops click events from "bubbling up" to parent elements.

Modal pattern WITHOUT stopPropagation (broken):
```razor
<!-- Clicking ANYWHERE closes the modal, even inside the content -->
<div @onclick="CloseModal" class="fixed inset-0 bg-black/50">
    <div class="bg-white p-6 rounded-lg">
        <h2>Pokemon Details</h2>
        <p>Click me closes the modal! ❌</p>
    </div>
</div>
```

Modal pattern WITH stopPropagation (correct):
```razor
<!-- Click overlay = close, click content = stays open -->
<div @onclick="CloseModal" class="fixed inset-0 bg-black/50">
    <div @onclick:stopPropagation class="bg-white p-6 rounded-lg">
        <h2>Pokemon Details</h2>
        <p>Click me keeps modal open! ✅</p>
        <button @onclick="CloseModal">Close</button>
    </div>
</div>
```

How event bubbling works:
1. User clicks inner div
2. Event fires on inner div
3. Event "bubbles up" to parent overlay div
4. Parent's @onclick="CloseModal" fires
5. Modal closes (even though user clicked content!)

stopPropagation stops step 3, preventing unwanted behavior.

Real-world modal example:
```razor
@if (showModal)
{
    <!-- Overlay: Click to close -->
    <div @onclick="CloseModal" 
         class="fixed inset-0 bg-black/50 flex items-center justify-center">
        
        <!-- Content: Click does nothing (stopPropagation) -->
        <div @onclick:stopPropagation 
             class="bg-white rounded-xl p-6 max-w-md">
            
            <h2>@selectedPokemon.Name</h2>
            <img src="@selectedPokemon.SpriteUrl" />
            
            <!-- Explicit close button -->
            <button @onclick="CloseModal" 
                    class="mt-4 px-4 py-2 bg-blue-600 text-white rounded">
                Close
            </button>
        </div>
    </div>
}
```

User experience:
- Click outside modal (overlay) → Closes ✅
- Click inside modal (content) → Stays open ✅
- Click Close button → Closes ✅

Other event modifiers:
- @onclick:preventDefault → Stops default behavior (like form submit)
- @onclick:stopPropagation → Stops event bubbling (this tip)
""",
            ELI5Example = "Modal overlay: clicking outside closes modal. Modal content with stopPropagation: clicking inside keeps modal open. Without it, clicking anywhere closes the modal."
        };

        yield return new TipTopic(
            Title: "String.Contains (Case-Insensitive)",
            Category: "C# — String Methods",
            Type: "Method",
            ELI5: "Search for text inside a string, ignoring uppercase/lowercase differences.",
            Example: "name.Contains(search, StringComparison.OrdinalIgnoreCase)",
            Tips: new[]
            {
                "Perfect for search functionality (finds 'PIKACHU', 'pikachu', 'Pikachu')",
                "Use StringComparison.OrdinalIgnoreCase for case-insensitive search",
                "Default Contains() is case-sensitive ('pikachu' ≠ 'Pikachu')",
                "Combine with LINQ Where() to filter collections"
            }
        )
        {
            LongELI5 = """

String.Contains checks if one string exists inside another.

Case-sensitive (default):
```csharp
string name = "Pikachu";
bool found = name.Contains("pika");  // false ❌
```

Case-insensitive (proper search):
```csharp
string name = "Pikachu";
bool found = name.Contains("pika", StringComparison.OrdinalIgnoreCase);  // true ✅
```

Real-world search implementation:
```csharp
List<Pokemon> allPokemon = GetAllPokemon();
string searchQuery = "char";

// Filter Pokemon whose names contain search query (case-insensitive)
var results = allPokemon
    .Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
    .ToList();

// Finds: Charizard, Charmander, Charmeleon
```

Why OrdinalIgnoreCase matters for search:

Without it (case-sensitive):
- User searches "pikachu" → Finds nothing (Pokemon name is "Pikachu")
- User must type exact case "Pikachu" → Annoying!

With it (case-insensitive):
- User searches "pikachu" → Finds "Pikachu" ✅
- User searches "PIKACHU" → Finds "Pikachu" ✅
- User searches "PiKaChU" → Finds "Pikachu" ✅

Complete search component example:
```razor
<input @bind="searchQuery" 
       @bind:event="oninput" 
       placeholder="Search Pokemon..." />

<p>Found: @FilteredPokemon.Count Pokemon</p>

@foreach (var pokemon in FilteredPokemon)
{
    <div>@pokemon.Name</div>
}

@code {
    string searchQuery = "";
    List<Pokemon> allPokemon = new();
    
    List<Pokemon> FilteredPokemon => string.IsNullOrWhiteSpace(searchQuery)
        ? allPokemon
        : allPokemon
            .Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
}
```

User searches "chu":
- Finds: Pikachu, Raichu, Pichu
- Case doesn't matter: "CHU", "chu", "Chu" all work

StringComparison options:
- OrdinalIgnoreCase: Fast, culture-insensitive, perfect for search
- CurrentCultureIgnoreCase: Respects user's language/culture
- InvariantCultureIgnoreCase: Consistent across all cultures

For Pokemon search, use OrdinalIgnoreCase (fast and simple).
""",
            ELI5Example = "User searches 'pika' in search box → name.Contains('pika', OrdinalIgnoreCase) → Finds 'Pikachu', 'Pichu' regardless of how user typed it (PIKA, pika, Pika all work)"
        };

        yield return new TipTopic(
            Title: "Modal Overlay Pattern",
            Category: "Blazor — UI Patterns",
            Type: "Pattern",
            ELI5: "Create popup dialogs with dark overlay that close when clicking outside.",
            Example: "Fixed overlay + centered content + stopPropagation",
            Tips: new[]
            {
                "Use fixed inset-0 for full-screen overlay",
                "Add bg-black/50 for semi-transparent dark background",
                "Use flex items-center justify-center to center modal",
                "@onclick on overlay to close, @onclick:stopPropagation on content"
            }
        )
        {
            LongELI5 = """

Modal overlay pattern creates popup dialogs that appear on top of your app.

Complete modal structure:
```razor
@if (showModal)
{
    <!-- Overlay: Dark background covering entire screen -->
    <div @onclick="CloseModal" 
         class="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
        
        <!-- Modal content: White card in center -->
        <div @onclick:stopPropagation 
             class="bg-white rounded-xl shadow-2xl p-6 max-w-md w-full mx-4">
            
            <!-- Header -->
            <div class="flex items-center justify-between mb-4">
                <h2 class="text-xl font-bold">Pokemon Details</h2>
                <button @onclick="CloseModal" class="text-gray-500 hover:text-gray-700">
                    ✕
                </button>
            </div>
            
            <!-- Content -->
            <img src="@pokemon.Sprite" class="w-32 h-32 mx-auto" />
            <h3 class="text-center text-lg font-semibold">@pokemon.Name</h3>
            
            <!-- Footer -->
            <button @onclick="CloseModal" 
                    class="w-full mt-4 px-4 py-2 bg-blue-600 text-white rounded-lg">
                Close
            </button>
        </div>
    </div>
}

@code {
    bool showModal = false;
    Pokemon? pokemon;
    
    void OpenModal(Pokemon p)
    {
        pokemon = p;
        showModal = true;
    }
    
    void CloseModal()
    {
        showModal = false;
    }
}
```

Key Tailwind classes explained:

**Overlay**:
- `fixed`: Positioned relative to viewport (not page scroll)
- `inset-0`: Top/right/bottom/left = 0 (covers entire screen)
- `bg-black/50`: Black background at 50% opacity (semi-transparent)
- `flex items-center justify-center`: Centers the modal content
- `z-50`: High z-index to appear above everything

**Modal content**:
- `max-w-md`: Maximum width (prevents too-wide modals)
- `w-full mx-4`: Full width with horizontal margin (responsive)
- `rounded-xl shadow-2xl`: Rounded corners and strong shadow
- `@onclick:stopPropagation`: Prevents closing when clicking content

Pokemon details modal example:
```razor
@if (selectedPokemon != null)
{
    <div @onclick="@(() => selectedPokemon = null)" 
         class="fixed inset-0 bg-black/50 flex items-center justify-center p-4">
        
        <div @onclick:stopPropagation 
             class="bg-white rounded-2xl p-6 max-w-md w-full">
            
            <!-- Pokemon info -->
            <img src="@GetSpriteUrl(selectedPokemon.Id)" 
                 class="w-48 h-48 mx-auto" />
            
            <h2 class="text-2xl font-bold text-center capitalize">
                @selectedPokemon.Name
            </h2>
            
            <!-- Stats -->
            <div class="mt-4 space-y-2">
                <p><strong>Height:</strong> @(selectedPokemon.Height / 10.0)m</p>
                <p><strong>Weight:</strong> @(selectedPokemon.Weight / 10.0)kg</p>
            </div>
            
            <!-- Types -->
            <div class="flex gap-2 mt-4">
                @foreach (var type in selectedPokemon.Types)
                {
                    <span class="px-3 py-1 bg-blue-100 text-blue-700 rounded-full text-sm">
                        @type.Type.Name
                    </span>
                }
            </div>
            
            <button @onclick="@(() => selectedPokemon = null)" 
                    class="w-full mt-6 px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg">
                Close
            </button>
        </div>
    </div>
}
```

Three ways to close modal:
1. Click overlay (dark area outside modal)
2. Click X button in header
3. Click Close button at bottom

All call the same method to close!
""",
            ELI5Example = "User clicks Pokemon card → showModal = true → Dark overlay appears → White card shows Pokemon details in center → Click outside or Close button → showModal = false → Modal disappears"
        };
    }
}

// Step 1 Tips: Project Setup & Tooling
public class Step1TipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "dotnet CLI",
            Category: "Tooling — .NET",
            Type: "Tool",
            ELI5: "Command-line tool for creating, building, running, and managing .NET projects.",
            Example: "dotnet new blazor -o MyApp\ndotnet build\ndotnet run",
            Tips: new[] {
                "Use 'dotnet new list' to see all available project templates",
                "Add --help to any command for detailed usage information",
                "Flags like --interactivity and --empty customize your project setup"
            }
        )
        {
            LongELI5 = "\n\nThe dotnet CLI is your Swiss Army knife for .NET development. Instead of clicking through wizards, you type one command and boom - new project created. It handles everything: creating files, restoring packages, compiling code, running your app. Think of it like ordering food: 'dotnet new blazor' is like saying 'I'll have a Blazor Server app, please.' The CLI knows exactly what files to create, what packages to include, and how to wire everything together. Developers love it because it's fast, scriptable, and works the same on Windows, Mac, and Linux.",
            ELI5Example = "# Create new Blazor Server app with interactive mode\ndotnet new blazor -o TruckingApp --interactivity Server --all-interactive --empty\n\n# Navigate into project\ncd TruckingApp\n\n# Restore packages (usually automatic)\ndotnet restore\n\n# Build the project\ndotnet build\n\n# Run the application\ndotnet run\n\n# See what template created\ndir  # Windows\nls   # Mac/Linux"
        };

        yield return new TipTopic(
            Title: "npm packages",
            Category: "Tooling — Frontend",
            Type: "Package Manager",
            ELI5: "npm installs JavaScript libraries and tools for your web project.",
            Example: "npm install tailwindcss @tailwindcss/cli\nnpm install --save-dev typescript",
            Tips: new[] {
                "npm install adds packages to node_modules folder",
                "package.json tracks which packages your project needs",
                "Use @ symbol for scoped packages like @tailwindcss/cli"
            }
        )
        {
            LongELI5 = "\n\nnpm is like an app store for JavaScript tools. Instead of downloading files manually, you type 'npm install tailwindcss' and boom - the entire Tailwind library appears in your project. npm tracks everything in package.json (like a shopping list) and stores files in node_modules (like your pantry). When someone else clones your project, they just run 'npm install' and get the exact same packages. Scoped packages (with @) are like brand names - @tailwindcss/cli is the official Tailwind CLI tool, not a knockoff.",
            ELI5Example = "# Install Tailwind CSS v4 and its CLI\nnpm install tailwindcss @tailwindcss/cli\n\n# package.json now shows:\n# \"dependencies\": {\n#   \"tailwindcss\": \"^4.0.0\",\n#   \"@tailwindcss/cli\": \"^4.0.0\"\n# }\n\n# Files appear in node_modules/\n# node_modules/tailwindcss/\n# node_modules/@tailwindcss/cli/\n\n# Use the installed CLI\nnpx @tailwindcss/cli --help"
        };

        yield return new TipTopic(
            Title: "Tailwind CSS",
            Category: "Styling — CSS Frameworks",
            Type: "Framework",
            ELI5: "Utility-first CSS framework that lets you style elements with pre-made class names instead of writing custom CSS.",
            Example: "<div class=\"flex items-center gap-4 p-6 bg-blue-100 rounded-lg\">\n  <h1 class=\"text-2xl font-bold text-blue-900\">Hello</h1>\n</div>",
            Tips: new[] {
                "Tailwind v4 uses @import \"tailwindcss\" instead of config files",
                "Combine utility classes to build complex designs quickly",
                "Responsive: use sm:, md:, lg: prefixes for breakpoints",
                "Use --watch flag to rebuild CSS as you code"
            }
        )
        {
            LongELI5 = "\n\nTailwind CSS is like building with LEGO blocks instead of sculpting clay. Instead of writing custom CSS rules (the clay), you use pre-made utility classes (the blocks) right in your HTML. Want padding? Add 'p-4'. Want blue background? Add 'bg-blue-100'. Need responsive design? Add 'sm:text-lg md:text-xl'. Tailwind v4 is even simpler - just @import \"tailwindcss\" in your CSS file and you're ready. The --watch flag is your friend during development - it watches your files and rebuilds the CSS instantly when you change classes. No more switching between HTML and CSS files!",
            ELI5Example = "/* Styles/input.css - Tailwind v4 */\n@import \"tailwindcss\";\n\n<!-- Your component -->\n<div class=\"max-w-4xl mx-auto p-6\">\n  <h1 class=\"text-3xl font-bold text-gray-900 mb-4\">\n    Welcome\n  </h1>\n  <p class=\"text-gray-600 leading-relaxed\">\n    Built with Tailwind!\n  </p>\n  <button class=\"px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg\">\n    Click Me\n  </button>\n</div>\n\n<!-- Responsive example -->\n<div class=\"grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4\">\n  <!-- Cards here -->\n</div>"
        };

        yield return new TipTopic(
            Title: ".NET Asset Pipeline",
            Category: "Blazor — Static Files",
            Type: "System",
            ELI5: "Blazor's system for managing static files (CSS, JS, images) with fingerprinting for cache busting.",
            Example: "<link rel=\"stylesheet\" href=\"@Assets[\"tailwind.css\"]\" />\n<script src=\"@Assets[\"app.js\"]\" />",
            Tips: new[] {
                "Assets[\"file.css\"] generates URLs with hash for cache busting",
                "Put files in wwwroot folder to make them accessible",
                "Fingerprinting ensures users get fresh files after updates",
                "No need to manually version your CSS/JS files"
            }
        )
        {
            LongELI5 = "\n\nThe .NET Asset Pipeline solves a common web problem: browser caching. When you update your CSS file, browsers might keep showing the old cached version. Blazor's asset pipeline fixes this by adding a unique fingerprint (hash) to your file URLs. Instead of 'tailwind.css', it becomes 'tailwind.abc123.css' where 'abc123' changes every time the file changes. This forces browsers to download the new version. Just use @Assets[\"tailwind.css\"] in your code and Blazor handles the rest. It's like putting a version number on your files, but automatic and based on actual content changes.",
            ELI5Example = "<!-- App.razor <head> section -->\n<link rel=\"stylesheet\" href=\"@Assets[\"tailwind.css\"]\" />\n\n<!-- Blazor generates: -->\n<link rel=\"stylesheet\" href=\"/tailwind.7a8b9c.css\" />\n\n<!-- After you update tailwind.css: -->\n<link rel=\"stylesheet\" href=\"/tailwind.4d5e6f.css\" />\n\n<!-- The hash (7a8b9c → 4d5e6f) changes! -->\n<!-- Browser sees different URL → downloads fresh file -->\n<!-- Users always get your latest CSS! -->"
        };
    }
}

// Blazor component styling and patterns


// Entity Framework Core and Database topics
public class EntityFrameworkTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        yield return new TipTopic(
            Title: "POCO Models",
            Category: "Entity Framework — Models",
            Type: "Pattern",
            ELI5: "Plain Old CLR Objects - simple C# classes that represent database tables without inheriting from special base classes.",
            Example: "public class Driver\n{\n    public int Id { get; set; }\n    public string Name { get; set; } = string.Empty;\n    public decimal HourlyRate { get; set; }\n}",
            Tips: new[] {
                "Keep models simple - just properties and basic logic",
                "EF Core tracks changes automatically - no special methods needed",
                "Id property is auto-detected as primary key",
                "Use = string.Empty or required keyword to avoid null warnings"
            }
        )
        {
            LongELI5 = "\n\nPOCO stands for Plain Old CLR Object. It's a fancy way of saying 'just a normal C# class'. Unlike older frameworks that forced you to inherit from special base classes or implement interfaces, EF Core works with your plain classes. Think of it like filling out a form - you don't need special paper, regular paper works fine. Your Driver class is just properties that match database columns. EF Core is smart enough to figure out that Id is the primary key, Name is a string column, and HourlyRate is a decimal column. No magic inheritance, no special attributes required (though you can add them for validation). This makes your code cleaner and easier to test.",
            ELI5Example = "// This is a POCO - just a plain class!\npublic class Driver\n{\n    public int Id { get; set; }          // EF Core knows this is the primary key\n    public string Name { get; set; } = string.Empty;  // String column\n    public decimal HourlyRate { get; set; }           // Decimal column\n    public bool IsAvailable { get; set; }             // Boolean column\n}\n\n// NOT a POCO - has to inherit from special base class (old way)\npublic class OldDriver : EntityBase  // ❌ Don't do this\n{\n    // Same properties...\n}"
        };

        yield return new TipTopic(
            Title: "Data Annotations",
            Category: "Entity Framework — Validation",
            Type: "Attribute",
            ELI5: "Attributes like [Required] and [StringLength] that add validation rules and configure database columns.",
            Example: "[Required]\n[StringLength(100)]\npublic string Name { get; set; } = string.Empty;",
            Tips: new[] {
                "Validates data before saving to database",
                "Works with Blazor forms automatically",
                "Common ones: [Required], [StringLength], [Range], [EmailAddress]",
                "Generates database constraints (NOT NULL, VARCHAR length)"
            }
        )
        {
            LongELI5 = "\n\nData annotations are like putting rules on a form field. [Required] means 'this field cannot be empty' - like when a website won't let you submit without filling in your name. [StringLength(100)] means 'this text can be at most 100 characters' - like a tweet limit but for database fields. These rules work two ways: they validate user input in your Blazor forms (showing error messages), AND they create database constraints (making the database enforce the rules too). It's like having a bouncer at two doors - one checks your form before it leaves the browser, another checks at the database door. This prevents bad data from getting into your database.",
            ELI5Example = "public class Driver\n{\n    public int Id { get; set; }\n\n    [Required]                    // Cannot be null or empty\n    [StringLength(100)]           // Max 100 characters → VARCHAR(100)\n    public string Name { get; set; } = string.Empty;\n\n    [Required]\n    [StringLength(20)]\n    public string LicenseNumber { get; set; } = string.Empty;\n\n    [Range(0, 50)]                // Must be between 0 and 50\n    public int YearsOfExperience { get; set; }\n\n    [Range(0, 200)]\n    public decimal HourlyRate { get; set; }\n}\n\n// Blazor automatically shows validation errors:\n// <ValidationMessage For=\"() => driver.Name\" />"
        };

        yield return new TipTopic(
            Title: "DbContext",
            Category: "Entity Framework — Core Concepts",
            Type: "Class",
            ELI5: "The main class that manages database connections and provides access to your tables through DbSet properties.",
            Example: "public class AppDbContext : DbContext\n{\n    public DbSet<Driver> Drivers { get; set; }\n    public DbSet<Route> Routes { get; set; }\n}",
            Tips: new[] {
                "Inject into components - don't create instances yourself",
                "Scoped lifetime - one instance per request",
                "Tracks entity changes automatically",
                "Call SaveChangesAsync() to commit changes to database"
            }
        )
        {
            LongELI5 = "\n\nDbContext is like your database receptionist. You tell it what you want (get all drivers, save this new route) and it handles the actual database work. Think of DbSet properties as filing cabinets - Drivers is the cabinet holding all driver records, Routes holds all route records. The DbContext manages connections, tracks changes you make, and generates SQL commands. When you modify a driver's name, DbContext remembers that change. When you call SaveChangesAsync(), it sends an UPDATE statement to the database. In Blazor, you inject DbContext into your components (don't new it up) so ASP.NET can manage the database connection properly.",
            ELI5Example = "@inject AppDbContext DbContext\n\n@code {\n    private List<Driver> drivers;\n\n    protected override async Task OnInitializedAsync()\n    {\n        // Load all drivers from database\n        drivers = await DbContext.Drivers.ToListAsync();\n    }\n\n    private async Task UpdateDriver(Driver driver)\n    {\n        // Modify the driver\n        driver.HourlyRate = 45.00m;\n        \n        // DbContext tracked the change!\n        // Just call SaveChangesAsync to update database\n        await DbContext.SaveChangesAsync();\n    }\n}"
        };

        yield return new TipTopic(
            Title: "DbSet<T>",
            Category: "Entity Framework — Core Concepts",
            Type: "Property",
            ELI5: "A property on DbContext that represents a table in your database and lets you query, add, update, or delete records.",
            Example: "public DbSet<Driver> Drivers { get; set; }\n// Query: await DbContext.Drivers.ToListAsync()\n// Add: DbContext.Drivers.Add(newDriver)",
            Tips: new[] {
                "Each DbSet<T> maps to a database table",
                "Use ToListAsync(), FindAsync() to query data",
                "Use Add(), Remove() to modify data",
                "Changes aren't saved until you call SaveChangesAsync()"
            }
        )
        {
            LongELI5 = "\n\nDbSet<T> is like a collection that's connected to a database table. DbSet<Driver> represents your Drivers table. You can treat it like a list - loop through it with foreach, filter it with LINQ (Where, Skip, Take), but behind the scenes it's querying the real database. When you call DbContext.Drivers.ToListAsync(), EF Core generates a SQL SELECT statement and loads the data. When you call DbContext.Drivers.Add(newDriver), it marks that driver for insertion, but doesn't actually insert until you call SaveChangesAsync(). Think of it as a shopping cart - you add items (Add), remove items (Remove), but the purchase doesn't happen until checkout (SaveChangesAsync).",
            ELI5Example = "public class AppDbContext : DbContext\n{\n    // Each DbSet = one table\n    public DbSet<Driver> Drivers { get; set; }\n    public DbSet<Route> Routes { get; set; }\n    public DbSet<Truck> Trucks { get; set; }\n}\n\n// Usage in component:\nprotected override async Task OnInitializedAsync()\n{\n    // Query the Drivers table\n    var allDrivers = await DbContext.Drivers.ToListAsync();\n    \n    // Filter with LINQ - generates SQL WHERE clause\n    var available = await DbContext.Drivers\n        .Where(d => d.IsAvailable)\n        .ToListAsync();\n}\n\nprivate async Task AddNewDriver()\n{\n    var driver = new Driver { Name = \"John\" };\n    DbContext.Drivers.Add(driver);  // Staged for insert\n    await DbContext.SaveChangesAsync();  // Actually inserts\n}"
        };

        yield return new TipTopic(
            Title: "EF Core Migrations",
            Category: "Entity Framework — Database Schema",
            Type: "Tool",
            ELI5: "Version control for your database schema - tracks changes to tables and columns over time.",
            Example: "dotnet ef migrations add InitialCreate\ndotnet ef database update",
            Tips: new[] {
                "Run 'dotnet ef migrations add <Name>' after changing models",
                "Run 'dotnet ef database update' to apply changes to database",
                "Each migration is a snapshot of schema changes",
                "Don't edit old migrations - create new ones for changes"
            }
        )
        {
            LongELI5 = "\n\nMigrations are like Git for your database structure. When you add a new property to your Driver model, the database table needs a new column. Instead of manually writing SQL, you create a migration that describes the change. 'dotnet ef migrations add AddPhoneNumber' creates a file with code to add the column (Up method) and remove it (Down method). Then 'dotnet ef database update' applies that change to your actual database. If you later deploy to production, you run the same migrations there to keep databases in sync. Each migration builds on the previous ones, like Git commits. This means your team can track how the database evolved and easily recreate it from scratch.",
            ELI5Example = "// 1. Add property to model\npublic class Driver\n{\n    public int Id { get; set; }\n    public string Name { get; set; }\n    public string PhoneNumber { get; set; }  // NEW!\n}\n\n// 2. Create migration\n// dotnet ef migrations add AddPhoneNumber\n// → Creates file: Migrations/20241115_AddPhoneNumber.cs\n\npublic partial class AddPhoneNumber : Migration\n{\n    protected override void Up(MigrationBuilder migrationBuilder)\n    {\n        migrationBuilder.AddColumn<string>(\n            name: \"PhoneNumber\",\n            table: \"Drivers\",\n            nullable: true);\n    }\n    \n    protected override void Down(MigrationBuilder migrationBuilder)\n    {\n        migrationBuilder.DropColumn(\n            name: \"PhoneNumber\",\n            table: \"Drivers\");\n    }\n}\n\n// 3. Apply to database\n// dotnet ef database update\n// → Adds PhoneNumber column to Drivers table"
        };

        yield return new TipTopic(
            Title: "ToListAsync",
            Category: "Entity Framework — Queries",
            Type: "Method",
            ELI5: "Executes a query and loads all results into a List<T> without blocking the UI thread.",
            Example: "var drivers = await DbContext.Drivers.ToListAsync();",
            Tips: new[] {
                "Always use async version in Blazor - never ToList()",
                "Loads all results into memory - be careful with large tables",
                "Use Where() before ToListAsync() to filter data",
                "Triggers actual database query execution"
            }
        )
        {
            LongELI5 = "\n\nToListAsync() is the moment your LINQ query actually runs against the database. Before you call it, you're just building a query (like writing a shopping list). When you call ToListAsync(), EF Core converts your LINQ to SQL, sends it to the database, waits for results, and loads everything into a C# List. The 'Async' part means it doesn't freeze your UI while waiting - your Blazor component can show a loading spinner. Think of it like ordering food: you place the order (write LINQ), then await the delivery (ToListAsync), but you can do other things while waiting. Always use the Async version in Blazor, or you'll freeze the browser.",
            ELI5Example = "protected override async Task OnInitializedAsync()\n{\n    // ❌ BAD - blocks UI thread\n    // var drivers = DbContext.Drivers.ToList();\n    \n    // ✅ GOOD - async, doesn't block\n    var drivers = await DbContext.Drivers.ToListAsync();\n    \n    // Filter before loading to reduce data\n    var available = await DbContext.Drivers\n        .Where(d => d.IsAvailable)        // SQL WHERE clause\n        .OrderBy(d => d.Name)              // SQL ORDER BY\n        .ToListAsync();                    // Execute query\n    \n    // For large tables, use pagination\n    var page1 = await DbContext.Drivers\n        .Skip(0)\n        .Take(10)\n        .ToListAsync();\n}"
        };

        yield return new TipTopic(
            Title: "SaveChangesAsync",
            Category: "Entity Framework — Persistence",
            Type: "Method",
            ELI5: "Commits all pending changes (inserts, updates, deletes) to the database in a single transaction.",
            Example: "DbContext.Drivers.Add(newDriver);\nawait DbContext.SaveChangesAsync();",
            Tips: new[] {
                "Call after Add(), Remove(), or modifying entities",
                "All changes happen in one transaction - all or nothing",
                "Returns number of affected rows",
                "Throws exception if database constraints violated"
            }
        )
        {
            LongELI5 = "\n\nSaveChangesAsync() is the checkout button for your database shopping cart. You can Add() new entities, Remove() old ones, and modify properties, but nothing actually happens in the database until you call SaveChangesAsync(). It's like editing a document - you make changes, but they're not saved until you hit the Save button. EF Core tracks everything you changed, then generates the right SQL commands (INSERT, UPDATE, DELETE) and sends them all at once in a transaction. If anything fails (like violating a unique constraint), nothing gets saved - it rolls back everything. Always await it to handle errors and get the count of rows affected.",
            ELI5Example = "private async Task CreateDriver()\n{\n    var driver = new Driver\n    {\n        Name = \"John Doe\",\n        LicenseNumber = \"CDL-12345\",\n        HourlyRate = 45.00m\n    };\n    \n    // Mark for insertion\n    DbContext.Drivers.Add(driver);\n    \n    // Actually insert into database\n    await DbContext.SaveChangesAsync();\n    // Now driver.Id has the generated primary key value!\n}\n\nprivate async Task UpdateDriver(Driver driver)\n{\n    // Just modify properties - EF tracks changes\n    driver.HourlyRate = 50.00m;\n    driver.IsAvailable = false;\n    \n    // Save changes to database\n    await DbContext.SaveChangesAsync();\n}\n\nprivate async Task DeleteDriver(int id)\n{\n    var driver = await DbContext.Drivers.FindAsync(id);\n    if (driver != null)\n    {\n        DbContext.Drivers.Remove(driver);\n        await DbContext.SaveChangesAsync();\n    }\n}"
        };

        yield return new TipTopic(
            Title: "FindAsync",
            Category: "Entity Framework — Queries",
            Type: "Method",
            ELI5: "Finds an entity by its primary key value, returning null if not found.",
            Example: "var driver = await DbContext.Drivers.FindAsync(id);",
            Tips: new[] {
                "Fastest way to get single entity by ID",
                "Returns null if not found - always check before using",
                "Checks in-memory tracked entities first, then database",
                "Use for Edit/Delete operations where you have the ID"
            }
        )
        {
            LongELI5 = "\n\nFindAsync() is like looking up someone in a phone book by their phone number (the ID). You give it a primary key value, and it returns that exact entity or null if it doesn't exist. It's optimized - it first checks if EF Core already loaded that entity earlier (in-memory cache), and only hits the database if needed. This makes it very fast for Edit/Delete scenarios where you already know the ID. Always check if the result is null before using it, because if that ID doesn't exist in the database, you'll get null back and trying to use it will crash with a NullReferenceException.",
            ELI5Example = "// Edit form - need to load existing driver\n[Parameter]\npublic int DriverId { get; set; }\n\nprivate Driver? driver;\n\nprotected override async Task OnInitializedAsync()\n{\n    // Load driver by ID\n    driver = await DbContext.Drivers.FindAsync(DriverId);\n    \n    if (driver == null)\n    {\n        // Not found - maybe deleted by someone else\n        Navigation.NavigateTo(\"/drivers\");\n        return;\n    }\n}\n\nprivate async Task DeleteDriver(int id)\n{\n    var driver = await DbContext.Drivers.FindAsync(id);\n    \n    // Always check for null!\n    if (driver != null)\n    {\n        DbContext.Drivers.Remove(driver);\n        await DbContext.SaveChangesAsync();\n    }\n}"
        };

        yield return new TipTopic(
            Title: "Navigation Properties",
            Category: "Entity Framework — Relationships",
            Type: "Property",
            ELI5: "Properties that link related entities together, like a Driver having many Routes or a Route belonging to one Driver.",
            Example: "public class Driver\n{\n    public int Id { get; set; }\n    public List<Route> Routes { get; set; }\n}\n\npublic class Route\n{\n    public int DriverId { get; set; }\n    public Driver Driver { get; set; }\n}",
            Tips: new[] {
                "Use List<T> for one-to-many (Driver has many Routes)",
                "Use single property for many-to-one (Route has one Driver)",
                "EF Core creates foreign key columns automatically",
                "Use Include() to load related data (eager loading)"
            }
        )
        {
            LongELI5 = "\n\nNavigation properties create relationships between your entities, just like tables are related in a database. Think of it like a family tree - a parent (Driver) can have multiple children (Routes), and each child knows who their parent is. When you put 'public List<Route> Routes' on Driver, you're saying 'a driver can have many routes'. When you put 'public Driver Driver' on Route, you're saying 'a route belongs to one driver'. EF Core uses these properties to create foreign key columns (like DriverId on the Routes table). You can then navigate from a driver to see all their routes, or from a route to see which driver owns it. Use Include() to load related data in one query, or EF Core will lazy-load it later.",
            ELI5Example = "// One-to-Many: Driver has many Routes\npublic class Driver\n{\n    public int Id { get; set; }\n    public string Name { get; set; }\n    \n    // Navigation property - collection for \"many\"\n    public List<Route> Routes { get; set; } = new();\n}\n\npublic class Route\n{\n    public int Id { get; set; }\n    public string Name { get; set; }\n    \n    // Foreign key\n    public int DriverId { get; set; }\n    \n    // Navigation property - single for \"one\"\n    public Driver Driver { get; set; } = null!;\n}\n\n// Usage - load driver with their routes\nvar driver = await DbContext.Drivers\n    .Include(d => d.Routes)  // Eager load routes\n    .FirstOrDefaultAsync(d => d.Id == driverId);\n\n// Now can access: driver.Routes (List of Route objects)\n// Or from a route: route.Driver (the Driver object)"
        };
    }
}

// Trucking Tutorial Tips
public class TruckingTutorialTipsContributor : ITipsContributor
{
    public IEnumerable<TipTopic> GetTopics()
    {
        // Step 1 tips
        yield return new TipTopic(
            Title: "npm packages",
            Category: "Tooling — Frontend",
            Type: "Package Manager",
            ELI5: "npm manages JavaScript libraries and tools for your project, like a shopping list that automatically downloads what you need.",
            Example: "npm install @tailwindcss/cli\nnpm run build:css",
            Tips: new[]
            {
                "Use package.json to track dependencies.",
                "npm install downloads packages to node_modules/.",
                "Scripts in package.json let you run common commands."
            }
        )
        {
            LongELI5 = "\n\nnpm (Node Package Manager) is like a universal parts catalog for web development. Instead of manually downloading CSS frameworks or build tools, you tell npm what you need and it fetches everything. The package.json file is your shopping list—it lists all the packages your project depends on, their versions, and custom scripts you can run. When you run 'npm install', npm reads package.json and downloads all listed packages into a folder called node_modules. You can then use these packages in your project.",
            ELI5Example = "// package.json\n{\n  \"scripts\": {\n    \"build:css\": \"npx tailwindcss -i Styles/input.css -o wwwroot/tailwind.css\"\n  },\n  \"dependencies\": {\n    \"@tailwindcss/cli\": \"^4.0.0\"\n  }\n}\n\n// Run scripts with: npm run build:css"
        };

        yield return new TipTopic(
            Title: "Tailwind CSS",
            Category: "Styling — CSS",
            Type: "Framework",
            ELI5: "A utility-first CSS framework where you build designs by combining small, single-purpose classes directly in your HTML.",
            Example: "<div class=\"bg-blue-500 text-white px-4 py-2 rounded\">\n  Click me\n</div>",
            Tips: new[]
            {
                "Use responsive prefixes like sm:, md:, lg: for breakpoints.",
                "Combine utilities to create complex designs without custom CSS.",
                "Configure colors, spacing, and fonts in tailwind.config.js."
            }
        )
        {
            LongELI5 = "\n\nTailwind CSS is like building with LEGO blocks instead of sculpting clay. Traditional CSS is like sculpting—you write custom rules for each element. Tailwind gives you pre-made utility classes (blocks) that do one thing well: 'bg-blue-500' makes the background blue, 'px-4' adds horizontal padding, 'rounded' makes corners rounded. You compose these utilities directly in your HTML to build any design. This approach is faster, more consistent, and easier to maintain than writing custom CSS. Tailwind's build process removes unused classes, keeping your final CSS file small.",
            ELI5Example = "<!-- Traditional CSS -->\n<style>\n  .my-button { background: blue; color: white; padding: 0.5rem 1rem; border-radius: 0.25rem; }\n</style>\n<button class=\"my-button\">Click</button>\n\n<!-- Tailwind CSS -->\n<button class=\"bg-blue-500 text-white px-4 py-2 rounded\">Click</button>\n\n<!-- Responsive design -->\n<div class=\"text-sm md:text-base lg:text-lg\">\n  Responsive text\n</div>"
        };

        yield return new TipTopic(
            Title: ".NET Asset Pipeline",
            Category: "Blazor — Assets",
            Type: "Build System",
            ELI5: "The system that processes static files (CSS, JS, images) and makes them available to your Blazor app at runtime.",
            Example: "// Program.cs\napp.MapStaticAssets();\n\n// In Razor\n<link rel=\"stylesheet\" href=\"app.css\" />",
            Tips: new[]
            {
                "Place static files in wwwroot/ folder.",
                "Use MapStaticAssets() to serve files efficiently.",
                "Files are served from the root URL path automatically."
            }
        )
        {
            LongELI5 = "\n\nThe .NET asset pipeline is like a conveyor belt that takes your static files (CSS, JavaScript, images) from the wwwroot folder and delivers them to users' browsers. When you put files in wwwroot/, .NET automatically makes them available at the root URL. For example, wwwroot/app.css becomes available at /app.css. The MapStaticAssets() method in Program.cs enables this pipeline with optimizations like caching headers and compression. You can reference these files in your Razor pages using relative paths.",
            ELI5Example = "// File structure:\n// wwwroot/\n//   ├── app.css\n//   ├── tailwind.css\n//   └── js/\n//       └── site.js\n\n// Program.cs\napp.MapStaticAssets();  // Enable asset serving\n\n// App.razor\n<link rel=\"stylesheet\" href=\"app.css\" />\n<link rel=\"stylesheet\" href=\"tailwind.css\" />\n<script src=\"js/site.js\"></script>"
        };

        // Step 7 tips
        yield return new TipTopic(
            Title: "POCO Models",
            Category: "C# — Data Modeling",
            Type: "Pattern",
            ELI5: "Plain Old CLR Objects—simple C# classes with properties, no special base classes or interfaces required.",
            Example: "public class Driver\n{\n    public int Id { get; set; }\n    public string Name { get; set; } = string.Empty;\n}",
            Tips: new[]
            {
                "Keep models focused on data, not behavior.",
                "Use init accessors for immutable properties.",
                "Data Annotations add validation metadata."
            }
        )
        {
            LongELI5 = "\n\nPOCO (Plain Old CLR Object) means your model classes are just regular C# classes—no inheritance from framework types, no special interfaces. They're like simple containers for data. This makes them easy to test, serialize, and use with different frameworks. EF Core works great with POCOs: you define your properties, and EF Core figures out how to map them to database columns. The 'plain' part means they don't know about databases or frameworks; they just hold data.",
            ELI5Example = "// POCO - just properties, no framework dependencies\npublic class Truck\n{\n    public int Id { get; set; }\n    public string LicensePlate { get; set; } = string.Empty;\n    public int Capacity { get; set; }\n    public bool IsAvailable { get; set; } = true;\n}\n\n// EF Core maps this to a database table automatically\n// No base class needed, no special attributes required (though you can add them)"
        };

        yield return new TipTopic(
            Title: "Data Annotations",
            Category: "C# — Data Modeling",
            Type: "Attributes",
            ELI5: "Attributes like [Required] and [MaxLength] that add validation rules and metadata to model properties.",
            Example: "public class Driver\n{\n    [Required]\n    [StringLength(100)]\n    public string Name { get; set; } = string.Empty;\n}",
            Tips: new[]
            {
                "Use [Required] for mandatory fields.",
                "[StringLength] limits text length and creates database constraints.",
                "[Range] validates numeric values."
            }
        )
        {
            LongELI5 = "\n\nData Annotations are like stickers you put on your model properties to say 'this field is required' or 'this text can't be longer than 100 characters'. They serve two purposes: validation (Blazor checks them when forms are submitted) and database schema (EF Core uses them to create appropriate column constraints). When you mark a property with [Required], Blazor won't let the form submit if it's empty, and EF Core creates a NOT NULL column. [StringLength(100)] limits input length and creates a VARCHAR(100) column.",
            ELI5Example = "public class Driver\n{\n    public int Id { get; set; }\n    \n    [Required(ErrorMessage = \"Name is required\")]\n    [StringLength(100, MinimumLength = 2)]\n    public string Name { get; set; } = string.Empty;\n    \n    [Range(0, 40, ErrorMessage = \"Experience must be between 0 and 40 years\")]\n    public int YearsExperience { get; set; }\n    \n    [EmailAddress]\n    public string Email { get; set; } = string.Empty;\n}"
        };

        // Step 8 tips
        yield return new TipTopic(
            Title: "DbContext",
            Category: "Blazor — Database",
            Type: "Class",
            ELI5: "Your gateway to the database—it manages connections, tracks changes, and saves data.",
            Example: "public class AppDbContext : DbContext\n{\n    public DbSet<Driver> Drivers { get; set; }\n    public DbSet<Truck> Trucks { get; set; }\n}",
            Tips: new[]
            {
                "Inject DbContext into components or services.",
                "Use async methods like SaveChangesAsync() for performance.",
                "DbContext tracks changes automatically—just modify entities and call SaveChanges()."
            }
        )
        {
            LongELI5 = "\n\nDbContext is like a project manager for your database. It handles all the messy details: opening connections, translating C# code to SQL, tracking what changed, and saving everything back. You define DbSet<T> properties for each table, and DbContext provides LINQ queries to work with them. When you modify an entity, DbContext remembers what changed. When you call SaveChangesAsync(), it generates the appropriate INSERT, UPDATE, or DELETE statements. It's your one-stop shop for database operations.",
            ELI5Example = "public class AppDbContext : DbContext\n{\n    public DbSet<Driver> Drivers { get; set; }\n    public DbSet<Truck> Trucks { get; set; }\n    \n    protected override void OnConfiguring(DbContextOptionsBuilder options)\n        => options.UseSqlite(\"Data Source=app.db\");\n}\n\n// Usage\nvar driver = await DbContext.Drivers.FindAsync(id);\ndriver.Name = \"Updated Name\";\nawait DbContext.SaveChangesAsync();  // Saves the change"
        };

        yield return new TipTopic(
            Title: "DbSet<T>",
            Category: "Blazor — Database",
            Type: "Property",
            ELI5: "A property on DbContext that represents a table—use it to query, add, or remove entities.",
            Example: "public DbSet<Driver> Drivers { get; set; }\n\n// Query\nvar list = await DbContext.Drivers.ToListAsync();",
            Tips: new[]
            {
                "Each DbSet<T> maps to a database table.",
                "Use LINQ to query: Where(), OrderBy(), Include().",
                "Add entities with .Add(), remove with .Remove()."
            }
        )
        {
            LongELI5 = "\n\nDbSet<T> is like a folder that represents one table in your database. If you have a Drivers table, you create a DbSet<Driver> property. You then use LINQ to query this 'folder': DbContext.Drivers.Where(d => d.Name.Contains(\"Smith\")) translates to SQL. You can add new entities (DbContext.Drivers.Add(newDriver)), remove them (DbContext.Drivers.Remove(driver)), or modify them (just change properties). DbSet<T> is your interface to a specific table.",
            ELI5Example = "// In DbContext\npublic DbSet<Driver> Drivers { get; set; }\npublic DbSet<Route> Routes { get; set; }\n\n// Usage\n// Query all\nvar all = await DbContext.Drivers.ToListAsync();\n\n// Query with filter\nvar experienced = await DbContext.Drivers\n    .Where(d => d.YearsExperience > 5)\n    .ToListAsync();\n\n// Add new\nvar newDriver = new Driver { Name = \"John\" };\nDbContext.Drivers.Add(newDriver);\nawait DbContext.SaveChangesAsync();"
        };

        yield return new TipTopic(
            Title: "EF Core Migrations",
            Category: "Blazor — Database",
            Type: "Tool",
            ELI5: "Version control for your database schema—create and apply changes to keep the database in sync with your models.",
            Example: "dotnet ef migrations add InitialCreate\ndotnet ef database update",
            Tips: new[]
            {
                "Run 'add migration' after changing models.",
                "Use 'database update' to apply migrations.",
                "Migrations are code—check them into source control."
            }
        )
        {
            LongELI5 = "\n\nMigrations are like Git commits for your database structure. When you change your C# models (add a property, change a type), you create a migration that describes how to update the database to match. 'dotnet ef migrations add CreateDriverTable' generates code that says 'add a Drivers table with these columns'. 'dotnet ef database update' runs that code against your actual database. This way, your database evolves with your code, and you have a history of all schema changes. Migrations are reversible—you can roll back if needed.",
            ELI5Example = "// 1. Create models\npublic class Driver { public int Id { get; set; } public string Name { get; set; } }\n\n// 2. Create migration\n// dotnet ef migrations add InitialCreate\n// → Generates Migrations/XXX_InitialCreate.cs with Up() and Down() methods\n\n// 3. Apply to database\n// dotnet ef database update\n// → Creates Drivers table in database\n\n// 4. Add property to model\npublic int YearsExperience { get; set; }\n\n// 5. Create migration for change\n// dotnet ef migrations add AddExperience\n\n// 6. Apply\n// dotnet ef database update\n// → Adds YearsExperience column"
        };

        // Step 9 tips
        yield return new TipTopic(
            Title: "ToListAsync",
            Category: "Blazor — Database",
            Type: "Method",
            ELI5: "Executes a LINQ query and returns all results as a List<T> asynchronously.",
            Example: "var drivers = await DbContext.Drivers.ToListAsync();",
            Tips: new[]
            {
                "Always use async methods in Blazor components.",
                "ToListAsync() triggers query execution—LINQ is lazy until then.",
                "Use Where(), OrderBy() before ToListAsync() to filter/sort."
            }
        )
        {
            LongELI5 = "\n\nToListAsync() is like pressing 'Execute' on your database query. LINQ queries are lazy—they don't run until you ask for results. When you write DbContext.Drivers.Where(d => d.Name.Contains(\"John\")), nothing happens yet. When you add .ToListAsync(), EF Core translates your LINQ to SQL, sends it to the database, and brings back all matching rows as a List<Driver>. The 'Async' part means it doesn't block your UI while waiting for the database.",
            ELI5Example = "// Query is built but not executed\nvar query = DbContext.Drivers\n    .Where(d => d.YearsExperience > 5)\n    .OrderBy(d => d.Name);\n\n// Now execute and get results\nvar results = await query.ToListAsync();\n\n// Can also chain in one statement\nvar drivers = await DbContext.Drivers\n    .Where(d => d.IsActive)\n    .OrderByDescending(d => d.HireDate)\n    .ToListAsync();"
        };

        yield return new TipTopic(
            Title: "FindAsync",
            Category: "Blazor — Database",
            Type: "Method",
            ELI5: "Finds a single entity by its primary key value, checking the cache first before querying the database.",
            Example: "var driver = await DbContext.Drivers.FindAsync(driverId);",
            Tips: new[]
            {
                "FindAsync checks the DbContext cache before querying.",
                "Returns null if not found.",
                "Use FirstOrDefaultAsync for complex queries."
            }
        )
        {
            LongELI5 = "\n\nFindAsync is like checking your desk drawer before walking to the filing cabinet. When you ask for an entity by ID, DbContext first checks if it already loaded that entity earlier (the cache). If yes, it returns it instantly without hitting the database. If no, it queries the database. This is efficient for common scenarios like loading a detail page. If the entity isn't found, FindAsync returns null. For queries more complex than 'find by ID', use FirstOrDefaultAsync with Where clauses.",
            ELI5Example = "// Find by primary key (efficient—uses cache)\nvar driver = await DbContext.Drivers.FindAsync(5);\nif (driver == null) { /* not found */ }\n\n// For complex queries, use Where + FirstOrDefaultAsync\nvar driver = await DbContext.Drivers\n    .Where(d => d.Email == email)\n    .FirstOrDefaultAsync();\n\n// Composite keys\nvar route = await DbContext.RouteAssignments.FindAsync(driverId, truckId);"
        };

        yield return new TipTopic(
            Title: "SaveChangesAsync",
            Category: "Blazor — Database",
            Type: "Method",
            ELI5: "Saves all changes tracked by DbContext to the database in a single transaction.",
            Example: "driver.Name = \"Updated\";\nawait DbContext.SaveChangesAsync();",
            Tips: new[]
            {
                "Call once after all changes to batch database operations.",
                "Returns the number of rows affected.",
                "Wraps all changes in a transaction—either all succeed or all fail."
            }
        )
        {
            LongELI5 = "\n\nSaveChangesAsync is like clicking 'Save All' after editing multiple files. DbContext tracks every entity you load, modify, add, or remove. When you call SaveChangesAsync(), it generates the necessary SQL (INSERT, UPDATE, DELETE) for all tracked changes and executes them in one transaction. If anything fails, nothing is saved (transaction rolls back). This ensures data consistency. The async version doesn't block the UI while waiting for the database to respond.",
            ELI5Example = "// Load entity\nvar driver = await DbContext.Drivers.FindAsync(id);\n\n// Modify (DbContext tracks this change)\ndriver.Name = \"New Name\";\ndriver.YearsExperience = 10;\n\n// Add new entity (DbContext tracks this too)\nvar newRoute = new Route { Name = \"Route 66\", DriverId = driver.Id };\nDbContext.Routes.Add(newRoute);\n\n// Save all changes in one transaction\nawait DbContext.SaveChangesAsync();\n// → Generates: UPDATE Drivers SET Name=..., YearsExperience=... WHERE Id=...\n//              INSERT INTO Routes (Name, DriverId) VALUES (...)"
        };

        // Step 10 tips
        yield return new TipTopic(
            Title: "Service Lifetimes",
            Category: "Blazor — Services",
            Type: "Pattern",
            ELI5: "Controls how long a service instance lives: Singleton (app-wide), Scoped (per user session), or Transient (new every time).",
            Example: "builder.Services.AddScoped<AppState>();\nbuilder.Services.AddSingleton<ConfigService>();",
            Tips: new[]
            {
                "Use Scoped for per-user state in Blazor Server.",
                "Singleton shares one instance across all users.",
                "Transient creates a new instance for every injection."
            }
        )
        {
            LongELI5 = "\n\nService lifetimes are like deciding whether to share tools. Singleton is like having one hammer for the entire construction site—everyone uses the same one. Scoped is like each worker getting their own toolbox at shift start and returning it at shift end—in Blazor Server, each user connection gets its own instance. Transient is like getting a brand new tool every time you ask for it—wasteful unless the tool is disposable. Choose based on whether the service holds state: stateless services can be Singleton, per-user state needs Scoped, and cheap disposable services can be Transient.",
            ELI5Example = "// Program.cs\nbuilder.Services.AddSingleton<TimeService>();  // One for entire app\nbuilder.Services.AddScoped<ShoppingCart>();    // One per user session\nbuilder.Services.AddTransient<EmailSender>();  // New instance every time\n\n// In component\n@inject ShoppingCart Cart  // Gets the user's cart instance\n@inject TimeService Time  // Gets the shared time service"
        };

        yield return new TipTopic(
            Title: "StateHasChanged",
            Category: "Blazor — Lifecycle",
            Type: "Method",
            ELI5: "Tells Blazor to re-render the component because something changed outside the normal event flow.",
            Example: "private void OnDataChanged()\n{\n    StateHasChanged();\n}",
            Tips: new[]
            {
                "Call after async callbacks or event subscriptions.",
                "Not needed after @onclick or @onchange—Blazor re-renders automatically.",
                "Use InvokeAsync for thread-safe UI updates."
            }
        )
        {
            LongELI5 = "\n\nStateHasChanged is like ringing a bell to say 'something changed, please update the display'. Blazor automatically re-renders after UI events like button clicks, but if data changes from a timer, background task, or event subscription, Blazor doesn't know. You call StateHasChanged() to trigger a re-render. In event handlers that aren't direct UI events (like a service broadcasting a notification), you often need StateHasChanged() to update what the user sees.",
            ELI5Example = "// Event subscription pattern\npublic class MyComponent : ComponentBase\n{\n    [Inject] AppState State { get; set; }\n    \n    protected override void OnInitialized()\n    {\n        State.OnChange += HandleStateChanged;\n    }\n    \n    private void HandleStateChanged()\n    {\n        StateHasChanged();  // Re-render when state changes\n    }\n    \n    public void Dispose()\n    {\n        State.OnChange -= HandleStateChanged;\n    }\n}"
        };

        yield return new TipTopic(
            Title: "Event Notifications",
            Category: "Blazor — Patterns",
            Type: "Pattern",
            ELI5: "A pattern where services notify components when data changes using C# events or callbacks.",
            Example: "public event Action? OnChange;\nprivate void NotifyStateChanged() => OnChange?.Invoke();",
            Tips: new[]
            {
                "Use Action or EventHandler for event signatures.",
                "Always unsubscribe in Dispose to prevent memory leaks.",
                "Components call StateHasChanged when notified."
            }
        )
        {
            LongELI5 = "\n\nEvent notifications are like a school bell system. The service is the bell—when something important happens (state changes), it rings. Components are listeners—they subscribe to the bell (event) and react when it rings (call StateHasChanged to re-render). This pattern lets multiple components stay in sync with shared state. When one component updates the state in the service, the service fires the event, and all subscribed components update their UI. Always unsubscribe when the component is disposed to avoid memory leaks.",
            ELI5Example = "// Service with notification\npublic class AppState\n{\n    public event Action? OnChange;\n    \n    public string SelectedDriver { get; private set; } = \"\";\n    \n    public void SelectDriver(string name)\n    {\n        SelectedDriver = name;\n        OnChange?.Invoke();  // Notify subscribers\n    }\n}\n\n// Component subscribing\npublic class DriverDisplay : ComponentBase, IDisposable\n{\n    [Inject] AppState State { get; set; }\n    \n    protected override void OnInitialized()\n    {\n        State.OnChange += StateChanged;\n    }\n    \n    private void StateChanged()\n    {\n        StateHasChanged();\n    }\n    \n    public void Dispose()\n    {\n        State.OnChange -= StateChanged;  // Unsubscribe\n    }\n}"
        };

        // Step 11 tips
        yield return new TipTopic(
            Title: "Business Validation",
            Category: "Blazor — Patterns",
            Type: "Pattern",
            ELI5: "Custom logic that validates data based on business rules, beyond simple 'required' or 'max length' checks.",
            Example: "private string ValidateAssignment(Driver driver, Truck truck)\n{\n    if (truck.Capacity < driver.MinCapacity)\n        return \"Truck too small for driver requirements\";\n    return string.Empty;\n}",
            Tips: new[]
            {
                "Put validation in services or helper methods, not UI components.",
                "Return error messages as strings or use Result<T> pattern.",
                "Validate before saving to prevent invalid data."
            }
        )
        {
            LongELI5 = "\n\nBusiness validation is like checking if a driver has the right license for a truck type, not just whether they have 'a' license. Data annotations handle simple rules (required, length), but business rules are complex: 'This driver needs CDL-A for this truck', 'This route exceeds the driver's max hours', 'This truck is already assigned'. You write these checks in methods that examine multiple entities and return error messages. Call validation before saving changes, and display errors to the user so they can fix issues.",
            ELI5Example = "public class ScheduleValidator\n{\n    public List<string> ValidateAssignment(Driver driver, Truck truck, Route route)\n    {\n        var errors = new List<string>();\n        \n        if (!driver.HasLicense(truck.RequiredLicense))\n            errors.Add($\"Driver needs {truck.RequiredLicense} license\");\n        \n        if (route.Distance > driver.MaxDistance)\n            errors.Add($\"Route too long (max: {driver.MaxDistance} mi)\");\n        \n        if (truck.Capacity < route.RequiredCapacity)\n            errors.Add(\"Truck capacity insufficient\");\n        \n        return errors;\n    }\n}\n\n// In component\nvar errors = validator.ValidateAssignment(driver, truck, route);\nif (errors.Any())\n    ShowErrors(errors);\nelse\n    await SaveAssignment();"
        };

        yield return new TipTopic(
            Title: "@bind-Value:after",
            Category: "Blazor — Data Binding",
            Type: "Directive",
            ELI5: "Runs custom code immediately after a bound value changes, useful for validation or related field updates.",
            Example: "<InputText @bind-Value=\"model.Name\" @bind-Value:after=\"ValidateName\" />",
            Tips: new[]
            {
                "Use for instant validation feedback.",
                "Can update other fields based on the changed value.",
                "Fires on every keystroke with @bind:event=\"oninput\"."
            }
        )
        {
            LongELI5 = "\n\n@bind-Value:after is like having a helper that runs a task immediately after you update a field. When a user types in an input, the bound value updates, then your 'after' method runs. This is perfect for instant validation ('check if this name is unique'), cascading updates ('when state changes, update city dropdown'), or recalculating totals. It fires after the value binding completes, so your property already has the new value when the method runs.",
            ELI5Example = "// Model\npublic string DriverName { get; set; } = \"\";\npublic string ValidationMessage { get; set; } = \"\";\n\n// Validation method\nprivate void ValidateDriverName()\n{\n    if (string.IsNullOrWhiteSpace(DriverName))\n        ValidationMessage = \"Name required\";\n    else if (DriverName.Length < 3)\n        ValidationMessage = \"Name too short\";\n    else\n        ValidationMessage = string.Empty;\n}\n\n// Markup\n<InputText @bind-Value=\"DriverName\" @bind-Value:after=\"ValidateDriverName\" />\n@if (!string.IsNullOrEmpty(ValidationMessage))\n{\n    <div class=\"error\">@ValidationMessage</div>\n}"
        };

        yield return new TipTopic(
            Title: "Switch Expressions",
            Category: "C# — Language Features",
            Type: "Syntax",
            ELI5: "A concise way to return values based on pattern matching, cleaner than long if-else chains.",
            Example: "var message = status switch\n{\n    \"Active\" => \"Driver is ready\",\n    \"OnRoute\" => \"Driver is delivering\",\n    _ => \"Unknown status\"\n};",
            Tips: new[]
            {
                "Use _ for the default case (discard pattern).",
                "Each arm must return a value of the same type.",
                "Great for mapping enums or statuses to strings."
            }
        )
        {
            LongELI5 = "\n\nSwitch expressions are like a decision tree written backwards. Traditional switch statements are verbose with 'case', 'break', etc. Switch expressions are compact: you write the value you're checking, then 'switch', then patterns with arrows (=>) pointing to results. The _ pattern is 'default'—it matches anything that didn't match earlier patterns. The result is an expression that evaluates to a value, so you can use it in assignments, return statements, or method calls.",
            ELI5Example = "// Traditional switch statement\nstring GetStatusMessage(string status)\n{\n    switch (status)\n    {\n        case \"Active\": return \"Ready\";\n        case \"OnRoute\": return \"Delivering\";\n        default: return \"Unknown\";\n    }\n}\n\n// Switch expression (cleaner)\nstring GetStatusMessage(string status) => status switch\n{\n    \"Active\" => \"Ready\",\n    \"OnRoute\" => \"Delivering\",\n    _ => \"Unknown\"\n};\n\n// Pattern matching\ndecimal GetBonus(Route route) => route.Type switch\n{\n    \"Hazmat\" => 250m,\n    \"Oversized\" => 300m,\n    \"LongHaul\" when route.Distance > 500 => route.Distance * 0.15m,\n    _ => 0m\n};"
        };

        // Step 12 tips
        yield return new TipTopic(
            Title: "Service Pattern",
            Category: "Blazor — Patterns",
            Type: "Architecture",
            ELI5: "Moving business logic out of components into dedicated service classes for reusability and testability.",
            Example: "public class ScheduleService\n{\n    public decimal CalculateDriverPay(Driver driver, Route route)\n    {\n        // Business logic here\n    }\n}",
            Tips: new[]
            {
                "Keep components thin—just UI concerns.",
                "Services hold business logic, calculations, and data access.",
                "Register services in Program.cs for dependency injection."
            }
        )
        {
            LongELI5 = "\n\nThe service pattern is like hiring specialists instead of doing everything yourself. Components are like receptionists—they show information and handle user interactions. Services are like accountants, warehouse managers, etc.—they handle specific business tasks. When you need to calculate driver pay, the component asks the ScheduleService to do it. This separation keeps components simple (easier UI changes) and services reusable (other components can use the same logic). It also makes testing easier: you can test business logic without rendering UI.",
            ELI5Example = "// Service class\npublic class ScheduleService\n{\n    public decimal CalculateDriverPay(int hours, decimal rate, int experience)\n    {\n        var base_pay = hours * rate;\n        var bonus = experience > 5 ? base_pay * 0.10m : 0;\n        return base_pay + bonus;\n    }\n}\n\n// Register in Program.cs\nbuilder.Services.AddScoped<ScheduleService>();\n\n// Use in component\n@inject ScheduleService Schedule\n\nprivate decimal totalPay;\n\nprivate void Calculate()\n{\n    totalPay = Schedule.CalculateDriverPay(hours, rate, experience);\n}"
        };

        yield return new TipTopic(
            Title: "Business Logic",
            Category: "Blazor — Patterns",
            Type: "Concept",
            ELI5: "The rules and calculations that define how your application works—pricing, validation, workflows, etc.",
            Example: "// Business logic: pay calculation with experience bonus\nvar basePay = hours * hourlyRate;\nvar bonus = years > 5 ? basePay * 0.1m : 0;\nreturn basePay + bonus;",
            Tips: new[]
            {
                "Extract to services, not inline in components.",
                "Document why rules exist, not just what they do.",
                "Make constants for magic numbers."
            }
        )
        {
            LongELI5 = "\n\nBusiness logic is the 'how it works' of your app—the rules that make it specific to your business. It's not framework code or UI code; it's things like 'drivers with 5+ years get a 10% bonus', 'hazmat routes require special certification', 'trucks over 10 tons need a CDL-A license'. This logic often involves calculations, validations, and workflows. Keep it separate from UI code so it's reusable, testable, and easy to update when business rules change. Use services to hold business logic.",
            ELI5Example = "// Business logic examples\npublic class TruckingRules\n{\n    // Rule: Experience bonus\n    public decimal ApplyExperienceBonus(decimal basePay, int years)\n    {\n        const decimal BonusRate = 0.01m;  // 1% per year\n        const decimal MaxBonus = 0.25m;   // Cap at 25%\n        \n        var multiplier = Math.Min(years * BonusRate, MaxBonus);\n        return basePay * (1 + multiplier);\n    }\n    \n    // Rule: License requirement\n    public bool CanDriveTruck(Driver driver, Truck truck)\n    {\n        return truck.Weight switch\n        {\n            < 10000 => driver.HasLicense(\"CDL-C\"),\n            < 26000 => driver.HasLicense(\"CDL-B\"),\n            _ => driver.HasLicense(\"CDL-A\")\n        };\n    }\n}"
        };

        yield return new TipTopic(
            Title: "Calculation Methods",
            Category: "C# — Methods",
            Type: "Pattern",
            ELI5: "Methods dedicated to performing calculations, with clear inputs and outputs, making formulas reusable and testable.",
            Example: "public decimal CalculateFuelCost(int distance, decimal mpg, decimal pricePerGallon)\n{\n    return (distance / mpg) * pricePerGallon;\n}",
            Tips: new[]
            {
                "Make them pure functions when possible (no side effects).",
                "Use descriptive names that explain what's calculated.",
                "Document the formula and units."
            }
        )
        {
            LongELI5 = "\n\nCalculation methods are like showing your work in math class. Instead of scattering formulas throughout your code, you create focused methods that do one calculation. This makes the formula reusable (call it from multiple places), testable (easy to verify with different inputs), and understandable (the method name explains what it calculates). Good calculation methods are pure functions: given the same inputs, they always return the same output, with no hidden dependencies or side effects.",
            ELI5Example = "public class RouteCalculations\n{\n    private const decimal FuelPricePerGallon = 3.85m;\n    private const double AverageMpg = 6.5;\n    \n    // Pure calculation method\n    public decimal EstimateFuelCost(int distanceMiles)\n    {\n        var gallons = distanceMiles / AverageMpg;\n        return (decimal)gallons * FuelPricePerGallon;\n    }\n    \n    public decimal CalculateRevenue(int distance, decimal ratePerMile, decimal surcharges)\n    {\n        var baseRevenue = distance * ratePerMile;\n        return baseRevenue + surcharges;\n    }\n    \n    public decimal CalculateProfit(decimal revenue, decimal costs)\n    {\n        return revenue - costs;\n    }\n}\n\n// Usage\nvar calc = new RouteCalculations();\nvar fuel = calc.EstimateFuelCost(500);\nvar revenue = calc.CalculateRevenue(500, 2.5m, 50m);\nvar profit = calc.CalculateProfit(revenue, fuel);"
        };

        // Step 13 tips
        yield return new TipTopic(
            Title: "LINQ Aggregation",
            Category: "C# — LINQ",
            Type: "Methods",
            ELI5: "Methods like Sum(), Count(), Average() that compute a single value from a collection of items.",
            Example: "var total = routes.Sum(r => r.Revenue);\nvar avg = routes.Average(r => r.Distance);\nvar count = routes.Count(r => r.Status == \"Active\");",
            Tips: new[]
            {
                "Use with filtering: routes.Where(r => r.IsActive).Sum(r => r.Revenue)",
                "Count() with predicate is cleaner than Where().Count()",
                "Returns default (0) for empty collections."
            }
        )
        {
            LongELI5 = "\n\nLINQ aggregation is like using a calculator on a spreadsheet column. Instead of looping through items manually to add them up, you use methods like Sum(), Count(), Average(), Min(), or Max(). These methods examine every item in a collection and return a single summary value. You can combine them with Where() to filter first: 'sum the revenue of only active routes'. Aggregation methods are expressive and concise—they clearly state your intent.",
            ELI5Example = "var routes = await DbContext.Routes.ToListAsync();\n\n// Total revenue\nvar totalRevenue = routes.Sum(r => r.Revenue);\n\n// Count active routes\nvar activeCount = routes.Count(r => r.Status == \"Active\");\n\n// Average distance\nvar avgDistance = routes.Average(r => r.Distance);\n\n// Max revenue\nvar topRevenue = routes.Max(r => r.Revenue);\n\n// Unique drivers\nvar uniqueDrivers = routes.Select(r => r.DriverId).Distinct().Count();\n\n// Filtered aggregation\nvar completedRevenue = routes\n    .Where(r => r.Status == \"Completed\")\n    .Sum(r => r.Revenue);"
        };

        yield return new TipTopic(
            Title: "Conditional Styling",
            Category: "Blazor — Styling",
            Type: "Pattern",
            ELI5: "Applying different CSS classes based on data values or state, like showing green for 'Completed' and red for 'Delayed'.",
            Example: "var statusClass = status switch\n{\n    \"Active\" => \"bg-green-100 text-green-700\",\n    \"Delayed\" => \"bg-red-100 text-red-700\",\n    _ => \"bg-gray-100 text-gray-700\"\n};\n\n<span class=\"@statusClass\">@status</span>",
            Tips: new[]
            {
                "Use switch expressions or helper methods to map data to CSS classes.",
                "Keep styling logic out of markup—create GetStatusClass() methods.",
                "Use Tailwind utilities for quick color/style changes."
            }
        )
        {
            LongELI5 = "\n\nConditional styling is like traffic lights changing color based on conditions. You examine some data (like route status) and return different CSS classes based on the value. Green background for completed routes, yellow for in-progress, red for delayed. This gives instant visual feedback. In Blazor, create helper methods that return class strings, then use @GetStatusClass(status) in your markup. This keeps styling logic reusable and your markup clean.",
            ELI5Example = "// Helper method\nprivate string GetStatusClass(string status) => status switch\n{\n    \"Scheduled\" => \"bg-blue-100 text-blue-700\",\n    \"In Progress\" => \"bg-yellow-100 text-yellow-700\",\n    \"Completed\" => \"bg-green-100 text-green-700\",\n    \"Delayed\" => \"bg-red-100 text-red-700\",\n    _ => \"bg-gray-100 text-gray-700\"\n};\n\n// Markup\n@foreach (var route in routes)\n{\n    <div class=\"route-card\">\n        <span class=\"badge @GetStatusClass(route.Status)\">\n            @route.Status\n        </span>\n        <p>@route.Name</p>\n    </div>\n}\n\n// Can also conditionally apply styles\n<div class=\"@(isUrgent ? \"border-red-500\" : \"border-gray-300\")\">\n    ...\n</div>"
        };

        yield return new TipTopic(
            Title: "KPI Design",
            Category: "Blazor — UI Patterns",
            Type: "Pattern",
            ELI5: "Key Performance Indicators—displaying important metrics (revenue, count, averages) prominently for quick insights.",
            Example: "<div class=\"kpi-card\">\n    <div class=\"kpi-label\">Total Revenue</div>\n    <div class=\"kpi-value\">$@totalRevenue.ToString(\"N0\")</div>\n</div>",
            Tips: new[]
            {
                "Show the most important numbers first.",
                "Use large, bold text for values.",
                "Add context with comparisons or trends."
            }
        )
        {
            LongELI5 = "\n\nKPI design is like a car dashboard showing speed, fuel, and engine temp at a glance. Key Performance Indicators are the vital stats of your business: total revenue, active routes, average delivery time. Design them to stand out: use large numbers, color coding (green for good, red for bad), and clear labels. Group related KPIs together (all financial metrics in one section). Good KPI cards give users instant understanding without needing to read tables or dig through data. They answer 'how are we doing?' at a glance.",
            ELI5Example = "// KPI Card Component Pattern\n<div class=\"grid grid-cols-1 md:grid-cols-3 gap-4\">\n    <!-- KPI: Total Revenue -->\n    <div class=\"kpi-card bg-green-50 border border-green-200 rounded-xl p-4\">\n        <div class=\"text-xs text-green-600 font-semibold\">TOTAL REVENUE</div>\n        <div class=\"text-3xl font-bold text-green-900\">$@totalRevenue.ToString(\"N0\")</div>\n        <div class=\"text-xs text-green-700\">+12% from last month</div>\n    </div>\n    \n    <!-- KPI: Active Routes -->\n    <div class=\"kpi-card bg-blue-50 border border-blue-200 rounded-xl p-4\">\n        <div class=\"text-xs text-blue-600 font-semibold\">ACTIVE ROUTES</div>\n        <div class=\"text-3xl font-bold text-blue-900\">@activeRoutes</div>\n        <div class=\"text-xs text-blue-700\">@totalRoutes total</div>\n    </div>\n    \n    <!-- KPI: Average Distance -->\n    <div class=\"kpi-card bg-purple-50 border border-purple-200 rounded-xl p-4\">\n        <div class=\"text-xs text-purple-600 font-semibold\">AVG DISTANCE</div>\n        <div class=\"text-3xl font-bold text-purple-900\">@avgDistance.ToString(\"N0\") mi</div>\n        <div class=\"text-xs text-purple-700\">Per route</div>\n    </div>\n</div>"
        };
    }
}

