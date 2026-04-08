namespace ExplorerApp.Shared.DTOs
{
    public class DashboardDto
    {
        public int TotalCountriesSaved { get; set; }
        public int TotalFavorites { get; set; }
        public List<FavoriteDto> MostRecentFavorites { get; set; } = new();
        public List<CountryDto> MostRecentCountries { get; set; } = new();
    }
}
