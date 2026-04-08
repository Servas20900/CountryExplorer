using ExplorerApp.Infrastructure.Data;
using ExplorerApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExplorerApp.Services;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddExplorerAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddHttpClient("RestCountriesApi", client =>
        {
            client.BaseAddress = new Uri("https://restcountries.com/v3.1/");
        });

        services.AddScoped<IRestCountriesService, RestCountriesService>();
        services.AddScoped<IFavoriteService, FavoriteService>();

        return services;
    }

    public static void ApplyExplorerMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
}
