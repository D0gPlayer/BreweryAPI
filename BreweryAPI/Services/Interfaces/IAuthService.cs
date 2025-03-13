using BreweryAPI.Models.Auth;

namespace BreweryAPI.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<bool> Register(UserDTO user);
        public Task<string> Login(UserDTO user);
    }
}
