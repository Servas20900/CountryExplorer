using System.Net;
using System.Net.Http;
using ExplorerApp.Infrastructure.Data;
using ExplorerApp.Infrastructure.Models;
using ExplorerApp.Services;
using ExplorerApp.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;

namespace ExplorerApp.Tests;

public class ServicesTests
{
    [Fact]
    public async Task GetByNameAsync_ReturnsCountryDto_WhenCountryExists()
    {
        var json = """
        [
          {
            "cca3": "CRI",
            "name": { "common": "Costa Rica", "official": "Republic of Costa Rica" },
            "capital": ["San Jose"],
            "region": "Americas",
            "subregion": "Central America",
            "flags": { "png": "https://flag.png", "svg": "https://flag.svg" },
            "latlng": [10.0, -84.0]
          }
        ]
        """;

        var handler = new Mock<HttpMessageHandler>();
        handler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });

        var httpClient = new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://restcountries.com/v3.1/")
        };

        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(f => f.CreateClient("RestCountriesApi")).Returns(httpClient);

        await using var db = CreateDbContext();
        var service = new RestCountriesService(factory.Object, db);

        var result = await service.GetByNameAsync("Costa Rica");

        Assert.NotNull(result);
        Assert.Equal("CRI", result.CountryCode);
        Assert.Equal("Costa Rica", result.CommonName);
        Assert.Equal("San Jose", result.Capital);
    }

    [Fact]
    public async Task SaveCountryAsync_ReturnsSavedDto_WhenValid()
    {
        var factory = new Mock<IHttpClientFactory>();
        await using var db = CreateDbContext();
        var service = new RestCountriesService(factory.Object, db);

        var dto = new CreateCountryDto
        {
            CountryCode = "cri",
            CommonName = "Costa Rica",
            OfficialName = "Republic of Costa Rica",
            Capital = "San Jose",
            Region = "Americas",
            Subregion = "Central America",
            FlagPngUrl = "https://flag.png",
            FlagSvgUrl = "https://flag.svg",
            Latitude = 10,
            Longitude = -84
        };

        var result = await service.SaveCountryAsync(dto);

        Assert.Equal("CRI", result.CountryCode);
        Assert.Equal("Costa Rica", result.CommonName);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task AddFavoriteAsync_ReturnsDto_WhenFavoriteAdded()
    {
        await using var db = CreateDbContext();
        var service = new FavoriteService(db);

        var result = await service.AddFavoriteAsync(new CreateFavoriteDto
        {
            CountryCode = "CRI",
            CommonName = "Costa Rica",
            FlagPngUrl = "https://flag.png"
        });

        Assert.True(result.Id > 0);
        Assert.Equal("CRI", result.CountryCode);
        Assert.Equal("Costa Rica", result.CommonName);
    }

    [Fact]
    public async Task DeleteFavoriteAsync_ReturnsTrue_WhenFavoriteExists()
    {
        await using var db = CreateDbContext();
        db.Favorites.Add(new Favorite
        {
            CountryCode = "CRI",
            CommonName = "Costa Rica",
            FlagPngUrl = "https://flag.png",
            AddedAt = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var favoriteId = await db.Favorites.Select(f => f.Id).FirstAsync();
        var service = new FavoriteService(db);

        var result = await service.DeleteFavoriteAsync(favoriteId);

        Assert.True(result);
        Assert.Empty(db.Favorites);
    }

    [Fact]
    public async Task GetDashboardAsync_ReturnsCorrectCounts()
    {
        var factory = new Mock<IHttpClientFactory>();
        await using var db = CreateDbContext();

        db.Countries.AddRange(
            new Country
            {
                CountryCode = "CRI",
                CommonName = "Costa Rica",
                OfficialName = "Republic of Costa Rica",
                Capital = "San Jose",
                Region = "Americas",
                Subregion = "Central America",
                FlagPngUrl = "https://flag1.png",
                FlagSvgUrl = "https://flag1.svg",
                Latitude = 10,
                Longitude = -84,
                CreatedAt = DateTime.UtcNow.AddMinutes(-5)
            },
            new Country
            {
                CountryCode = "MEX",
                CommonName = "Mexico",
                OfficialName = "United Mexican States",
                Capital = "Mexico City",
                Region = "Americas",
                Subregion = "North America",
                FlagPngUrl = "https://flag2.png",
                FlagSvgUrl = "https://flag2.svg",
                Latitude = 23,
                Longitude = -102,
                CreatedAt = DateTime.UtcNow
            });

        db.Favorites.AddRange(
            new Favorite
            {
                CountryCode = "CRI",
                CommonName = "Costa Rica",
                FlagPngUrl = "https://flag1.png",
                AddedAt = DateTime.UtcNow.AddMinutes(-2)
            },
            new Favorite
            {
                CountryCode = "MEX",
                CommonName = "Mexico",
                FlagPngUrl = "https://flag2.png",
                AddedAt = DateTime.UtcNow
            });

        await db.SaveChangesAsync();

        var service = new RestCountriesService(factory.Object, db);

        var result = await service.GetDashboardAsync();

        Assert.Equal(2, result.TotalCountriesSaved);
        Assert.Equal(2, result.TotalFavorites);
        Assert.Equal(2, result.MostRecentFavorites.Count);
        Assert.Equal(2, result.MostRecentCountries.Count);
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
