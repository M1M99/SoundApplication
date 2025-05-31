using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SoundApplication.Models;
using SoundApplication.Services.Abstract;

namespace SoundApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoundController : ControllerBase
    {
        private readonly ISoundService _soundService;

        public SoundController(ISoundService soundService)
        {
            _soundService = soundService;
        }
        [HttpGet("GetAllSound")]
        public async Task<ActionResult<List<Sound>>> GetAll()
        {
            return (Ok(await _soundService.GetAllSounds()));
        }
        [HttpDelete("SoftDelete")]
        public async Task<ActionResult> SoftDelete(string id)
        {
            var deleted = await _soundService.SoftDelete(id);
            if (deleted)
            {
                return NoContent();
            }
            return BadRequest();
        }
        [HttpDelete("HardDelete")]
        public async Task<ActionResult> HardDelete(string id)
        {
            var deleted = await _soundService.HardDelete(id);
            if (deleted)
            {
                return NoContent();
            }
            return BadRequest();
        }
        [HttpPut("Edit")]
        public async Task<ActionResult> Put(string id, SoundUpdateDto dto)
        {
            var sound = new Sound
            {
                Category = dto.Category,
                Description = dto.Description,
                Downloads = dto.Downloads,
                Id = id,
                FileType = dto.FileType,
                FileUrl = dto.FileUrl,
                IsActive = dto.IsActive,
                Length = dto.Length,
                Likes = dto.Likes,
                PlayCount = dto.PlayCount,
                PublishDate = dto.PublishDate,
                SampleRate = dto.SampleRate,
                Title = dto.Title,
            };
            var data = await _soundService.Update(id,sound);
            if (!data.Id.IsNullOrEmpty())
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
