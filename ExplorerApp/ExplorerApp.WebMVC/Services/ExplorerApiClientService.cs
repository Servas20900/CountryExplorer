using System.Net.Http.Json;
using ExplorerApp.Shared.DTOs;

namespace ExplorerApp.WebMVC.Services;

public class ExplorerApiClientService : IExplorerApiClientService
{
    private readonly HttpClient httpClient;

    public ExplorerApiClientService(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient("ExplorerApi");
    }

    public async Task<List<CountryDto>> GetSavedCountriesAsync()
    {
        return await httpClient.GetFromJsonAsync<List<CountryDto>>("api/countries") ?? [];
    }

    public async Task<List<CountryDto>> GetArchivedCountriesAsync()
    {
        return await httpClient.GetFromJsonAsync<List<CountryDto>>("api/countries/archived") ?? [];
    }

    public async Task<CountryDto?> SearchCountryAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return null;
        }

        var response = await httpClient.GetAsync($"api/countries/search/{Uri.EscapeDataString(query.Trim())}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<CountryDto>();
    }

}