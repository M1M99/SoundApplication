using SoundApplication.Models;

namespace SoundApplication.Services.Abstract
{
    public interface ISoundService
    {
        Task<List<Sound>> GetAllSounds();
        Task<Sound> GetSoundsById(string soundId);
        Task<bool> SoftDelete(string soundId);
        Task<bool> HardDelete(string soundId);
        Task<Sound> Update (string soundId, Sound newSound);
        Task<List<Sound>> GetSoundsByAuthorId (string authorId);
        int GetCountByAuthorId(string authorId);
        Task AddSound(Sound sound); 
        Task<string> GetSoundUrlById(string soundId);
    }
}
