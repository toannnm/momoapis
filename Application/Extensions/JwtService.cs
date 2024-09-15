using Application.Interfaces.IExtensionServices;
using Application.Models.Settings;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Extensions
{
    public class JwtService : IJwtService
    {
        private readonly JwtSection _jwtSection;

        public JwtService(IOptions<JwtSection> jwt) => _jwtSection = jwt.Value;

        public string Hash(string password, string salt) => BCrypt.Net.BCrypt.HashPassword(password, salt);

        public bool Verify(string password, string stringVerify) => BCrypt.Net.BCrypt.Verify(password, stringVerify);

        public string Salt() => BCrypt.Net.BCrypt.GenerateSalt();

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSection.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expiredToken = DateTime.UtcNow.AddDays(_jwtSection.ExpiresInDays);
            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role,user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,expiredToken.ToString("yyyy/MM/dd hh:mm:ss"),ClaimValueTypes.String),
            };
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: expiredToken,
                    signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
