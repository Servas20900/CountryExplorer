namespace ExplorerApp.Shared.DTOs
{
    public class SaveCountryResultDto
    {
        public bool Guardado { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public CountryDto Pais { get; set; } = new();
    }
}
