using Identity.Api.Dtos;
using Identity.Api.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Api.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<IdentityUser> userManager,
                           SignInManager<IdentityUser> signInManager,
                           IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            return result;
        }

        public async Task<string> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return null;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            return GenerateJwtToken(user, roles);
        }

        private string GenerateJwtToken(IdentityUser user, IList<string> roles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<bool> LogoutAsync()
        {
            throw new Exception();
        }
    }
}