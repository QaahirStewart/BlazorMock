using BlazorMock.Components;
using BlazorMock.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        // Enable detailed circuit errors in development
        options.DetailedErrors = builder.Environment.IsDevelopment();
    });

// Note: Using the new Razor Components model with Interactive Server render mode.
// Do NOT register AddServerSideBlazor here to avoid conflicts with the Razor Components pipeline.

// App services
builder.Services.AddSingleton<LayoutState>();
builder.Services.AddScoped<ILearningProgressService, LearningProgressService>();
builder.Services.AddSingleton<ITipsService, TipsService>();
builder.Services.AddSingleton<ITipsContributor, DefaultTipsContributor>();
builder.Services.AddSingleton<ITipsContributor, FormsTipsContributor>();
builder.Services.AddSingleton<ITipsContributor, RoutingTipsContributor>();
builder.Services.AddSingleton<ITipsContributor, ServicesTipsContributor>();
builder.Services.AddSingleton<ITipsContributor, LifecycleTipsContributor>();
builder.Services.AddSingleton<ITipsContributor, NavigationTipsContributor>();
builder.Services.AddSingleton<ITipsContributor, JsInteropTipsContributor>();
builder.Services.AddSingleton<ITipsContributor, LifecycleAdvancedTipsContributor>();
builder.Services.AddSingleton<ITipsContributor, DataBindingTipsContributor>();
builder.Services.AddSingleton<ITipsContributor, CSharpLanguageTipsContributor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Seed tips from contributors
using (var scope = app.Services.CreateScope())
{
    var tips = scope.ServiceProvider.GetRequiredService<ITipsService>();
    foreach (var c in scope.ServiceProvider.GetServices<ITipsContributor>())
        tips.AddContributor(c);
}

app.Run();
