using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SoundApplication.Models;
using SoundApplication.Services.Abstract;
using SoundApplication.Services.Data;

namespace SoundApplication.Services.Concrete
{
    public class SoundService : ISoundService
    {
        private readonly SoundDbContext _context;

        public SoundService(SoundDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SoftDelete(string soundId)
        {
            var sound = await _context.Sounds.FirstOrDefaultAsync(a => a.Id == soundId);
            if (sound is null)
            {
                throw new KeyNotFoundException($"Sound with id '{soundId}' not found.");
            }

            if (!sound.IsActive)
            {
                return false;
            }
            sound.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Sound>> GetAllSounds()
        {
            var list = await _context.Sounds.Where(a => a.IsActive == true).ToListAsync();
            return list;
        }

        public async Task<List<Sound>> GetSoundsByAuthorId(string authorId)
        {
            var data = await _context.Sounds.Where(s => s.AuthorId == authorId).ToListAsync();
            return data;
        }

        public async Task<Sound> GetSoundsById(string soundId)
        {
            var data = await _context.Sounds.FirstOrDefaultAsync(s => s.Id == soundId);
            if (data is null)
            {
                throw new KeyNotFoundException("I Cant Found");
            }
            return data;

        }

        public async Task<Sound> Update(string soundId, Sound newSound)
        {
            var data = _context.Sounds.FirstOrDefault(s => s.Id == soundId);
            if (data is not null)
            {
                data.Category = newSound.Category;
                data.Description = newSound.Description;
                data.Downloads = newSound.Downloads;
                data.FileType = newSound.FileType;
                data.FileUrl = newSound.FileUrl;
                data.IsActive = newSound.IsActive;
                data.Length = newSound.Length;
                data.Likes = newSound.Likes;
                data.PlayCount = newSound.PlayCount;
                data.PublishDate = newSound.PublishDate;
                data.SampleRate = newSound.SampleRate;
                data.Title = newSound.Title;
                data.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return (data);
            }
            return (new Sound());
        }

        public async Task<bool> HardDelete(string soundId)
        {
            var data = await _context.Sounds.FirstOrDefaultAsync(a => a.Id == soundId);
            if (data is null)
            {
                throw new KeyNotFoundException("Bad Request");
            }
            _context.Sounds.Remove(data);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task AddSound(Sound sound)
        {
            await _context.Sounds.AddAsync(sound);
            await _context.SaveChangesAsync();
        }

        public int GetCountByAuthorId(string authorId)
        {
            return _context.Sounds.Where(a => a.AuthorId == authorId).Count();
        }

        public async Task<string> GetSoundUrlById(string soundId)
        {
            var sound = await _context.Sounds.FirstOrDefaultAsync(s => s.Id == soundId);
            if (sound is not null)
            {
                return sound.FileUrl;
            }
            return "";
        }
    }
}
