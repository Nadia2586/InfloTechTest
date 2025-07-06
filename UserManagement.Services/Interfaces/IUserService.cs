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

    void Create(User user);
    void Update(User user);
    void Delete(long id);
    void DeleteConfirm(long id); // Added DeleteConfirm method to match the controller's action


}
