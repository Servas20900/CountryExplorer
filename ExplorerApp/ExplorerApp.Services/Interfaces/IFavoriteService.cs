using ExplorerApp.Shared.DTOs;

namespace ExplorerApp.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<FavoriteDto>> GetAllFavoritesAsync();
        Task<FavoriteDto> AddFavoriteAsync(CreateFavoriteDto dto);
        Task<bool> DeleteFavoriteAsync(int favoriteId);
    }
}