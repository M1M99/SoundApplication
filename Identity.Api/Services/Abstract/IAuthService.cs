using Identity.Api.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Services.Abstract
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto dto);
        Task<IdentityResult> RegisterAsync(RegisterDto dto);
        Task<bool> LogoutAsync();
    }
}
