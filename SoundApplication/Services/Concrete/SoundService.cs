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
            if(sound is null)
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

        public Task<Sound> GetSoundsByAuthorId(string authorId)
        {
            throw new NotImplementedException();
        }

        public async Task<Sound> GetSoundsById(string soundId)
        {
            var data = await _context.Sounds.FirstOrDefaultAsync(s => s.Id == soundId);
            if(data is null)
            {
                throw new KeyNotFoundException("I Cant Found");
            }
            return data;

        }

        public Task<Sound> Update(string soundId)
        {
            throw new NotImplementedException();
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
    }
}
