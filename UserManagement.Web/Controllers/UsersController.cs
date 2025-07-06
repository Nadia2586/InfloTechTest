using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using System.Threading.Tasks;

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
}
