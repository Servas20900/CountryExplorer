using ExplorerApp.Services;
using ExplorerApp.Services.Interfaces;
using ExplorerApp.Shared.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddExplorerAppServices(builder.Configuration);

var app = builder.Build();

app.Services.ApplyExplorerMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

var api = app.MapGroup("/api");
var countries = api.MapGroup("/countries");

countries.MapGet("/", async (IRestCountriesService service) =>
    Results.Ok(await service.GetAllSavedAsync()));

countries.MapGet("/archived", async (IRestCountriesService service) =>
    Results.Ok(await service.GetAllArchivedAsync()));

countries.MapGet("/external", async (IRestCountriesService service) =>
    Results.Ok(await service.GetAllAsync(new[] { "name", "capital", "flags", "region", "latlng", "subregion", "cca3" })));

countries.MapGet("/search/{name}", async (string name, IRestCountriesService service) =>
{
    var country = await service.GetByNameAsync(name);
    return country is not null ? Results.Ok(country) : Results.NotFound();
});

countries.MapPost("/save", async (CreateCountryDto dto, IRestCountriesService service) =>
{
    var existsBefore = await service.ExistsSavedByCodeAsync(dto.CountryCode);
    var saved = await service.SaveCountryAsync(dto);

    if (existsBefore)
    {
        return Results.Ok(new SaveCountryResultDto
        {
            Guardado = false,
            Mensaje = "El pais ya estaba guardado.",
            Pais = saved
        });
    }

    return Results.Created($"/api/countries/{saved.CountryCode}", new SaveCountryResultDto
    {
        Guardado = true,
        Mensaje = "Pais guardado correctamente.",
        Pais = saved
    });
});

countries.MapPost("/{id:int}/archive", async (int id, IRestCountriesService service) =>
{
    var archived = await service.ArchiveCountryAsync(id);
    return archived ? Results.NoContent() : Results.NotFound();
});

countries.MapPost("/{id:int}/unarchive", async (int id, IRestCountriesService service) =>
{
    var unarchived = await service.UnarchiveCountryAsync(id);
    return unarchived ? Results.NoContent() : Results.NotFound();
});

var favorites = api.MapGroup("/favorites");

favorites.MapGet("/", async (IFavoriteService service) =>
    Results.Ok(await service.GetAllFavoritesAsync()));

favorites.MapPost("/", async (CreateFavoriteDto dto, IFavoriteService service) =>
{
    var favorite = await service.AddFavoriteAsync(dto);
    return Results.Created($"/api/favorites/{favorite.Id}", favorite);
});

favorites.MapDelete("/{id:int}", async (int id, IFavoriteService service) =>
{
    var deleted = await service.DeleteFavoriteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

api.MapGet("/dashboard", async (IRestCountriesService service) =>
    Results.Ok(await service.GetDashboardAsync()));

app.Run();