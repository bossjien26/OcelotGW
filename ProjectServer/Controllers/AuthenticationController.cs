using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ProjectServer.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet]
        [Route("JwtToken")]
        public IActionResult JwtToken()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "cash")
            };
            var securityKey = "123730a1-1e99-428b-9f6d-9f3ed4021234";
            var token = new JwtSecurityToken(new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                                                                                  SecurityAlgorithms.HmacSha256)),
                                             new JwtPayload(issuer: "CashUser",
                                                            audience: "CashAudience",
                                                            claims: claims,
                                                            notBefore: DateTime.UtcNow,
                                                            expires: DateTime.UtcNow.AddMinutes(30)));
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { Token = tokenStr });
        }
    }
}