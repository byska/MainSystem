using MainSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MainSystem.Api.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userMgr;
        private readonly IConfiguration _cfg;

        public AuthController(UserManager<ApplicationUser> userMgr, IConfiguration cfg)
        {
            _userMgr = userMgr;
            _cfg = cfg;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _userMgr.FindByEmailAsync(dto.Email);
                if (user is null || !await _userMgr.CheckPasswordAsync(user, dto.Password))
                    return Unauthorized("Yanlış e-posta veya parola");

                var roles = await _userMgr.GetRolesAsync(user);

                var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!)
        };
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]));
                var token = new JwtSecurityToken(
                    issuer: _cfg["Jwt:Issuer"],
                    audience: _cfg["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(12),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

                return RedirectToAction("Home","Flights");
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        public record LoginDto(string Email, string Password);
    }
}
