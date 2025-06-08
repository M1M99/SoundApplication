using SoundApplication.Models;

namespace SoundApplication.Dtos
{
    public class PlaylistDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PlaylistItemDto> Sounds { get; set; }
    }

    public class PlaylistItemDto
    {
        public string SoundId { get; set; }
        public DateTime AddedAt { get; set; }
        public Sound Sound { get; set; }
    }

}
