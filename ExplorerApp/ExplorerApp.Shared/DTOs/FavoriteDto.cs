namespace ExplorerApp.Shared.DTOs
{
    public class FavoriteDto
    {
        public int Id { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string CommonName { get; set; } = string.Empty;
        public string FlagPngUrl { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; }
    }
}