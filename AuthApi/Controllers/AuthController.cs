using AuthApi.Data;
using AuthApi.DTOs;
using AuthApi.Models;
using AuthApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ApplicationDbContext context, JwtService jwtService) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly PasswordHasher<User> _passwordHasher = new();

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Email and password are required." });

        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return Conflict(new { message = "Email already registered." });

        var role = await _context.Roles.FirstOrDefaultAsync(r=>r.Id==request.RoleId);

        if (role == null)
        {
            return BadRequest(new { messagge = "Invalid Role Selected" });
        }

        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();


        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        };

        return Ok(new { message = "User registered successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
            return Unauthorized(new { message = "Invalid email or password." });

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Invalid email or password" });

        var token = jwtService.GenerateToken(user);
        
        return Ok(new LoginResponse
        {
            Token = token,
            Email = user.Email,
            FullName = user.FullName
        });
    }
}