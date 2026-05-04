using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EDUBackEnd.Interfaces.Auth;
using EDUBackEnd.Models.Users;
using EDUBackEnd.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using System.IdentityModel.Tokens.Jwt;

namespace EDUBackEnd.Tests.Services.Auth
{
    public class TokenServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public TokenServiceTests()
        {
            _mockConfig = new Mock<IConfiguration>();

            // Setting up the mock configuration for JWT
            _mockConfig.Setup(c => c["JWT:Key"]).Returns("ThisIsAVerySecretKeyThatIsAtLeast32CharsLong!");
            _mockConfig.Setup(c => c["JWT:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(c => c["JWT:Audience"]).Returns("TestAudience");

            // UserManager helper: Pass a Mock UserStore to the constructor
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task CreateToken_ShouldReturnValidJwtString()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                Email = "test@example.com",
                SchoolId = 10,
                ClassId= 1
            };

            var roles = new List<string> { "Student", "Admin" };

            _mockUserManager.Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(roles);

            var tokenService = new TokenService(_mockConfig.Object, _mockUserManager.Object);

            // Act
            var tokenString = await tokenService.CreateToken(user);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(tokenString));

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenString);

            // Use the keys shown in your error message (unique_name, nameid, etc.)
            Assert.Contains(jwtToken.Claims, c => c.Type == "unique_name" && c.Value == "testuser");
            Assert.Contains(jwtToken.Claims, c => c.Type == "nameid" && c.Value == user.Id);
            Assert.Contains(jwtToken.Claims, c => c.Type == "email" && c.Value == "test@example.com");

            // Custom claims usually don't get mapped, so this stays the same
            Assert.Contains(jwtToken.Claims, c => c.Type == "SchoolId" && c.Value == "10");

            // Roles are still usually "role" or "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            // Based on your error log, they are "role"
            Assert.Contains(jwtToken.Claims, c => c.Type == "role" && c.Value == "Student");
            Assert.Contains(jwtToken.Claims, c => c.Type == "role" && c.Value == "Admin");

            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Equal("TestAudience", jwtToken.Audiences.First());
        }
    }
}