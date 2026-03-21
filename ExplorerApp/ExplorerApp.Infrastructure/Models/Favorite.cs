namespace ExplorerApp.Infrastructure.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public Country Country { get; set; } = null!;
        public ICollection<UserNote> UserNotes { get; set; } = new List<UserNote>();
    }
}