using System.Collections.Generic;
using UserManagement.Services.Domain.Interfaces;
using System.Linq;
using System;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#pragma warning restore IDE0005 // Using directive is unnecessary.
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Web.Controllers;
using UserManagement.Web.Models.Logs;




namespace UserManagement.Web.Tests;

public class LogControllerTests
{
    [Fact]
    public void LogFull_ShouldReturnPaginatedLogListViewModel()
    {
        // Arrange
        var logs = new List<LogEntry>
    {
        new LogEntry
        {
            Id = 1,
            Timestamp = DateTime.UtcNow,
            Action = "Create",
            Description = "User A was created.",
            User = new User { Forename = "Alice", Surname = "Smith" }
        },
        new LogEntry
        {
            Id = 2,
            Timestamp = DateTime.UtcNow.AddMinutes(-1),
            Action = "Edit",
            Description = "User B was edited.",
            User = new User { Forename = "Bob", Surname = "Jones" }
        }
    }.AsQueryable();

        var mockUserService = new Mock<IUserService>();
        var mockDataContext = new Mock<IDataContext>();

        mockUserService.Setup(s => s.GetAllLogs()).Returns(logs);

        var controller = new LogsController(mockDataContext.Object, mockUserService.Object);

        // Act
        var result = controller.LogFull(null, null, null, page: 1, pageSize: 10);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LogListViewModel>(viewResult.Model);

        model.Logs.Count.Should().Be(2);
        model.CurrentPage.Should().Be(1);
        model.TotalPages.Should().Be(1);
        model.Logs[0].ActionName.Should().Be("Create");
        model.Logs[1].UserName.Should().Be("Bob Jones");
    }

    [Fact]
    public void LogDetails_WithValidId_ShouldReturnViewWithModel()
    {
        // Arrange
        var mockContext = new Mock<IDataContext>();
        var mockUserService = new Mock<IUserService>();

        var logEntry = new LogEntry
        {
            Id = 1,
            Action = "Edit",
            Description = "User updated.",
            Timestamp = DateTime.UtcNow,
            User = new User { Forename = "Test", Surname = "User" }
        };

        mockContext.Setup(c => c.GetAllLogs()).Returns(new List<LogEntry> { logEntry }.AsQueryable());

        var controller = new LogsController(mockContext.Object, mockUserService.Object);

        // Act
        var result = controller.LogDetails(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LogListItemViewModel>(viewResult.Model);
        model.Id.Should().Be(1);
        model.ActionName.Should().Be("Edit");
        model.Description.Should().Be("User updated.");
        model.UserName.Should().Be("Test User");
    }

    [Fact]
    public void LogDetails_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var mockContext = new Mock<IDataContext>();
        var mockUserService = new Mock<IUserService>();

        mockContext.Setup(c => c.GetAllLogs()).Returns(new List<LogEntry>().AsQueryable());

        var controller = new LogsController(mockContext.Object, mockUserService.Object);

        // Act
        var result = controller.LogDetails(999); // Nonexistent ID

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void LogFull_ShouldReturnPaginatedSortedFilteredLogs()
    {
        // Arrange
        var mockContext = new Mock<IDataContext>();
        var mockUserService = new Mock<IUserService>();

        var logs = new List<LogEntry>
    {
        new LogEntry { Id = 1, Action = "Edit", Timestamp = DateTime.UtcNow.AddMinutes(-5), User = new User { Forename = "Alice", Surname = "Smith" } },
        new LogEntry { Id = 2, Action = "Edit", Timestamp = DateTime.UtcNow.AddMinutes(-10), User = new User { Forename = "Bob", Surname = "Brown" } },
        new LogEntry { Id = 3, Action = "Create", Timestamp = DateTime.UtcNow.AddMinutes(-15), User = new User { Forename = "Charlie", Surname = "White" } }
    }.AsQueryable();

        mockUserService.Setup(s => s.GetAllLogs()).Returns(logs);

        var controller = new LogsController(mockContext.Object, mockUserService.Object);

        // Act
        var result = controller.LogFull(actionFilter: "Edit", sortBy: "user", searchTerm: null, page: 1, pageSize: 2);


        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LogListViewModel>(viewResult.Model);

        model.Logs.Should().HaveCount(2); // Should be paginated to 2 results
        model.Logs.All(l => l.ActionName == "Edit").Should().BeTrue(); // Only "Edit" actions
        model.Logs[0].UserName.Should().Be("Alice Smith"); // Sorted by user
        model.CurrentPage.Should().Be(1);
        model.TotalPages.Should().Be(1); // 2 out of 2 edits = 1 page with pageSize=2
    }

    [Fact]
    public void LogFull_WhenNoLogs_ShouldReturnEmptyList()
    {
        // Arrange
        var mockService = new Mock<IUserService>();
        var mockDataContext = new Mock<IDataContext>(); // ✅ Fix: Create a mock context

        mockService.Setup(s => s.GetAllLogs())
                   .Returns(Enumerable.Empty<LogEntry>().AsQueryable());

        var controller = new LogsController(mockDataContext.Object, mockService.Object);

        // Act
        var result = controller.LogFull(actionFilter: null, sortBy: null, searchTerm: null, page: 1, pageSize: 10);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LogListViewModel>(viewResult.Model);

        model.Logs.Should().BeEmpty();
        model.CurrentPage.Should().Be(1);
        model.TotalPages.Should().Be(0);
    }

    [Fact]
    public void LogDetails_WhenLogExists_ShouldReturnViewWithLog()
    {
        // Arrange
        var mockLog = new LogEntry
        {
            Id = 1,
            Timestamp = DateTime.UtcNow,
            Action = "Edit",
            Description = "Test description",
            User = new User { Forename = "John", Surname = "Doe" }
        };

        var mockDataContext = new Mock<IDataContext>();
        var mockUserService = new Mock<IUserService>();

        mockDataContext.Setup(dc => dc.GetAllLogs())
                       .Returns(new List<LogEntry> { mockLog }.AsQueryable());

        var controller = new LogsController(mockDataContext.Object, mockUserService.Object);

        // Act
        var result = controller.LogDetails(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LogListItemViewModel>(viewResult.Model);

        model.Id.Should().Be(1);
        model.ActionName.Should().Be("Edit");
        model.Description.Should().Be("Test description");
        model.UserName.Should().Be("John Doe");
    }

    [Fact]
    public void LogDetails_WhenLogDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var mockDataContext = new Mock<IDataContext>();
        var mockUserService = new Mock<IUserService>();

        // Simulate no logs found
        mockDataContext.Setup(dc => dc.GetAllLogs())
                       .Returns(Enumerable.Empty<LogEntry>().AsQueryable());

        var controller = new LogsController(mockDataContext.Object, mockUserService.Object);

        // Act
        var result = controller.LogDetails(999); // Invalid log ID

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }


}
