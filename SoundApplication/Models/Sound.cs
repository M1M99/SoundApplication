namespace SoundApplication.Models
{
    public class Sound
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string AuthorId { get; set; }
        public decimal Length { get; set; }
        public string Category { get; set; }
        public int SampleRate { get; set; }
        public int Likes { get; set; }
        public int Downloads { get; set; }
        public bool IsActive { get; set; } = true;
        public int PlayCount { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
