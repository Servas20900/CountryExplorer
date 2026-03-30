using ExplorerApp.Infrastructure.Data;
using ExplorerApp.Services;
using ExplorerApp.Services.Interfaces;
using ExplorerApp.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<ICountryService, RestCountriesService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var countries = app.MapGroup("/api/countries");

countries.MapGet("/", async (ICountryService service) =>
    Results.Ok(await service.GetFromDbAsync()));

countries.MapGet("/external", async (ICountryService service) =>
    Results.Ok(await service.GetAllFromApiAsync()));

countries.MapGet("/{name}", async (string name, ICountryService service) =>
{
    var country = await service.SearchByNameAsync(name);
    return country is not null ? Results.Ok(country) : Results.NotFound();
});

countries.MapPost("/save", async (CountryDto dto, ICountryService service) =>
{
    await service.SaveCountryToDbAsync(dto);
    return Results.Created($"/api/countries/{dto.Name}", dto);
});

var favorites = app.MapGroup("/api/favorites");

favorites.MapGet("/", async (IFavoriteService service) =>
    Results.Ok(await service.GetAllFavoritesAsync()));

favorites.MapPost("/", async (CreateFavoriteDto dto, IFavoriteService service) =>
{
    try
    {
        await service.AddFavoriteAsync(dto.CountryId);
        return Results.StatusCode(201);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

favorites.MapDelete("/{id:int}", async (int id, IFavoriteService service) =>
{
    try
    {
        await service.DeleteFavoriteAsync(id);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.Run();