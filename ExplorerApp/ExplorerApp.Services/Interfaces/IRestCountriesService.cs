using ExplorerApp.Shared.DTOs;

namespace ExplorerApp.Services.Interfaces
{
    public interface IRestCountriesService
    {
        Task<List<CountryDto>> GetAllAsync(string[] fields);
        Task<CountryDto?> GetByNameAsync(string name);
        Task<CountryDto?> GetByCodeAsync(string code);
        Task<List<CountryDto>> GetByRegionAsync(string region);
        Task<List<CountryDto>> GetAllSavedAsync();
        Task<List<CountryDto>> GetAllArchivedAsync();
        Task<bool> ExistsSavedByCodeAsync(string countryCode);
        Task<CountryDto> SaveCountryAsync(CreateCountryDto dto);
        Task<bool> ArchiveCountryAsync(int id);
        Task<bool> UnarchiveCountryAsync(int id);
        Task<DashboardDto> GetDashboardAsync();
    }
}
