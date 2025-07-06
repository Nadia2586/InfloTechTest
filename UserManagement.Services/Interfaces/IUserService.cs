using System.Collections.Generic;
using UserManagement.Models;
using System.Threading.Tasks;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    IEnumerable<User> FilterByActive(bool isActive);
  

    Task<List<User>> GetAllAsync(); // IEnumerable<User> GetAll() removed - replace with new async method to retrieve all users
}
