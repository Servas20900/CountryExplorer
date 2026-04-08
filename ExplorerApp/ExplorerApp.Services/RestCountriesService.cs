using System.Text.Json;
using System.Linq.Expressions;
using ExplorerApp.Infrastructure.Data;
using ExplorerApp.Infrastructure.Models;
using ExplorerApp.Services.Interfaces;
using ExplorerApp.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExplorerApp.Services
{
    public class RestCountriesService : IRestCountriesService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppDbContext _context;

        public RestCountriesService(IHttpClientFactory httpClientFactory, AppDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        public async Task<List<CountryDto>> GetAllAsync(string[] fields)
        {
            var selectedFields = fields != null && fields.Length > 0
                ? string.Join(',', fields)
                : "name,capital,flags,region,subregion,latlng,cca3";

            var response = await GetClient().GetAsync($"all?fields={selectedFields}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return ParseCountryArray(json);
        }

        public async Task<CountryDto?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var normalizedName = name.Trim();
            var response = await GetClient().GetAsync($"name/{Uri.EscapeDataString(normalizedName)}?fields=name,capital,flags,region,subregion,latlng,cca3");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return ParseCountryArray(json).FirstOrDefault();
        }

        public async Task<CountryDto?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            var response = await GetClient().GetAsync($"alpha/{Uri.EscapeDataString(code.Trim())}?fields=name,capital,flags,region,subregion,latlng,cca3");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var root = JsonDocument.Parse(json).RootElement;

            if (root.ValueKind == JsonValueKind.Array)
            {
                return ParseCountryArray(json).FirstOrDefault();
            }

            return MapApiCountry(root);
        }

        public async Task<List<CountryDto>> GetByRegionAsync(string region)
        {
            if (string.IsNullOrWhiteSpace(region))
            {
                return new List<CountryDto>();
            }

            var response = await GetClient().GetAsync($"region/{Uri.EscapeDataString(region.Trim())}?fields=name,capital,flags,region,subregion,latlng,cca3");

            if (!response.IsSuccessStatusCode)
            {
                return new List<CountryDto>();
            }

            var json = await response.Content.ReadAsStringAsync();
            return ParseCountryArray(json);
        }

        public async Task<List<CountryDto>> GetAllSavedAsync()
        {
            return await _context.Countries
                .Where(c => !c.IsArchived)
                .OrderBy(c => c.CommonName)
                .Select(ToCountryDtoExpression())
                .ToListAsync();
        }

        public async Task<List<CountryDto>> GetAllArchivedAsync()
        {
            return await _context.Countries
                .Where(c => c.IsArchived)
                .OrderBy(c => c.CommonName)
                .Select(ToCountryDtoExpression())
                .ToListAsync();
        }

        public async Task<bool> ExistsSavedByCodeAsync(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                return false;
            }

            var normalizedCode = countryCode.Trim().ToUpperInvariant();
            return await _context.Countries.AnyAsync(c => c.CountryCode == normalizedCode);
        }

        public async Task<CountryDto> SaveCountryAsync(CreateCountryDto dto)
        {
            var normalizedCode = dto.CountryCode.Trim().ToUpperInvariant();

            var existing = await _context.Countries
                .FirstOrDefaultAsync(c => c.CountryCode == normalizedCode);

            if (existing != null)
            {
                return ToCountryDto(existing);
            }

            var country = new Country
            {
                CountryCode = normalizedCode,
                CommonName = dto.CommonName.Trim(),
                OfficialName = dto.OfficialName.Trim(),
                Capital = dto.Capital.Trim(),
                Region = dto.Region.Trim(),
                Subregion = dto.Subregion.Trim(),
                FlagPngUrl = dto.FlagPngUrl.Trim(),
                FlagSvgUrl = dto.FlagSvgUrl.Trim(),
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return ToCountryDto(country);
        }

        public async Task<bool> ArchiveCountryAsync(int id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return false;
            }

            if (country.IsArchived)
            {
                return true;
            }

            country.IsArchived = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnarchiveCountryAsync(int id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return false;
            }

            if (!country.IsArchived)
            {
                return true;
            }

            country.IsArchived = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            var totalCountriesSaved = await _context.Countries.CountAsync();
            var totalFavorites = await _context.Favorites.CountAsync();

            var mostRecentFavorites = await _context.Favorites
                .OrderByDescending(f => f.AddedAt)
                .Take(5)
                .Select(f => new FavoriteDto
                {
                    Id = f.Id,
                    CountryCode = f.CountryCode,
                    CommonName = f.CommonName,
                    FlagPngUrl = f.FlagPngUrl,
                    AddedAt = f.AddedAt
                })
                .ToListAsync();

            var mostRecentCountries = await _context.Countries
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .Select(ToCountryDtoExpression())
                .ToListAsync();

            return new DashboardDto
            {
                TotalCountriesSaved = totalCountriesSaved,
                TotalFavorites = totalFavorites,
                MostRecentFavorites = mostRecentFavorites,
                MostRecentCountries = mostRecentCountries
            };
        }

        private HttpClient GetClient()
        {
            return _httpClientFactory.CreateClient("RestCountriesApi");
        }

        private static List<CountryDto> ParseCountryArray(string json)
        {
            var root = JsonDocument.Parse(json).RootElement;
            var countries = new List<CountryDto>();

            if (root.ValueKind != JsonValueKind.Array)
            {
                return countries;
            }

            foreach (var item in root.EnumerateArray())
            {
                countries.Add(MapApiCountry(item));
            }

            return countries;
        }

        private static CountryDto MapApiCountry(JsonElement item)
        {
            var countryCode = item.TryGetProperty("cca3", out var code)
                ? code.GetString() ?? string.Empty
                : string.Empty;

            var commonName = item.TryGetProperty("name", out var nameObj) && nameObj.TryGetProperty("common", out var common)
                ? common.GetString() ?? string.Empty
                : string.Empty;

            var officialName = item.TryGetProperty("name", out var officialObj) && officialObj.TryGetProperty("official", out var official)
                ? official.GetString() ?? string.Empty
                : string.Empty;

            var capital = item.TryGetProperty("capital", out var cap) && cap.ValueKind == JsonValueKind.Array && cap.GetArrayLength() > 0
                ? cap[0].GetString() ?? string.Empty
                : string.Empty;

            var region = item.TryGetProperty("region", out var regionValue)
                ? regionValue.GetString() ?? string.Empty
                : string.Empty;

            var subregion = item.TryGetProperty("subregion", out var subregionValue)
                ? subregionValue.GetString() ?? string.Empty
                : string.Empty;

            var flagPng = item.TryGetProperty("flags", out var flags) && flags.TryGetProperty("png", out var png)
                ? png.GetString() ?? string.Empty
                : string.Empty;

            var flagSvg = item.TryGetProperty("flags", out var flagsObj) && flagsObj.TryGetProperty("svg", out var svg)
                ? svg.GetString() ?? string.Empty
                : string.Empty;

            var latitude = 0d;
            var longitude = 0d;

            if (item.TryGetProperty("latlng", out var latlng) && latlng.ValueKind == JsonValueKind.Array)
            {
                if (latlng.GetArrayLength() > 0)
                {
                    latitude = latlng[0].GetDouble();
                }

                if (latlng.GetArrayLength() > 1)
                {
                    longitude = latlng[1].GetDouble();
                }
            }

            return new CountryDto
            {
                CountryCode = countryCode,
                CommonName = commonName,
                OfficialName = officialName,
                Capital = capital,
                Region = region,
                Subregion = subregion,
                FlagPngUrl = flagPng,
                FlagSvgUrl = flagSvg,
                Latitude = latitude,
                Longitude = longitude,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static CountryDto ToCountryDto(Country country)
        {
            return new CountryDto
            {
                Id = country.Id,
                CountryCode = country.CountryCode,
                CommonName = country.CommonName,
                OfficialName = country.OfficialName,
                Capital = country.Capital,
                Region = country.Region,
                Subregion = country.Subregion,
                FlagPngUrl = country.FlagPngUrl,
                FlagSvgUrl = country.FlagSvgUrl,
                Latitude = country.Latitude,
                Longitude = country.Longitude,
                CreatedAt = country.CreatedAt
            };
        }

        private static Expression<Func<Country, CountryDto>> ToCountryDtoExpression()
        {
            return c => new CountryDto
            {
                Id = c.Id,
                CountryCode = c.CountryCode,
                CommonName = c.CommonName,
                OfficialName = c.OfficialName,
                Capital = c.Capital,
                Region = c.Region,
                Subregion = c.Subregion,
                FlagPngUrl = c.FlagPngUrl,
                FlagSvgUrl = c.FlagSvgUrl,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                IsArchived = c.IsArchived,
                CreatedAt = c.CreatedAt
            };
        }
    }
}