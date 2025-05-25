using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudyBuddy.Core.Entities;
using StudyBuddy.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudyBuddy.Services.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _config;

        public JwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id),
                new Claim("nameid", user.Id),
                new Claim("fullName", user.FullName),
                new Claim(ClaimTypes.Name, user.UserName)
            };


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return GenerateJwt(claims, _config["Jwt:Key"], minutesValid: 30);
        }

        public string GenerateRefreshToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            return GenerateJwt(claims, _config["Jwt:RefreshTokenKey"] ?? _config["Jwt:Key"], daysValid: 1);
        }

        private string GenerateJwt(IEnumerable<Claim> claims, string? key, int minutesValid = 0, int daysValid = 0)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(minutesValid > 0 ? minutesValid : daysValid * 24 * 60);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
