using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Web.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.WebMS.Controllers;
using System.Linq;
using System;


namespace UserManagement.Web.Tests;

public class UserControllerTests
{
    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange
        var users = SetupUsers(); // this now returns a List<User>
        _userService
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(users); // properly mocked async return

        var controller = CreateController();

        // Act
        var result = await controller.List();
        var model = (result as ViewResult)?.Model as UserListViewModel;

        // Assert
        model.Should().BeOfType<UserListViewModel>();
        model.Items.Should().BeEquivalentTo(
            users.Select(u => new UserListItemViewModel
            {
                Id = u.Id,
                Forename = u.Forename,
                Surname = u.Surname,
                DateOfBirth = u.DateOfBirth,
                Email = u.Email,
                IsActive = u.IsActive
            })
        );
    }

    private List<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        return new List<User>
        {
            new User
            {
                Id = 1,
                Forename = forename,
                Surname = surname,
                DateOfBirth = new DateTime(1990, 5, 1),
                Email = email,
                IsActive = isActive
            }
        };
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
