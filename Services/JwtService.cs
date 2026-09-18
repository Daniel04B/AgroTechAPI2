using AgroTechAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgroTechAPI.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(Agricultor agricultor)
        {
            var jwtSettings =
                _configuration.GetSection("Jwt");

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        jwtSettings["Key"]!));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    agricultor.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    agricultor.Usuario),

                new Claim(
                    ClaimTypes.Role,
                    agricultor.Rol ?? "Usuario")
            };

            var token =
                new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(
                        Convert.ToDouble(
                            jwtSettings["DurationInMinutes"])),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}