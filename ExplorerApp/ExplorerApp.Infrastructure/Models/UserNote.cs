namespace ExplorerApp.Infrastructure.Models
{
    public class UserNote
    {
        public int Id { get; set; }
        public int FavoriteId { get; set; }
        public string NoteText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Favorite Favorite { get; set; } = null!;
    }
}