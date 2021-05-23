using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

using PhotoGallery.BLL.Configuration;
using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.Exceptions;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.BLL.Services;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;
using PhotoGallery.Tests.ServiceTests.Base;

namespace PhotoGallery.Tests.ServiceTests
{
    public class UserServiceTests : ServiceTestsBase
    {
        IMapper mapper;
        JwtSettings jwtSettings;

        Mock<IServiceHelper> mockHelper;
        Mock<IUnitOfWork> mockUow;       

        UserService service;

        public UserServiceTests()
        {
            mapper = CreateMapperProfile();

            mockUow = new Mock<IUnitOfWork>();
            mockHelper = new Mock<IServiceHelper>();
            
            jwtSettings = new JwtSettings();
            var mockOptions = new Mock<IOptions<JwtSettings>>();
            mockOptions.SetupGet(o => o.Value).Returns(jwtSettings);

            service = new UserService(mapper, mockUow.Object, mockHelper.Object, mockOptions.Object);

            var mockStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockStore.Object, null, null, null, null, null, null, null, null);

            mockUow.Setup(uow => uow.UserManager).Returns(mockUserManager.Object);
        }        

        [Fact]
        public async Task GetUserAsync_Should_ReturnUserDTO()
        {
            var userId = 1;
            var user = new User { UserName = "username", Email = "email" };
            var roles = new List<string> { "role1" };

            mockUow.Setup(uow => uow.Users.GetByIdAsync(userId)).ReturnsAsync(user);
            mockUow.Setup(uow => uow.UserManager.GetRolesAsync(user)).ReturnsAsync(roles);

            var result = await service.GetUserAsync(userId);

            result.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
            result.Roles.Should().BeEquivalentTo(roles);
        }

