using Microsoft.EntityFrameworkCore;
using SoundApplication.Dtos;
using SoundApplication.Models;
using SoundApplication.Services.Abstract;
using SoundApplication.Services.Data;

namespace SoundApplication.Services.Concrete
{
    public class PlayListService : IPlayListService
    {
        private readonly SoundDbContext _context;

        public PlayListService(SoundDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddSoundToPlaylistAsync(string playlistId, string soundId)
        {
            var playlist = await _context.PlayLists
                                            .Include(p => p.Sounds)
                                            .FirstOrDefaultAsync(p => p.Id == playlistId);
            if (playlist == null) return false;

            if (playlist.Sounds.Any(ps => ps.SoundId == soundId))
                return false;

            playlist.Sounds.Add(new PlaylistItem
            {
                PlayListId = playlistId,
                SoundId = soundId,
                AddedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PlayList> CreatePlaylistAsync(string userId, string title)
        {
            var playlist = new PlayList
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Title = title
            };

            _context.PlayLists.Add(playlist);
            await _context.SaveChangesAsync();
            return playlist;
        }

        public async Task<bool> DeletePlaylistAsync(string playlistId, string userId)
        {
            var playlist = await _context.PlayLists
                                         .FirstOrDefaultAsync(p => p.Id == playlistId && p.UserId == userId);
            if (playlist == null) return false;

            _context.PlayLists.Remove(playlist);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<PlayList> GetPlaylistByIdAsync(string playlistId)
        {
            return await _context.PlayLists
                                 .Include(p => p.Sounds)
                                 .FirstOrDefaultAsync(p => p.Id == playlistId);
        }

        public async Task<PlaylistDto> GetPlaylistWithSoundsAsync(string playlistId)
        {
            var playlist = await _context.PlayLists
                .Include(p => p.Sounds)
                    .ThenInclude(pi => pi.Sound)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist == null) return null;

            return new PlaylistDto
            {
                Id = playlist.Id,
                Title = playlist.Title,
                UserId = playlist.UserId,
                CreatedAt = playlist.CreatedAt,
                Sounds = playlist.Sounds.Select(pi => new PlaylistItemDto
                {
                    SoundId = pi.SoundId,
                    AddedAt = pi.AddedAt,
                    Sound = pi.Sound
                }).ToList()
            };
        }



        public async Task<List<PlayList>> GetUserPlaylistsAsync(string userId)
        {
            return await _context.PlayLists
                                 .Where(p => p.UserId == userId)
                                 .ToListAsync();
        }
        public async Task<bool> RemoveSoundFromPlaylistAsync(string playlistId, string soundId)
        {
            var playlist = await _context.PlayLists
                                         .Include(p => p.Sounds)
                                         .FirstOrDefaultAsync(p => p.Id == playlistId);
            if (playlist == null) return false;

            var item = playlist.Sounds.FirstOrDefault(ps => ps.SoundId == soundId);
            if (item == null) return false;

            playlist.Sounds.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
