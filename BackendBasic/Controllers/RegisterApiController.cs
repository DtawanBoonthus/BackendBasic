using BackendBasic.Data;
using BackendBasic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendBasic.Controllers;

[ApiController]
[Route("api/register")]
public class RegisterApiController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet("get-users")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await dbContext.UserDb
            .AsNoTracking()
            .Select(u => new User
            {
                ID = u.ID,
                Username = u.Username,
                Password = u.Password,
            })
            .ToListAsync();
        
        return Ok(users is {Count: <= 0} ? Enumerable.Empty<User>() : users);
    }

    [HttpGet("get-user/{id:int}")]
    public async Task<ActionResult<User?>> GetUserById(int id)
    {
        var user = await dbContext.UserDb.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id).ConfigureAwait(false);

        return user is not null ? Ok(user) : NotFound();
    }

    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(user.Username) ||
            string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest("Username and password are required");
        }

        var existingUser = await dbContext.UserDb
            .FirstOrDefaultAsync(u => u.Username == user.Username)
            .ConfigureAwait(false);

        if (existingUser != null)
        {
            return Conflict("Username already exists.");
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        try
        {
            await dbContext.UserDb.AddAsync(user).ConfigureAwait(false);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while creating the user.");
        }

        return CreatedAtAction(nameof(GetUserById), new { id = user.ID }, user);
    }

    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (user.ID == 0 ||
            string.IsNullOrWhiteSpace(user.Username) ||
            string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest("Invalid required");
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        dbContext.UserDb.Update(user);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return Ok();
    }

    [HttpDelete("delete-user/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await dbContext.UserDb.FirstOrDefaultAsync(x => x.ID == id).ConfigureAwait(false);

        if (user is null)
        {
            return NotFound();
        }

        dbContext.UserDb.Remove(user);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
        return Ok();
    }
}