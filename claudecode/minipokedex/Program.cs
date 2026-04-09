using MiniPokedex.Application.Services;
using MiniPokedex.Domain.Ports;
using MiniPokedex.Infrastructure.PokeApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Infrastructure: typed HTTP client for the PokéAPI (internal to the Infrastructure layer).
builder.Services.AddHttpClient<IPokeApiClient, PokeApiClient>(client =>
{
    client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
});

// Infrastructure → Domain: concrete repository wired to the domain port.
builder.Services.AddScoped<IPokemonRepository, PokeApiPokemonRepository>();

// Application: orchestration service used by controllers.
builder.Services.AddScoped<PokemonAppService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pokemon}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
