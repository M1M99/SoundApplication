namespace SoundApplication.Models
{
    public class PlayList
    {
        public string Id { get; set; } 
        public string Title { get; set; }  
        public string UserId { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

        public List<PlaylistItem> Sounds { get; set; } = new List<PlaylistItem>();
    }
}
