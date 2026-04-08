using ExplorerApp.Shared.DTOs;

namespace ExplorerApp.WebMVC.Services;

public interface IExplorerApiClientService
{
    Task<List<CountryDto>> GetSavedCountriesAsync();
    Task<List<CountryDto>> GetArchivedCountriesAsync();
    Task<CountryDto?> SearchCountryAsync(string query);
}