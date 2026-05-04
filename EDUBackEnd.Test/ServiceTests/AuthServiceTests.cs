using EDUBackEnd.Data;
using EDUBackEnd.Interfaces.Email;
using EDUBackEnd.Interfaces.Timetable.School;
using EDUBackEnd.Models;
using EDUBackEnd.Services.Auth;
using EDUBackEnd.Services.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EDUBackEnd.Test.ServiceTests
{
    public class AuthServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockSchoolService = new Mock<ICurrentSchoolService>();
            mockSchoolService.Setup(s => s.SchoolId).Returns(1);

            return new AppDbContext(mockSchoolService.Object, options);
        }

        [Fact]
        public async Task ForgotPasswordAsync_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var context = GetDbContext();
            // ALWAYS mock the interface (IForgotPassword or IEmailService)
            var mockEmailService = new Mock<IForgotPassword>();
            var mockIConfiguration = new Mock<IConfiguration>();

            var authService = new AuthService(context, mockEmailService.Object,mockIConfiguration.Object);

            // Act
            // Line 28 in your Service likely fails here if you don't check for null
            var result = await authService.SendPasswordResetEmail("nonexistent@example.com");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ForgotPasswordAsync_UserExists_SetsTokenAndSendsEmail()
        {
            // Arrange
            var context = GetDbContext();
            var mockEmailService = new Mock<IForgotPassword>();
            var mockIConfiguration = new Mock<IConfiguration>();

            var user = new Models.Users.User
            {
                Email = "test@example.com",
                UserName = "TestUser",
                SchoolId = 1, // Match the ID from GetDbContext
                ClassId = 1
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var authService = new AuthService(context, mockEmailService.Object, mockIConfiguration.Object);

            // Act
            var result = await authService.SendPasswordResetEmail("test@example.com");

            // Assert
            Assert.True(result);
            Assert.NotNull(user.ResetPasswordToken);
            mockEmailService.Verify(x => x.SendPasswordResetEmail(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task ResetPasswordAsync_ValidToken_ClearsTokenAndReturnsTrue()
        {
            // Arrange
            var context = GetDbContext();
            var mockEmailService = new Mock<IForgotPassword>();
            var mockIConfiguration = new Mock<IConfiguration>();
            string validToken = "secure-token";

            var user = new Models.Users.User
            {
                Email = "test@example.com",
                ResetPasswordToken = validToken,
                PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(30),
                ClassId = 1,
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var authService = new AuthService(context, mockEmailService.Object, mockIConfiguration.Object);

            // Act
            var result = await authService.ResetPasswordAsync(validToken, "NewPass123!", "NewPass123!");

            // Assert
            Assert.True(result);
            Assert.Null(user.ResetPasswordToken);
            Assert.Null(user.PasswordResetTokenExpiry);
        }

        [Fact]
        public async Task ResetPasswordAsync_ExpiredToken_ReturnsFalse()
        {
            // Arrange
            var context = GetDbContext();
            var mockEmailService = new Mock<IForgotPassword>();
            var mockIConfiguration = new Mock<IConfiguration>();
            string expiredToken = "expired-token";

            var user = new Models.Users.User
            {
                ResetPasswordToken = expiredToken,
                PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(-10), // 10 mins ago
                ClassId = 1,
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var authService = new AuthService(context, mockEmailService.Object, mockIConfiguration.Object);

            // Act
            var result = await authService.ResetPasswordAsync(expiredToken, "NewPass!", "NewPass!");

            // Assert
            Assert.False(result);
        }
    }
}