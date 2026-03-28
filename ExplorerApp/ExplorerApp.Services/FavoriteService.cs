using ExplorerApp.Infrastructure.Data;
using ExplorerApp.Infrastructure.Models;
using ExplorerApp.Services.Interfaces;
using ExplorerApp.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExplorerApp.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly AppDbContext _context;

        public FavoriteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FavoriteDto>> GetAllFavoritesAsync()
        {
            return await _context.Favorites
                .Include(f => f.Country)
                .Select(f => new FavoriteDto
                {
                    Id = f.Id,
                    CountryId = f.CountryId,
                    CountryName = f.Country.Name,
                    FlagUrl = f.Country.FlagUrl,
                    AddedDate = f.AddedDate
                })
                .ToListAsync();
        }

        public async Task AddFavoriteAsync(int countryId)
        {
            var countryExists = await _context.Countries.AnyAsync(c => c.Id == countryId);
            if (!countryExists) throw new Exception("País no encontrado");

            var alreadyFavorite = await _context.Favorites.AnyAsync(f => f.CountryId == countryId);
            if (alreadyFavorite) throw new Exception("Este país ya está en favoritos");

            var favorite = new Favorite
            {
                CountryId = countryId,
                AddedDate = DateTime.UtcNow
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFavoriteAsync(int favoriteId)
        {
            var favorite = await _context.Favorites.FindAsync(favoriteId);
            if (favorite == null) throw new Exception("Favorito no encontrado");

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }
}