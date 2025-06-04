namespace SoundApplication.Services.Abstract
{
    public interface ICloundinaryService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}
