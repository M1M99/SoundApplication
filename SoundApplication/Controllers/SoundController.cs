using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SoundApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoundController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            var list = new List<string> {
                "sa",
                "os",
            };
            return Ok(list);
        }
        [HttpGet("{id}")]
        public ActionResult GetAllById(int id)
        {
            var list = new List<string> {
                $"{id}",
                "sa",
                "os",
            };
            return Ok(list);
        }
    }
}