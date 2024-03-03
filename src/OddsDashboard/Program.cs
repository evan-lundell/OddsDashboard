using OddsDashboard;
using OddsDashboard.Components;
using OddsDashboard.Services;

var builder = WebApplication.CreateBuilder(args);

if (string.IsNullOrWhiteSpace(builder.Configuration[Constants.OddsApiUrlEnvVar]))
{
    Console.WriteLine("Odds API Url not provided");
    Environment.Exit(1);
}

if (string.IsNullOrWhiteSpace(builder.Configuration[Constants.OddsApiKeyEnvVar]))
{
    Console.WriteLine("Odds API key not provided");
    Environment.Exit(1);
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddLogging();

if ((builder.Configuration[Constants.EnvironmentEnvVar] ?? "Development") == "Development")
{
    builder.Services.AddScoped<IOddsService, OddsFileService>();
}
else
{
    builder.Services.AddHttpClient<IOddsService, OddsService>(options =>
    {
        options.BaseAddress = new Uri(builder.Configuration[Constants.OddsApiUrlEnvVar]!);
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(OddsDashboard.Client._Imports).Assembly);

app.Run();
