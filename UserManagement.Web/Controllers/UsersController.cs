using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using System.Threading.Tasks;
using UserManagement.Models;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using System;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public async Task<ViewResult> List(string filter = "all") // Updated to allow the filter buttons to work properly
    {
        var users = await _userService.GetAllAsync();

        var filtered = filter.ToLower() switch
        {
            "active" => users.Where(u => u.IsActive),
            "inactive" => users.Where(u => !u.IsActive),
            _ => users
        };

        var items = filtered.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            DateOfBirth = p.DateOfBirth,
            Email = p.Email,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.ToList(),
            CurrentFilter = filter.ToLower()
        };

        return View(model);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public IActionResult Create(UserCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User
        {
            Forename = model.Forename!,
            Surname = model.Surname!,
            Email = model.Email!,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };

        _userService.Create(user); // or _dataContext.Create(user) depending on your structure

        return RedirectToAction("List");
    }

    [HttpGet("view/{id}")]
    public async Task<IActionResult> View(long id)
    {
        var user = (await _userService.GetAllAsync()).FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserDetailViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(model);
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(long id)
    {
        var user = (await _userService.GetAllAsync()).FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserEditViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(model);
    }

    [HttpPost("edit/{id}")]
    public IActionResult Edit(long id, UserEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var updatedUser = new User
        {
            Id = model.Id,
            Forename = model.Forename!,
            Surname = model.Surname!,
            Email = model.Email!,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };

        _userService.Update(updatedUser);

        return RedirectToAction("List");
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var user = (await _userService.GetAllAsync()).FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserDeleteViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname
        };

        return View(model);
    }

    [HttpPost("delete/{id}")]
    public IActionResult DeleteConfirm(long id)
    {
        _userService.Delete(id);
        TempData["SuccessMessage"] = "User deleted successfully.";
        return RedirectToAction("List");
    }



}
