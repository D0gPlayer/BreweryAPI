using BreweryAPI.Data;
using BreweryAPI.Models.Auth;
using BreweryAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MockQueryable.Core;
using MockQueryable.Moq;
using Moq;

namespace BreweryTests
{
    [TestClass]
    public class AuthServiceTests
    {
        private Mock<IRepository<User>> _userRepositoryMock;
        private Mock<IPasswordHasher<User>> _passwordHasherMock;
        private Mock<IConfiguration> _configurationMock;
        private AuthService _authService;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.SetupGet(x => x[It.Is<string>(s => s == "AppSettings:Token")])
                .Returns("SuperSuperSecretSecretKeyKeyThatThatIsIsSuperSuperVeryVeryMuchLongLongAndSecure!");
            _configurationMock.SetupGet(x => x[It.Is<string>(s => s == "AppSettings:Issuer")]).Returns("TestIssuer");
            _configurationMock.SetupGet(x => x[It.Is<string>(s => s == "AppSettings:Audience")]).Returns("TestAudience");

            _authService = new AuthService(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _configurationMock.Object);
        }

        [TestMethod]
        public async Task Login_UserNotFound_ReturnsEmptyString()
        {
            // Arrange
            var userDto = new UserDTO { UserName = "nonexistent", PassWord = "any" };
            var emptyList = new List<User>().AsQueryable();
            var mockDbSet = emptyList.BuildMockDbSet(); // using MockQueryable.Moq

            _userRepositoryMock.Setup(r => r.DbSet).Returns(mockDbSet.Object);

            // Act
            var token = await _authService.Login(userDto);

            // Assert
            Assert.AreEqual(string.Empty, token);
        }

        [TestMethod]
        public async Task Login_PasswordVerificationFails_ReturnsEmptyString()
        {
            // Arrange
            var userDto = new UserDTO { UserName = "john", PassWord = "wrongpassword" };
            var user = new User { UserName = "john", PassWordHash = "hashedPassword" };
            var list = new List<User> { user }.AsQueryable();
            var mockDbSet = list.BuildMockDbSet();

            _userRepositoryMock.Setup(r => r.DbSet).Returns(mockDbSet.Object);
            _passwordHasherMock.Setup(ph => ph.VerifyHashedPassword(user, user.PassWordHash, userDto.PassWord))
                .Returns(PasswordVerificationResult.Failed);

            // Act
            var token = await _authService.Login(userDto);

            // Assert
            Assert.AreEqual(string.Empty, token);
        }

        [TestMethod]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var userDto = new UserDTO { UserName = "john", PassWord = "correctpassword" };
            var user = new User
            {
                UserName = "john",
                PassWordHash = "hashedPassword",
                Id = Guid.NewGuid()
            };
            var list = new List<User> { user }.AsQueryable();
            var mockDbSet = list.BuildMockDbSet();

            _userRepositoryMock.Setup(r => r.DbSet).Returns(mockDbSet.Object);
            _passwordHasherMock.Setup(ph => ph.VerifyHashedPassword(user, user.PassWordHash, userDto.PassWord))
                .Returns(PasswordVerificationResult.Success);

            // Act
            var token = await _authService.Login(userDto);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(token));
        }

        [TestMethod]
        public async Task Register_ValidUser_ReturnsTrue()
        {
            // Arrange
            var userDto = new UserDTO { UserName = "jane", PassWord = "password123" };
            _userRepositoryMock.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(true);
            _passwordHasherMock.Setup(ph => ph.HashPassword(It.IsAny<User>(), userDto.PassWord))
                .Returns("hashedPassword");

            // Act
            var result = await _authService.Register(userDto);

            // Assert
            Assert.IsTrue(result);
            _userRepositoryMock.Verify(r => r.Add(
                It.Is<User>(u => u.UserName == userDto.UserName && u.PassWordHash == "hashedPassword")),
                Times.Once);
        }

        [TestMethod]
        public async Task Register_RepositoryFails_ReturnsFalse()
        {
            // Arrange
            var userDto = new UserDTO { UserName = "jane", PassWord = "password123" };
            _userRepositoryMock.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(false);
            _passwordHasherMock.Setup(ph => ph.HashPassword(It.IsAny<User>(), userDto.PassWord))
                .Returns("hashedPassword");

            // Act
            var result = await _authService.Register(userDto);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
