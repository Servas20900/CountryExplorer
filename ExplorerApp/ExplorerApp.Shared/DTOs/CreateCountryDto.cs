namespace ExplorerApp.Shared.DTOs
{
    public class CreateCountryDto
    {
        public string CountryCode { get; set; } = string.Empty;
        public string CommonName { get; set; } = string.Empty;
        public string OfficialName { get; set; } = string.Empty;
        public string Capital { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Subregion { get; set; } = string.Empty;
        public string FlagPngUrl { get; set; } = string.Empty;
        public string FlagSvgUrl { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}