using System;
using System.Collections.Generic;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        throw new NotImplementedException();
    }

    // public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>(); removed to create new method GetAllAsync below

    public async Task<List<User>> GetAllAsync()
    {
        // Using async method to retrieve all users
        return await _dataAccess.GetAll<User>().ToListAsync();
    }

    public void Create(User user)
    {
        _dataAccess.Create(user);
    }

    public void Update(User user)
    {
        _dataAccess.Update(user);
    }

    public void Delete(long id)
    {
        var user = _dataAccess.GetAll<User>().FirstOrDefault(u => u.Id == id);
        if (user is not null)
        {
            _dataAccess.Delete(user);
        }
    }

    public void DeleteConfirm(long id)
    {
        var user = _dataAccess.GetAll<User>().FirstOrDefault(u => u.Id == id);
        if (user is not null)
        {
            _dataAccess.Delete(user);
        }
    }


}
