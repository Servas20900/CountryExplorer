namespace ExplorerApp.Shared.DTOs
{
    public class CreateFavoriteDto
    {
        public string CountryCode { get; set; } = string.Empty;
        public string CommonName { get; set; } = string.Empty;
        public string FlagPngUrl { get; set; } = string.Empty;
    }
}