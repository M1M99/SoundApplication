using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Services.Data
{
    public class AuthDbContext : IdentityDbContext<IdentityUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> opt):
            base(opt)
        {
        }
        //DbSet<IdentityUser> Users { get; set; }
    }
}
