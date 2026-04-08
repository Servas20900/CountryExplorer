namespace ExplorerApp.Infrastructure.Models
{
    public class UserNote
    {
        public int Id { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string NoteText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}