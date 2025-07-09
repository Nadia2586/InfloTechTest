using Microsoft.AspNetCore.Mvc;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using Microsoft.EntityFrameworkCore;
#pragma warning restore IDE0005 // Using directive is unnecessary.
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Shared.Dto;

namespace UserManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IDataContext _dataContext;

    public UsersController(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    public IEnumerable<UserDto> GetAllUsers()
    {
        var users = _dataContext.GetAll<User>().ToList();

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth ?? DateTime.MinValue
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(long id)
    {
        var user = await _dataContext.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        var result = new UserDto
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth ?? default(DateTime),
            IsActive = user.IsActive
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(UserDto userDto)
    {
        var user = new User
        {
            Forename = userDto.Forename,
            Surname = userDto.Surname,
            Email = userDto.Email,
            DateOfBirth = userDto.DateOfBirth,
            IsActive = userDto.IsActive
        };

        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();

        userDto.Id = user.Id;

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, userDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserDto userDto)
    {
        var user = await _dataContext.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        user.Forename = userDto.Forename;
        user.Surname = userDto.Surname;
        user.Email = userDto.Email;
        user.DateOfBirth = userDto.DateOfBirth;
        user.IsActive = userDto.IsActive;

        await _dataContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _dataContext.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        _dataContext.Users.Remove(user);
        await _dataContext.SaveChangesAsync();

        return NoContent();
    }



}
