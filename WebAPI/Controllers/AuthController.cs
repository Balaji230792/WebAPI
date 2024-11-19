using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController(IOptions<JWTAuthenticationScheme> jwtAuthenticationScheme) : ControllerBase
    {
        private readonly JWTAuthenticationScheme _jwtAuthenticationScheme = jwtAuthenticationScheme.Value;

        [HttpPost("login")]
        public IActionResult Login([FromQuery]string username,[FromQuery] string password)
        {
            //if(username != "xyz" || password != "abc")
            //{
            //    return Unauthorized();
            //}

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthenticationScheme.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtAuthenticationScheme.ValidIssuer,
                audience: _jwtAuthenticationScheme.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString });

        }
    }
}
