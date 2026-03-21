namespace ExplorerApp.Shared.DTOs
{
    public class UserNoteDto
    {
        public int Id { get; set; }
        public int FavoriteId { get; set; }
        public string NoteText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}