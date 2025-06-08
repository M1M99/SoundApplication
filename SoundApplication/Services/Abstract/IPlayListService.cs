using SoundApplication.Dtos;
using SoundApplication.Models;

namespace SoundApplication.Services.Abstract
{
    public interface IPlayListService
    {
        Task<PlayList> CreatePlaylistAsync(string userId, string title);
        Task<PlayList> GetPlaylistByIdAsync(string playlistId);
        Task<PlaylistDto> GetPlaylistWithSoundsAsync(string playlistId);
        Task<List<PlayList>> GetUserPlaylistsAsync(string userId);
        Task<bool> DeletePlaylistAsync(string playlistId, string userId);
        Task<bool> AddSoundToPlaylistAsync(string playlistId, string soundId);
        Task<bool> RemoveSoundFromPlaylistAsync(string playlistId, string soundId);
    }
}
