using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoundApplication.Dtos;
using SoundApplication.Services.Abstract;
using SoundApplication.Services.Concrete;

namespace SoundApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlayListService _playListService;

        public PlaylistController(IPlayListService plaListService)
        {
            _playListService = plaListService;
        }
        [HttpGet("PlayList/{userId}")]
        public async Task<IActionResult> GetUserPlaylists(string userId)
        {
            var playlists = await _playListService.GetUserPlaylistsAsync(userId);
            return Ok(playlists);
        }
        [HttpGet("GetSoundsAuthor")]
        public async Task<ActionResult> GetList(string userId)
        {
            return Ok(await _playListService.GetUserPlaylistsAsync(userId));
        }

        [HttpPost("{playlistId}/add")]
        public async Task<IActionResult> AddSound(string     playlistId, [FromBody] string soundId)
        {
            var result = await _playListService.AddSoundToPlaylistAsync(playlistId, soundId);
            if (!result)
                return BadRequest("Bad req");

            return Ok("Sound added.");
        }

        [HttpGet("GetSoundByListId/{listId}")]
        public async Task<ActionResult<PlaylistDto>> GetAllSound(string listId)
        {
            return Ok(await _playListService.GetPlaylistWithSoundsAsync(listId));
        }
        [HttpPost("AddNewList")]
        public async Task<ActionResult> PostNewList([FromBody]CreateListDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.UserId) || string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest("UserId and Title are required.");
            await _playListService.CreatePlaylistAsync(dto.UserId,dto.Title);
            return Ok();
        }
        [HttpDelete("DeletePlaylist/{playlistId}")]
        public async Task<IActionResult> DeletePlaylist(string playlistId, [FromQuery] string userId)
        {
            var success = await _playListService.DeletePlaylistAsync(playlistId, userId);
            if (success) return Ok();
            return NotFound("Playlist not found or you don't have permission.");
        }

        [HttpPost("AddSoundToPlaylist")]
        public async Task<IActionResult> AddSoundToPlaylist([FromBody] AddSoundDto dto)
        {
            var success = await _playListService.AddSoundToPlaylistAsync(dto.PlaylistId, dto.SoundId);
            if (success) return Ok();
            return BadRequest("Failed to add sound to playlist.");
        }

    }
}
