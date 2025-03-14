using AutoMapper;
using BreweryAPI.Data;
using BreweryAPI.Models;
using BreweryAPI.Models.Auth;
using BreweryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BreweryAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(IRepository<User> userRepository,
            IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<string> Login(UserDTO userDto)
        {
            var user = await _userRepository.DbSet.FirstOrDefaultAsync(x => x.UserName == userDto.UserName);
            if (user == null) return string.Empty;

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PassWordHash, userDto.PassWord);
            if(verificationResult != PasswordVerificationResult.Success) return string.Empty;

            return CreateToken(user);
        }

        public async Task<bool> Register(UserDTO userDto)
        {
            var user = new User();
            var hashedPassword = _passwordHasher.HashPassword(user, userDto.PassWord);

            user.UserName = userDto.UserName;
            user.PassWordHash = hashedPassword;

            return await _userRepository.Add(user);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())};

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["AppSettings:Token"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
