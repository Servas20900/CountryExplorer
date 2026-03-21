namespace ExplorerApp.Shared.DTOs
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Capital { get; set; } = string.Empty;
        public long Population { get; set; }
        public string Region { get; set; } = string.Empty;
        public string FlagUrl { get; set; } = string.Empty;
    }
}