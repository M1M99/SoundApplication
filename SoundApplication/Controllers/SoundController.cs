using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICloundinaryService _cloundinaryService;

        public SoundController(ISoundService soundService, ICloundinaryService cloundinaryService)
        {
            _soundService = soundService;
            _cloundinaryService = cloundinaryService;
        }
        [HttpGet("GetAllSound")]
        public async Task<ActionResult<List<Sound>>> GetAll()
        {
            return (Ok(await _soundService.GetAllSounds()));
        }

        [HttpGet("GetByAuthorId")]
        public async Task<ActionResult<List<Sound>>> GetSoundByAuthorId(string authorId)
        {
            return (Ok(await _soundService.GetSoundsByAuthorId(authorId)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sound>> GetById(string id)
        {
            return (Ok(await _soundService.GetSoundsById(id)));
        }

        [HttpPost("upload")]
        public async Task<ActionResult> UploadSound(IFormFile file, [FromForm] SoundUploadDto sound)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(Guid.NewGuid().ToString(), "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{uniqueFileName}";

            var soundToSave = new Sound
            {
                Id = Guid.NewGuid().ToString(),
                Title = sound.Title,
                Category = sound.Category,
                Length = sound.Length,
                SampleRate = sound.SampleRate,
                FileUrl = fileUrl,
                FileType = sound.FileType,
                PublishDate = DateTime.UtcNow,
                Likes = 0,
                Downloads = 0,
                PlayCount = 0,
                IsActive = true
            };

            await _soundService.AddSound(soundToSave);
            await _cloundinaryService.UploadFileAsync(file);
            return Ok(new { url = file.FileName, soundId = soundToSave.Id });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> SoftDelete(string id)
        {
            var deleted = await _soundService.SoftDelete(id);
            if (deleted)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("HardDelete/{id}")]
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
            var data = await _soundService.Update(id, sound);
            if (!data.Id.IsNullOrEmpty())
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
