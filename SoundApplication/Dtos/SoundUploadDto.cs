namespace SoundApplication
{
    public class SoundUploadDto
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public decimal Length { get; set; }
        public int SampleRate { get; set; }
        public string FileType { get; set; }
    }
}