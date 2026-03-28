using ExplorerApp.Shared.DTOs;

namespace ExplorerApp.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<FavoriteDto>> GetAllFavoritesAsync();
        Task AddFavoriteAsync(int countryId);
        Task DeleteFavoriteAsync(int favoriteId);
    }
}