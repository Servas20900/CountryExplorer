using ExplorerApp.Shared.DTOs;

namespace ExplorerApp.Services.Interfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryDto>> GetAllFromApiAsync();
        Task<CountryDto?> SearchByNameAsync(string name);
        Task<IEnumerable<CountryDto>> GetFromDbAsync();
        Task SaveCountryToDbAsync(CountryDto dto);
    }
}