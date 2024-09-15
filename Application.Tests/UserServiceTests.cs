using Application.Extensions;
using Application.Models.UserModels;
using Application.Services;
using AutoFixture;
using Domain.Entities;
using Domain.Enums;
using Domain.Tests;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;

namespace Application.Tests
{
    public class UserServiceTests : SetupTest
    {
        private readonly UserService _userService;

        public UserServiceTests()
        => _userService = new UserService(_unitOfWorkMock.Object, _mapperMock.Object, _jwtServiceMock.Object, _claimServiceMock.Object);

        [Fact]
        public async Task GetUsersAsync_ReturnsPaginationUserModel_WhenUsersExist()
        {
            // Arrange
            var users = _fixture
                .Build<User>()
                .Without(x => x.Orders)
                .CreateMany(100).ToList();

            var pagination = new Pagination<User>
            {
                PageIndex = 1,
                PageSize = 10,
                Items = users,
                TotalItemsCount = users.Count(),
            };

            _unitOfWorkMock.Setup(x => x.UserRepository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(pagination);

            _mapperMock.Setup(x => x.Map<Pagination<UserModel>>(It.IsAny<Pagination<User>>()))
                .Returns(
                new Pagination<UserModel>
                {
                    PageIndex = 1,
                    PageSize = 10,
                    Items = new List<UserModel> { new UserModel() },
                    TotalItemsCount = 1000
                });

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1000, result.Result!.TotalItemsCount);
            Assert.Single(result.Result!.Items);
            Assert.Equal(200, (int)result.Code);
        }


