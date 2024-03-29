﻿using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.Tests.ControllerTests.Base;
using PhotoGallery.API.Controllers;
using PhotoGallery.API.Models.In;
using PhotoGallery.API.Models.Out;

namespace PhotoGallery.Tests.ControllerTests
{
    public class UserControllerTests : ControllerTestsBase
    {
        int userId = 1;

        IMapper mapper;

        Mock<IUserService> mockPhotoService;
        Mock<ITagService> mockTagService;
        
        UserController controller;

        public UserControllerTests()
        {
            mapper = CreateMapperProfile();
            mockPhotoService = new Mock<IUserService>();
            mockTagService = new Mock<ITagService>();

            controller = new UserController(mapper, mockPhotoService.Object, mockTagService.Object);
            AddIdentity(controller, userId);
        }

        [Fact]
        public void GetUsers_Should_ReturnUserModels()
        {
            var userDTOs = new List<UserDTO> { new UserDTO  { UserName = "username", Email = "email", Roles = new string[] { "User" }, Tags = Array.Empty<string>() } };

            mockPhotoService.Setup(s => s.GetUsersAsync()).ReturnsAsync(userDTOs).Verifiable();

            controller.GetUsers().Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(userDTOs);
            mockPhotoService.Verify();
        }

        [Fact]
        public void GetUser_Should_ReturnUserModel()
        {
            var userDTO = new UserDTO  { UserName = "username", Email = "email", Roles = new string[] { "User" }, Tags = Array.Empty<string>() };

            mockPhotoService.Setup(s => s.GetUserAsync(userId)).ReturnsAsync(userDTO).Verifiable();

            controller.GetUser(userId).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(userDTO);
            mockPhotoService.Verify();
        }

        [Fact]
        public void GetUserByUserName_Should_ReturnUserModel()
        {
            var userName = "username";
            var userDTO = new UserDTO { UserName = "username", Email = "email", Roles = new string[] { "User" }, Tags = Array.Empty<string>() };

            mockPhotoService.Setup(s => s.GetUserByUserNameAsync(userName)).ReturnsAsync(userDTO).Verifiable();

            controller.GetUserByUserName(userName).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(userDTO);
            mockPhotoService.Verify();
        }

        [Fact]
        public void RegisterUser_Should_ReturnOk()
        {
            var userModel = new UserRegisterModel { UserName = "username", Email = "email", Password = "pass"};

            mockPhotoService.Setup(s => s.CreateUserAsync(It.Is<UserRegisterDTO>(dto =>
                dto.UserName == userModel.UserName && dto.Email == userModel.Email))).Verifiable();

            controller.RegisterUser(userModel).Result.Should().BeOfType<OkResult>();
            mockPhotoService.Verify();
        }

        [Fact]
        public void LoginUser_Should_ReturnUserWithTokenModel()
        {
            var userModel = new UserLoginModel { UserName = "username", Password = "pass" };
            var userDTO = new UserWithTokenDTO { UserName = "username" };

            mockPhotoService.Setup(s => s.LoginAsync(It.Is<UserLoginDTO>(dto =>
                dto.UserName == userModel.UserName && dto.Password == userModel.Password))).ReturnsAsync(userDTO).Verifiable();

            controller.LoginUser(userModel).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(userModel, options => options.ExcludingMissingMembers());
            mockPhotoService.Verify();
        }

        [Fact]
        public void DeleteUser_Should_ReturnUserModel()
        {
            mockPhotoService.Setup(s => s.RemoveUserAsync(userId)).Verifiable();

            controller.DeleteUser(userId).Result.Should().BeOfType<OkResult>();
            mockPhotoService.Verify();
        }

    }
}
