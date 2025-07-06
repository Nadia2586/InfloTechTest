using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Web.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.WebMS.Controllers;
using System.Linq;
using System;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#pragma warning restore IDE0005 // Using directive is unnecessary.



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

    [Fact]
    public void Create_Post_WithValidModel_ShouldSaveUserAndRedirect()
    {
        // Arrange
        var model = new UserCreateViewModel
        {
            Forename = "Test",
            Surname = "User",
            Email = "test@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1995, 4, 10)
        };

        var mockUserService = new Mock<IUserService>();
        var controller = new UsersController(mockUserService.Object);

        // Act
        var result = controller.Create(model);

        // Assert
        mockUserService.Verify(s => s.Create(It.Is<User>(u =>
            u.Forename == model.Forename &&
            u.Surname == model.Surname &&
            u.Email == model.Email &&
            u.IsActive == model.IsActive &&
            u.DateOfBirth == model.DateOfBirth
        )), Times.Once);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        redirect.ActionName.Should().Be("List");
    }


    [Fact]
    public void Create_Post_WithInvalidModel_ShouldReturnViewWithModel()
    {
        // Arrange
        var model = new UserCreateViewModel(); // Missing required fields
        var mockUserService = new Mock<IUserService>();
        var controller = new UsersController(mockUserService.Object);
        controller.ModelState.AddModelError("Forename", "Required");

        // Act
        var result = controller.Create(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        viewResult.Model.Should().Be(model);
    }


    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);

    [Fact]
    public async Task View_WithValidId_ShouldReturnUserDetailView()
    {
        // Arrange
        var users = new List<User>
    {
        new User
        {
            Id = 1,
            Forename = "Test",
            Surname = "User",
            Email = "test@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1995, 1, 1)
        }
    };

        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);
        var controller = new UsersController(mockService.Object);

        // Act
        var result = await controller.View(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UserDetailViewModel>(viewResult.Model);
        model.Id.Should().Be(1);
        model.Forename.Should().Be("Test");
        model.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task View_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var users = new List<User>(); // Empty list, so user ID won’t be found

        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);
        var controller = new UsersController(mockService.Object);

        // Act
        var result = await controller.View(999); // Non-existent ID

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Get_WithValidId_ShouldReturnUserEditView()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Forename = "Edit",
            Surname = "Me",
            Email = "edit@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1990, 6, 15)
        };

        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<User> { user });
        var controller = new UsersController(mockService.Object);

        // Act
        var result = await controller.Edit(1);

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UserEditViewModel>(view.Model);
        model.Id.Should().Be(1);
        model.Forename.Should().Be("Edit");
    }

    [Fact]
    public async Task Edit_Get_WithInvalidId_ShouldReturnNotFound()
    {
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<User>());
        var controller = new UsersController(mockService.Object);

        var result = await controller.Edit(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Post_WithValidModel_ShouldUpdateUserAndRedirect()
    {
        // Arrange
        var model = new UserEditViewModel
        {
            Id = 1,
            Forename = "Updated",
            Surname = "User",
            Email = "updated@example.com",
            IsActive = false,
            DateOfBirth = new DateTime(2000, 1, 1)
        };

        var mockService = new Mock<IUserService>();
        var controller = new UsersController(mockService.Object);

        // Act
        var result = controller.Edit(1, model);

        // Assert
        mockService.Verify(s => s.Update(It.Is<User>(u =>
            u.Id == model.Id &&
            u.Forename == model.Forename &&
            u.Email == model.Email
        )), Times.Once);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        redirect.ActionName.Should().Be("List");
    }

    [Fact]
    public void Edit_Post_WithInvalidModel_ShouldReturnView()
    {
        // Arrange
        var model = new UserEditViewModel(); // missing required fields
        var mockService = new Mock<IUserService>();
        var controller = new UsersController(mockService.Object);
        controller.ModelState.AddModelError("Forename", "Required");

        // Act
        var result = controller.Edit(1, model);

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        view.Model.Should().Be(model);
    }

    [Fact]
    public async Task Delete_Get_WithValidId_ShouldReturnConfirmationView()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Forename = "Delete",
            Surname = "Target",
            Email = "delete@example.com",
            IsActive = true
        };

        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetAllAsync())
                   .ReturnsAsync(new List<User> { user });

        var controller = new UsersController(mockService.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UserDeleteViewModel>(view.Model);
        model.Id.Should().Be(1);
        model.Forename.Should().Be("Delete");
    }

    [Fact]
    public async Task Delete_Get_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetAllAsync())
                   .ReturnsAsync(new List<User>()); // No users returned

        var controller = new UsersController(mockService.Object);

        // Act
        var result = await controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteConfirm_Post_ShouldCallDeleteAndRedirectToList()
    {
        // Arrange
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.DeleteConfirm(It.IsAny<long>()));

        var controller = new UsersController(mockService.Object);

        // Fake TempData to use as it fails when Unit Testing tries to access TempData in .Net
        var tempData = new TempDataDictionary(
            new DefaultHttpContext(),
            Mock.Of<ITempDataProvider>()
        );
        controller.TempData = tempData;

        // Act
        var result = controller.DeleteConfirm(1);

        // Assert
        mockService.Verify(s => s.Delete(1), Times.Once);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        redirect.ActionName.Should().Be("List");
    }


}