        [Fact]
        public async Task GetUsersAsync_ReturnsNotFound_WhenNoUsersExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.UserRepository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()!, It.IsAny<Func<IQueryable<User>?, IIncludableQueryable<User?, object>>>()!)!)
                .ReturnsAsync(null as Pagination<User>);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("List is empty!", result.Errors);
            Assert.Equivalent(404, (int)result.Code);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsNotFound_WhenNoUsersExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as User);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Not found user!", result.Errors);
            Assert.Equivalent(404, (int)result.Code);
        }

        [Fact]
        public async Task GetUserProfile_ReturnsNotFound_WhenNoUsersExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(userId);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntityByCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>())).ReturnsAsync(null as User);

            // Act
            var result = await _userService.GetUserProfile();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Not found user!", result.Errors);
            Assert.Equivalent(404, (int)result.Code);
        }

        // Test for GetUserByIdAsync method
        [Fact]
        public async Task GetUserByIdAsync_ReturnsUserModel_WhenUserExists()
        {
            // Arrange
            #region Fake Data User
            var user = new User
            {
                Email = "Toanmnh2002@gmail.com",
                Password = "$2a$11$l9kCBg7x7MIaQkIv0gR7Ve.Q89G1EaLZUqW3WXsX7qKRJklzGi522",
                Phone = "0123456789",
                FullName = "Toan",
                Username = "Toanmnh2002",
                Address = "Australia",
                Images = new List<string> { "image1", "image2" },
                Role = RoleEnum.Admin,
                Orders = []
            };
            #endregion


            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            var userModel = new UserModel();
            _mapperMock.Setup(x => x.Map<UserModel>(It.IsAny<User>())).Returns(userModel);

            // Act
            var result = await _userService.GetUserByIdAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userModel, result.Result);
            Assert.Equal(200, (int)result.Code);
        }

        // Test for GetUserProfile method
        [Fact]
        public async Task GetUserProfile_ReturnsUserModel_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Toan",
                Username = "ToanNM",
                Phone = "0987654321",
                Password = "$2a$11$l9kCBg7x7MIaQkIv0gR7Ve.Q89G1EaLZUqW3WXsX7qKRJklzGi522",
                Email = "Toanmnh2002@gmail.com",
                Address = "Australia",
                Role = RoleEnum.Admin,
                Images = new List<string> { "image1", "image2" },
                Orders = []
            };
            _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(user.Id);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntityByCondition(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>())).ReturnsAsync(user);
            var userModel = new UserModel();
            _mapperMock.Setup(x => x.Map<UserModel>(It.IsAny<User>())).Returns(userModel);

            // Act
            var result = await _userService.GetUserProfile();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userModel, result.Result);
            Assert.Equal(200, (int)result.Code);
        }

        [Fact]
        public async Task GetUserProfile_ReturnsNotFound_WhenUserNotLoggedIn()
        {
            // Arrange
            _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(Guid.Empty);

            // Act
            var result = await _userService.GetUserProfile();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Not login yet!", result.Errors);
            Assert.Equivalent(404, (int)result.Code);
        }



        // Test for LoginAsync method
        [Fact]
        public async Task LoginAsync_ReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            #region Fake Data User
            var user = new User
            {
                Email = "test@test.com",
                Password = "$2a$11$l9kCBg7x7MIaQkIv0gR7Ve.Q89G1EaLZUqW3WXsX7qKRJklzGi522",
                Phone = "0123456789",
                FullName = "Toan",
                Username = "Toanmnh2002",
                Address = "Australia",
                Images = new List<string> { "image1", "image2" },
                Role = RoleEnum.Admin,
                Orders = []
            };
            #endregion

            var loginModel = new LoginModel { Email = "test@test.com", Password = "12345" };

            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntityByCondition(It.IsAny<Expression<Func<User, bool>>>(), null)).ReturnsAsync(user);
            _jwtServiceMock.Setup(x => x.Verify(loginModel.Password, user.Password)).Returns(true);
            _jwtServiceMock.Setup(x => x.GenerateToken(user)).Returns("token");

            // Act
            var result = await _userService.LoginAsync(loginModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("token", result.Result);
            Assert.Equal(200, (int)result.Code);
        }

        // Test for LoginAsync method
        [Fact]
        public async Task LoginAsync_ReturnsError_WhenCredentialsAreNotValid()
        {
            // Arrange
            #region Fake Data User
            var user = new User
            {
                Email = "test@test.com",
                Password = "$2a$11$l9kCBg7x7MIaQkIv0gR7Ve.Q89G1EaLZUqW3WXsX7qKRJklzGi522",
                Phone = "0123456789",
                FullName = "Toan",
                Username = "Toanmnh2002",
                Address = "Australia",
                Images = new List<string> { "image1", "image2" },
                Role = RoleEnum.Admin,
                Orders = []
            };
            #endregion

            var loginModel = new LoginModel { Email = "test@test.com", Password = "incorrectpassword" };

            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntityByCondition(It.IsAny<Expression<Func<User, bool>>>(), null)).ReturnsAsync(user);
            _jwtServiceMock.Setup(x => x.Verify(loginModel.Password, user.Password)).Returns(false);

            // Act
            var result = await _userService.LoginAsync(loginModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Email or password is incorrect!", result.Errors);
            Assert.Equivalent(404, (int)result.Code);
        }

        // Test for RegisterUserAsync method
        [Fact]
        public async Task RegisterUserAsync_ReturnsUserModel_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerModel = new RegisterModel { Username = "User1", Password = "password" };
            #region Fake Data User
            var user = new User
            {
                Email = "test@test.com",
                Password = "$2a$11$l9kCBg7x7MIaQkIv0gR7Ve.Q89G1EaLZUqW3WXsX7qKRJklzGi522",
                Phone = "0123456789",
                FullName = "Toan",
                Username = "User1",
                Address = "Australia",
                Images = new List<string> { "image1", "image2" },
                Role = RoleEnum.Admin,
                Orders = []
            };
            #endregion

            _mapperMock.Setup(x => x.Map<User>(It.IsAny<RegisterModel>())).Returns(user);
            _jwtServiceMock.Setup(x => x.Salt()).Returns("salt");
            _jwtServiceMock.Setup(x => x.Hash(It.IsAny<string>(), It.IsAny<string>())).Returns("hashedpassword");
            _unitOfWorkMock.Setup(x => x.UserRepository.AddEntityAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var userModel = new UserModel();
            _mapperMock.Setup(x => x.Map<UserModel>(It.IsAny<User>())).Returns(userModel);

            // Act
            var result = await _userService.RegisterUserAsync(registerModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userModel, result.Result);
            Assert.Equal(200, (int)result.Code);
        }

        // Test for RegisterUserAsync method
        [Fact]
        public async Task RegisterUserAsync_ReturnsUserModel_WhenUserIsExisted()
        {
            // Arrange
            var registerModel = new RegisterModel { Username = "User1", Password = "password" };
            #region Fake Data User
            var user = new User
            {
                Email = "test@test.com",
                Password = "$2a$11$l9kCBg7x7MIaQkIv0gR7Ve.Q89G1EaLZUqW3WXsX7qKRJklzGi522",
                Phone = "0123456789",
                FullName = "Toan",
                Username = "User1",
                Address = "Australia",
                Images = new List<string> { "image1", "image2" },
                Role = RoleEnum.Admin,
                Orders = []
            };
            #endregion

            _mapperMock.Setup(x => x.Map<User>(It.IsAny<RegisterModel>())).Returns(user);
            _jwtServiceMock.Setup(x => x.Salt()).Returns("salt");
            _jwtServiceMock.Setup(x => x.Hash(It.IsAny<string>(), It.IsAny<string>())).Returns("hashedpassword");
            _unitOfWorkMock.Setup(x => x.UserRepository.AddEntityAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var userModel = new UserModel();
            _mapperMock.Setup(x => x.Map<UserModel>(It.IsAny<User>())).Returns(userModel);

            // Act
            var result = await _userService.RegisterUserAsync(registerModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userModel, result.Result);
            Assert.Equal(200, (int)result.Code);
        }

        // Test for DeleteUser method
        [Fact]
        public async Task DeleteUser_ReturnsUserModel_WhenDeletionIsSuccessful()
        {
            // Arrange
            #region Fake Data User
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                Password = "$2a$11$l9kCBg7x7MIaQkIv0gR7Ve.Q89G1EaLZUqW3WXsX7qKRJklzGi522",
                Phone = "0123456789",
                FullName = "Toan",
                Username = "User1",
                Address = "Australia",
                Images = new List<string> { "image1", "image2" },
                Role = RoleEnum.Admin,
                Orders = []
            };
            #endregion

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _unitOfWorkMock.Setup(x => x.UserRepository.SoftRemove(It.IsAny<User>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var userModel = new UserModel();
            _mapperMock.Setup(x => x.Map<UserModel>(It.IsAny<User>())).Returns(userModel);

            // Act
            var result = await _userService.DeleteUser(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userModel, result.Result);
            Assert.Equal(200, (int)result.Code);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as User);

            // Act
            var result = await _userService.DeleteUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Not found user!", result.Errors);
            Assert.Equivalent(404, (int)result.Code);
        }
        [Fact]
        public async Task RegisterUserAsync_ReturnsError_WhenUserExists()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Email = "test@test.com",
                Phone = "0123456789",
                Username = "User1",
                Password = "password",
                FullName = "Toan",
                Address = "Australia"
            };
            var user = new User
            {
                Email = "test@test.com",
                Phone = "0123456789",
                Username = "User1",
                Password = "$2a$11$l9kCBg7x7MIaQkIv0gR7Ve.Q89G1EaLZUqW3WXsX7qKRJklzGi522",
                FullName = "Toan",
                Address = "Australia",
                Images = new List<string> { "image1", "image2" },
                Role = RoleEnum.Admin,
                Orders = []
            };

            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntityByCondition(It.IsAny<Expression<Func<User, bool>>>(), null)).ReturnsAsync(user);

            // Act
            var result = await _userService.RegisterUserAsync(registerModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("User is existed!", result.Errors);
            Assert.Equivalent(500, (int)result.Code);
        }
        [Theory]
        [InlineData(100, 10, 10)] // TotalItemsCount is exactly divisible by PageSize
        [InlineData(101, 10, 11)] // TotalItemsCount is not exactly divisible by PageSize
        [InlineData(0, 10, 0)] // TotalItemsCount is zero
        public void TotalPageCount_ReturnsCorrectValue(int totalItemsCount, int pageSize, int expectedTotalPageCount)
        {
            // Arrange
            var pagination = new Pagination<User>
            {
                TotalItemsCount = totalItemsCount,
                PageSize = pageSize
            };

            // Act
            var totalPageCount = pagination.TotalPageCount;

            // Assert
            Assert.Equal(expectedTotalPageCount, totalPageCount);
        }

    }
}