        [Fact]
        public void GetUserAsync_WrongUserId_Should_ThrowException()
        {
            var userId = 1;

            mockUow.Setup(uow => uow.Users.GetByIdAsync(userId)).ReturnsAsync((User)null);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<User>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.GetUserAsync(userId)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        [Fact]
        public async Task GetUserByUserNameAsync_Should_ReturnUserDTO()
        {
            var userName = "username";
            var user = new User { UserName = "username", Email = "email" };
            var roles = new List<string> { "role1" };

            mockUow.Setup(uow => uow.Users.GetByUserNameAsync(userName)).ReturnsAsync(user);
            mockUow.Setup(uow => uow.UserManager.GetRolesAsync(user)).ReturnsAsync(roles);

            var result = await service.GetUserByUserNameAsync(userName);

            result.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
            result.Roles.Should().BeEquivalentTo(roles);
        }

        [Fact]
        public void GetUserByUserNameAsync_WrongUserId_Should_ThrowException()
        {
            var userName = "username";

            mockUow.Setup(uow => uow.Users.GetByUserNameAsync(userName)).ReturnsAsync((User)null);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<User>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.GetUserByUserNameAsync(userName)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        [Fact]
        public async Task GetUsersAsync_Should_ReturnUserDTOs()
        {            
            var user = new User { UserName = "username", Email = "email" };
            var roles = new List<string> { "role1" };
            var users = new List<User> { user };

            mockUow.Setup(uow => uow.Users.GetAllAsync()).ReturnsAsync(users);
            mockUow.Setup(uow => uow.UserManager.GetRolesAsync(user)).ReturnsAsync(roles);

            var result = await service.GetUsersAsync();

            result.Should().BeEquivalentTo(users, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task CreateUserAsync_Should_CreateUserInUow()
        {
            var dto = new UserRegisterDTO { UserName = "username", Email = "email", Password = "pass" };

            mockUow.Setup(uow => uow.UserManager.FindByEmailAsync(dto.Email)).ReturnsAsync(new User());
            mockUow.Setup(uow => uow.UserManager.FindByNameAsync(dto.UserName)).ReturnsAsync(new User());
            mockUow.Setup(uow => uow.UserManager.CreateAsync(It.Is<User>(u =>
                u.UserName == dto.UserName && u.Email == dto.Email), dto.Password)).Verifiable();
            
            await service.CreateUserAsync(dto);

            mockUow.Verify();
        }

        [Fact]
        public async Task CreateUserAsync_Should_AddUserToRoleInUow()
        {
            var dto = new UserRegisterDTO { UserName = "username", Email = "email", Password = "pass" };

            mockUow.Setup(uow => uow.UserManager.FindByEmailAsync(dto.Email)).ReturnsAsync(new User());
            mockUow.Setup(uow => uow.UserManager.FindByNameAsync(dto.UserName)).ReturnsAsync(new User());
            mockUow.Setup(uow => uow.UserManager.AddToRoleAsync(It.Is<User>(u =>
                u.UserName == dto.UserName && u.Email == dto.Email), "User")).Verifiable();

            await service.CreateUserAsync(dto);

            mockUow.Verify();
        }       

        [Fact]
        public void AddPhotoAsync_WrongEmail_Should_ThrowException()
        {
            var dto = new UserRegisterDTO { Email = "wrongemail" };

            mockUow.Setup(uow => uow.UserManager.FindByEmailAsync(dto.Email)).ReturnsAsync((User)null);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryFailedRegisterExceptionIfModelIsNotNull(
                It.IsAny<User>(), It.IsAny<string>())).Throws<PhotoGalleryFailedRegisterException>();

            service.Invoking(s => s.CreateUserAsync(dto)).Should().Throw<PhotoGalleryFailedRegisterException>();
        }

        [Fact]
        public void AddPhotoAsync_WrongUserName_Should_ThrowException()
        {
            var dto = new UserRegisterDTO { Email = "mail", UserName = "wrongusername" };

            mockUow.Setup(uow => uow.UserManager.FindByEmailAsync(dto.Email)).ReturnsAsync(new User());
            mockUow.Setup(uow => uow.UserManager.FindByNameAsync(dto.UserName)).ReturnsAsync((User)null);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryFailedRegisterExceptionIfModelIsNotNull(
                (User)null, It.IsAny<string>())).Throws<PhotoGalleryFailedRegisterException>();

            service.Invoking(s => s.CreateUserAsync(dto)).Should().Throw<PhotoGalleryFailedRegisterException>();
        }

        [Fact]
        public async Task LoginAsync_Should_ReturnUserWithTokenDTO()
        {
            jwtSettings.LifeTime = TimeSpan.FromMinutes(10);
            jwtSettings.JwtKey = "this is super key";

            var userLoginDTO = new UserLoginDTO { UserName = "username", Password = "pass" };
            var user = new User { UserName = "username", Email = "email" };
            var roles = new List<string> { "role1" };
            
            mockUow.Setup(uow => uow.UserManager.FindByNameAsync(userLoginDTO.UserName)).ReturnsAsync(user);
            mockUow.Setup(uow => uow.UserManager.CheckPasswordAsync(user, userLoginDTO.Password)).ReturnsAsync(true);
            mockUow.Setup(uow => uow.UserManager.GetRolesAsync(user)).ReturnsAsync(roles);

            var result = await service.LoginAsync(userLoginDTO);

            result.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
            result.Roles.Should().BeEquivalentTo(roles);
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_Should_ThrowException()
        {
            var userLoginDTO = new UserLoginDTO { UserName = "username", Password = "pass" };
            var user = new User { UserName = "username", Email = "email" };

            mockUow.Setup(uow => uow.UserManager.FindByNameAsync(userLoginDTO.UserName)).ReturnsAsync(user);
            mockUow.Setup(uow => uow.UserManager.CheckPasswordAsync(user, userLoginDTO.Password)).ReturnsAsync(false);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryFailedLoginExceptionIfModelIsNullOrPasswordInvalid(
                It.IsAny<User>(), It.IsAny<bool>())).Throws<PhotoGalleryFailedLoginException>();

            service.Invoking(s => s.LoginAsync(userLoginDTO)).Should().Throw<PhotoGalleryFailedLoginException>();
        }               

        [Fact]
        public async Task RemoveUserAsync_Should_DeleteUserFromUow()
        {
            var userId = 1;
            var user = new User { UserName = "username", Email = "email" };
            var likes = new List<Like>();
            var comments = new List<Comment>();

            mockUow.Setup(uow => uow.UserManager.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            mockUow.Setup(uow => uow.Likes.FindAsync(It.IsAny<Expression<Func<Like, bool>>>())).ReturnsAsync(likes);
            mockUow.Setup(uow => uow.Comments.FindAsync(It.IsAny<Expression<Func<Comment, bool>>>())).ReturnsAsync(comments);
            mockUow.Setup(uow => uow.Likes.RemoveRange(likes)).Verifiable();
            mockUow.Setup(uow => uow.Comments.RemoveRange(comments)).Verifiable();
            mockUow.Setup(uow => uow.UserManager.DeleteAsync(user)).Verifiable();

            await service.RemoveUserAsync(userId);

            mockUow.Verify();
        }

        [Fact]
        public void RemoveUserAsync_WrongUserId_Should_ThrowException()
        {
            var userId = 1;

            mockUow.Setup(uow => uow.UserManager.FindByIdAsync(userId.ToString())).ReturnsAsync((User)null);

            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<User>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.RemoveUserAsync(userId)).Should().Throw<PhotoGalleryNotFoundException>();
        }
        
    }
}
