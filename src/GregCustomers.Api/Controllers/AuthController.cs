using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GregCustomers.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IConfiguration config) : ControllerBase
{
    public record LoginRequest(string Username, string Password);

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Auth simples (POC) - em produção usaria Identity/DB/Hash
        var (ok, role) = request switch
        {
            { Username: "admin",  Password: "admin123"  } => (true, "admin"),
            { Username: "reader", Password: "reader123" } => (true, "reader"),
            _ => (false, "")
        };

        if (!ok)
            return Unauthorized();

        var jwt = config.GetSection("Jwt");
        var issuer = jwt["Issuer"]!;
        var audience = jwt["Audience"]!;
        var key = jwt["Key"]!;
        var expiresMinutes = int.Parse(jwt["ExpiresMinutes"]!);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, request.Username),
            new(ClaimTypes.Name, request.Username),
            new(ClaimTypes.Role, role)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: creds
        );

        return Ok(new
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(token),
            token_type = "Bearer",
            expires_in = expiresMinutes * 60
        });
    }
}