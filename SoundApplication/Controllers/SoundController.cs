using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SoundApplication.Dtos;
using SoundApplication.Models;
using SoundApplication.Services.Abstract;
using System.Security.Claims;
using System.Text.Json;

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

        [HttpGet("count/{authorId}")]
        public ActionResult<int> GetTracksCountByAuthorId(string authorId)
        {
            return Ok(_soundService.GetCountByAuthorId(authorId));
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<ActionResult> UploadSound(IFormFile file, [FromForm] SoundUploadDto sound)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized("User ID not found in token.");

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

            var url = await _cloundinaryService.UploadFileAsync(file);

            var soundToSave = new Sound
            {
                Id = Guid.NewGuid().ToString(),
                Title = sound.Title,
                Category = sound.Category,
                Length = sound.Length,
                SampleRate = sound.SampleRate,
                FileUrl = url,
                FileType = sound.FileType,
                PublishDate = DateTime.UtcNow,
                Likes = 0,
                Downloads = 0,
                PlayCount = 0,
                IsActive = true,
                AuthorId = userId
            };

            await _soundService.AddSound(soundToSave);
            string volumePath = "/app/soundUrls";  
            string jsonFilePath = Path.Combine(volumePath, "sounds.json");

            List<SoundUrlDto> soundUrls = new List<SoundUrlDto>();
            if (System.IO.File.Exists(jsonFilePath))
            {
                var existingJson = System.IO.File.ReadAllText(jsonFilePath);
                soundUrls = JsonSerializer.Deserialize<List<SoundUrlDto>>(existingJson) ?? new List<SoundUrlDto>();
            }

            soundUrls.Add(new SoundUrlDto { Id = soundToSave.Id, Url = url });

            System.IO.File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(soundUrls, new JsonSerializerOptions { WriteIndented = true }));

            return Ok(new { url = url, soundId = soundToSave.Id });
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

        [HttpGet("GetSoundURL/{soundId}")]
        public async Task<ActionResult<string>> Get(string soundId)
        {
            return await _soundService.GetSoundUrlById(soundId);
        }

        [HttpGet("GetSoundsByAuthor/{authorId}")]
        public async Task<ActionResult> GetSound(string authorId)
        {
            return Ok(await _soundService.GetSoundsByAuthorId(authorId));
        }
    }
}
