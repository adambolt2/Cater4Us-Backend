using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cater4Us_Backend.Services
{
    public class JwtTokenService
    {
        private readonly string _secretKey;

        public JwtTokenService(string secretKey)
        {
            _secretKey = secretKey;
        }

        public string GenerateToken(string email, int userId, int role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("UserId", userId.ToString()),
        new Claim(ClaimTypes.Role, role.ToString()) // Convert role to string
    };

            var token = new JwtSecurityToken(
                issuer: "http://localhost",
                audience: "http://localhost",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
