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
        public DbSet<PlayList> PlayLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlaylistItem>()
                .HasKey(pi => new { pi.PlayListId, pi.SoundId });

            modelBuilder.Entity<PlaylistItem>()
                .HasOne(pi => pi.PlayList)
                .WithMany(p => p.Sounds)
                .HasForeignKey(pi => pi.PlayListId);

            modelBuilder.Entity<PlaylistItem>()
                .HasOne(pi => pi.Sound)
                .WithMany()
                .HasForeignKey(pi => pi.SoundId);
        }

    }
}
