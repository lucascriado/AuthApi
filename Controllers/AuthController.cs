using AuthApi.Data;
using AuthApi.Models;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AuthApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (await _db.Users.AnyAsync(u => u.Username == user.Username))
            return BadRequest("Usuário já existe.");

        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.PasswordHash));
        user.PasswordHash = Convert.ToBase64String(hash);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok("Usuário registrado.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null) return Unauthorized("Usuário inválido.");

        using var sha256 = SHA256.Create();
        var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(request.PasswordHash)));

        if (user.PasswordHash != hash)
            return Unauthorized("Senha incorreta.");

        var token = TokenService.GenerateToken(user.Username, _config["JwtKey"]!);
        return Ok(new { token });
    }

    [HttpGet("protected")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult Protected()
    {
        return Ok("Você está autenticado.");
    }
}
