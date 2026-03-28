using System.Net.Http.Json;
using System.Text.Json;
using ExplorerApp.Infrastructure.Data;
using ExplorerApp.Infrastructure.Models;
using ExplorerApp.Services.Interfaces;
using ExplorerApp.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExplorerApp.Services
{
    public class RestCountriesService : ICountryService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;

        public RestCountriesService(HttpClient httpClient, AppDbContext context)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://restcountries.com/v3.1/");
        }

        public async Task<IEnumerable<CountryDto>> GetAllFromApiAsync()
        {
            var response = await _httpClient.GetAsync("all?fields=name,capital,population,region,flags");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var parsed = JsonDocument.Parse(json).RootElement;

            var countries = new List<CountryDto>();

            foreach (var item in parsed.EnumerateArray())
            {
                countries.Add(new CountryDto
                {
                    Name = item.GetProperty("name").GetProperty("common").GetString() ?? "",
                    Capital = item.TryGetProperty("capital", out var cap) && cap.ValueKind == JsonValueKind.Array && cap.GetArrayLength() > 0
                        ? cap[0].GetString() ?? "" : "",
                    Population = item.GetProperty("population").GetInt64(),
                    Region = item.GetProperty("region").GetString() ?? "",
                    FlagUrl = item.GetProperty("flags").GetProperty("png").GetString() ?? ""
                });
            }

            return countries;
        }

        public async Task<CountryDto?> SearchByNameAsync(string name)
        {
            var response = await _httpClient.GetAsync($"name/{name}?fields=name,capital,population,region,flags");

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var parsed = JsonDocument.Parse(json).RootElement;

            var item = parsed[0];

            return new CountryDto
            {
                Name = item.GetProperty("name").GetProperty("common").GetString() ?? "",
                Capital = item.TryGetProperty("capital", out var cap) && cap.ValueKind == JsonValueKind.Array && cap.GetArrayLength() > 0
                    ? cap[0].GetString() ?? "" : "",
                Population = item.GetProperty("population").GetInt64(),
                Region = item.GetProperty("region").GetString() ?? "",
                FlagUrl = item.GetProperty("flags").GetProperty("png").GetString() ?? ""
            };
        }

        public async Task<IEnumerable<CountryDto>> GetFromDbAsync()
        {
            return await _context.Countries
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Capital = c.Capital,
                    Population = c.Population,
                    Region = c.Region,
                    FlagUrl = c.FlagUrl
                })
                .ToListAsync();
        }

        public async Task SaveCountryToDbAsync(CountryDto dto)
        {
            var exists = await _context.Countries.AnyAsync(c => c.Name == dto.Name);
            if (exists) return;

            var country = new Country
            {
                Name = dto.Name,
                Capital = dto.Capital,
                Population = dto.Population,
                Region = dto.Region,
                FlagUrl = dto.FlagUrl
            };

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
        }
    }
}