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

        public async Task<List<FavoriteDto>> GetAllFavoritesAsync()
        {
            return await _context.Favorites
                .OrderByDescending(f => f.AddedAt)
                .Select(f => new FavoriteDto
                {
                    Id = f.Id,
                    CountryCode = f.CountryCode,
                    CommonName = f.CommonName,
                    FlagPngUrl = f.FlagPngUrl,
                    AddedAt = f.AddedAt
                })
                .ToListAsync();
        }

        public async Task<FavoriteDto> AddFavoriteAsync(CreateFavoriteDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CountryCode))
            {
                throw new Exception("CountryCode es requerido");
            }

            var normalizedCode = dto.CountryCode.Trim().ToUpperInvariant();

            var alreadyFavorite = await _context.Favorites.AnyAsync(f => f.CountryCode == normalizedCode);
            if (alreadyFavorite) throw new Exception("Este país ya está en favoritos");

            var favorite = new Favorite
            {
                CountryCode = normalizedCode,
                CommonName = dto.CommonName.Trim(),
                FlagPngUrl = dto.FlagPngUrl.Trim(),
                AddedAt = DateTime.UtcNow
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return new FavoriteDto
            {
                Id = favorite.Id,
                CountryCode = favorite.CountryCode,
                CommonName = favorite.CommonName,
                FlagPngUrl = favorite.FlagPngUrl,
                AddedAt = favorite.AddedAt
            };
        }

        public async Task<bool> DeleteFavoriteAsync(int favoriteId)
        {
            var favorite = await _context.Favorites.FindAsync(favoriteId);
            if (favorite == null) return false;

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}