namespace ExplorerApp.Shared.DTOs
{
    public class FavoriteDto
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string FlagUrl { get; set; } = string.Empty;
        public DateTime AddedDate { get; set; }
    }
}