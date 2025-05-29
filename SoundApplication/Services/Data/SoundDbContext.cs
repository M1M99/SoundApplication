using Microsoft.EntityFrameworkCore;
using SoundApplication.Models;

namespace SoundApplication.Services.Data
{
    public class SoundDbContext : DbContext
    {
        public SoundDbContext(DbContextOptions<SoundDbContext> options) :
            base(options)
        {
        }
        public DbSet<Sound> Sounds { get; set; }
    }
}
