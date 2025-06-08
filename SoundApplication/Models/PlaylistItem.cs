namespace SoundApplication.Models
{
    public class PlaylistItem
    {
        public string PlayListId { get; set; }
        
        public string SoundId { get; set; } 
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public PlayList PlayList { get; set; }
        public Sound Sound { get; set; }
    }
}
