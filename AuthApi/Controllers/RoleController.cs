using AuthApi.Data;
using AuthApi.DTOs;
using AuthApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RoleController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;


    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles= await _context.Roles
            .Select(r=>new { r.Id, r.Name })
            .OrderBy(r=>r.Name)
            .ToListAsync();
        return Ok(roles);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto dto)
    {
        if (await _context.Roles.AnyAsync(r => r.Name == dto.Name))
            return Conflict(new { message = "Role name already exists." });

        var role = new Role
        {
            Name = dto.Name,
            Description = dto.Description?.Trim()
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return Ok(new{message = "Role created successfully", id = role.Id});
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleCreateDto dto)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return NotFound(new { message = "Role not found." });

        role.Name = dto.Name.Trim();
        role.Description = dto.Description?.Trim();

        await _context.SaveChangesAsync();

        return Ok(new { message = "Role updated successfully." });
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return NotFound(new { message = "Role not found." });

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Role deleted successfully." });
    }

}