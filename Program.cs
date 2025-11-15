using BlazorMock.Components;
using BlazorMock.Services;
using BlazorMock.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        // Enable detailed circuit errors in development
        options.DetailedErrors = builder.Environment.IsDevelopment();
    });

// HTTP clients
builder.Services.AddHttpClient();

// Note: Using the new Razor Components model with Interactive Server render mode.
// Do NOT register AddServerSideBlazor here to avoid conflicts with the Razor Components pipeline.

// App services
builder.Services.AddSingleton<LayoutState>();
builder.Services.AddScoped<ILearningProgressService, LearningProgressService>();
builder.Services.AddSingleton<ILearningGuideService, LearningGuideService>();
builder.Services.AddScoped<AppState>();
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
builder.Services.AddSingleton<ITipsContributor, HttpAndDataTipsContributor>();
builder.Services.AddSingleton<ITipsContributor, Step6TipsContributor>();

// EF Core - SQLite
var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Data Source=blazormock.db";
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(connectionString));

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

// Seed tips and development data
using (var scope = app.Services.CreateScope())
{
    var tips = scope.ServiceProvider.GetRequiredService<ITipsService>();
    foreach (var c in scope.ServiceProvider.GetServices<ITipsContributor>())
        tips.AddContributor(c);

    // Ensure database exists
    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var db = dbFactory.CreateDbContext();
    // Apply pending EF Core migrations at startup (safe for dev)
    db.Database.Migrate();

    // Development seed data for quick demos
    if (app.Environment.IsDevelopment())
    {
        await DevDataSeeder.SeedAsync(db);
    }
}

// Development-only: endpoint to reset sample data
if (app.Environment.IsDevelopment())
{
    app.MapPost("/dev/reset-sample-data", async (IDbContextFactory<AppDbContext> factory) =>
    {
        await using var db = await factory.CreateDbContextAsync();
        await DevDataSeeder.ResetAsync(db);
        return Results.Ok(new { status = "reset", at = DateTime.UtcNow });
    });
}

app.Run();
