using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new DataContext(options);
            context.Users!.AddRange(SetupUsers());
            context.SaveChanges();

            var service = new UserService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(SetupUsers());
        }

        private List<User> SetupUsers() => new()
        {
            new User
            {
                Id = 1,
                Forename = "Johnny",
                Surname = "User",
                Email = "juser@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(1990, 1, 1)
            }
        };

        [Fact]
        public void Create_ShouldLogCreateAction()
        {
            // Arrange
            var mockContext = new Mock<IDataContext>();
            var service = new UserService(mockContext.Object);

            var user = new User { Id = 1, Forename = "Alice", Surname = "Smith" };

            // Act
            service.Create(user);

            // Assert
            mockContext.Verify(d => d.Create(It.Is<User>(u => u.Id == 1)), Times.Once);
            mockContext.Verify(d => d.Log(It.Is<LogEntry>(l =>
                l.UserId == 1 &&
                l.Action == "Create" &&
                (l.Description ?? "").Contains("Alice Smith") // null-safe check
            )), Times.Once);
        }


    }
}
