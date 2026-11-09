using Blogy.Business.Dtos.UserDtos;
using Microsoft.AspNetCore.Identity;

namespace Blogy.Business.Services.UserServices;

public interface IUserService
{
    Task<IdentityResult> RegisterAsync(RegisterDto model);
    Task<SignInResult> LoginAsync(LoginDto model);
    Task LogoutAsync();
}
